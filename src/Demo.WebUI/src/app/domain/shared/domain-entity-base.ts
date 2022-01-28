import { Injectable, OnDestroy } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { BehaviorSubject, combineLatest, Observable, of, Subject, throwError } from 'rxjs';
import { catchError, debounceTime, filter, finalize, map, switchMap, take, takeUntil, tap } from 'rxjs/operators';
import { ApiException, ProblemDetails, ValidationProblemDetails } from '@api/api.generated.clients';
import { MergeUtil } from '@domain/shared/merge.util';

export class InitFromRouteOptions {
  parameterName: string = "id";
  newValue: string = "new";
}

export interface IDomainEntityContext<T> {
  id: string | null;
  entity: Readonly<T> | undefined;
  pristine: Readonly<T> | undefined;
  isLoading: boolean;
  isSaving: boolean;
  isDeleting: boolean;
  hasNewerVersionWithMergeConflict: boolean;
  problemDetails: ValidationProblemDetails | ProblemDetails | undefined;
}

export class DomainEntityContext<T> implements DomainEntityContext<T> {
  constructor() {
    this.id = null;
    this.isLoading = false;
    this.isSaving = false;
    this.isDeleting = false;
    this.hasNewerVersionWithMergeConflict = false;
  }

  id: string | null;
  entity: Readonly<T> | undefined;
  pristine: Readonly<T> | undefined;
  isLoading: boolean;
  isSaving: boolean;
  isDeleting: boolean;
  hasNewerVersionWithMergeConflict: boolean;
  problemDetails: ValidationProblemDetails | ProblemDetails | undefined;
}

export interface DomainEntity<T> {
  id: string | undefined;
  createdOn: Date;
  lastModifiedOn?: Date | undefined;
  clone(): T;
}

@Injectable()
export abstract class DomainEntityBase<T extends DomainEntity<T>> implements OnDestroy {
  protected abstract instantiateForm(): void;
  protected abstract instantiateNewEntity(): T;
  protected abstract getByIdFunction: (id: string) => Observable<T>;
  protected abstract createFunction: (entity: T) => Observable<T>;
  protected abstract updateFunction: (entity: T) => Observable<T>;
  protected abstract deleteFunction: (id: string) => Observable<void>;
  protected afterPatchEntityToFormHook?(entity: T): void;
  protected abstract entityUpdatedEvent$: Observable<[any, T]>;

  protected readonly id = new BehaviorSubject<string | undefined>(undefined);
  protected readonly entity = new BehaviorSubject<Readonly<T> | undefined>(undefined);
  protected readonly pristine = new BehaviorSubject<Readonly<T> | undefined>(undefined);
  protected readonly isLoading = new BehaviorSubject<boolean>(false);
  protected readonly isSaving = new BehaviorSubject<boolean>(false);
  protected readonly isDeleting = new BehaviorSubject<boolean>(false);
  protected readonly hasNewerVersionWithMergeConflict = new BehaviorSubject<boolean>(false);
  protected readonly problemDetails = new BehaviorSubject<ValidationProblemDetails | ProblemDetails | ApiException | undefined>(undefined);
  protected readonly onDestroy = new Subject<void>();

  protected id$ = this.id.asObservable();
  protected entity$ = this.entity.asObservable();
  protected pristine$ = this.pristine.asObservable();
  protected isLoading$ = this.isLoading.asObservable();
  protected isSaving$ = this.isSaving.asObservable();
  protected isDeleting$ = this.isDeleting.asObservable();
  protected hasNewerVersionWithMergeConflict$ = this.hasNewerVersionWithMergeConflict.asObservable();
  protected problemDetails$ = this.problemDetails.asObservable();
  protected onDestroy$ = this.onDestroy.asObservable();

  protected readonly readonlyFormState: any = { value: null, disabled: true };

  protected observeInternal$ = combineLatest([
    this.id$,
    this.entity$,
    this.pristine$,
    this.isLoading$,
    this.isSaving$,
    this.isDeleting$,
    this.hasNewerVersionWithMergeConflict$,
    this.problemDetails$,
  ])
    .pipe(
      debounceTime(0),
      map(([
        id,
        entity,
        pristine,
        isLoading,
        isSaving,
        isDeleting,
        hasNewerVersionWithMergeConflict,
        problemDetails
      ]) => {
        return {
          id,
          entity,
          pristine,
          isLoading,
          isSaving,
          isDeleting,
          hasNewerVersionWithMergeConflict,
          problemDetails
        } as DomainEntityContext<T>;
      })
    ) as Observable<DomainEntityContext<T>>;

  private _form: FormGroup | undefined;
  protected get form(): FormGroup {
    return this._form!;
  }
  protected set form(value: FormGroup) {
    this._form = value;
  }

  constructor(
    protected readonly route: ActivatedRoute
  ) {
  }

  protected init(): void {
    this.instantiateForm();
    this.subscribeToEntityUpdatedEvent();
  }

  protected new(): Observable<null> {
    this.problemDetails.next(undefined);
    return new Observable(observer => {
      try {
        this.isLoading.next(true);
        const entity = this.instantiateNewEntity();
        this.entity.next(entity);
        this.patchEntityToForm(entity);
        this.pristine.next(entity.clone());
      }
      finally {
        this.isLoading.next(false);
        observer.next();
        observer.complete();
      }
    });
  }

  protected getById(id: string, getByIdFunction?: (id: string) => Observable<T>): Observable<null> {
    this.problemDetails.next(undefined);
    this.isLoading.next(true);
    getByIdFunction ??= this.getByIdFunction;
    return getByIdFunction(id)
      .pipe(
        catchError((error: ValidationProblemDetails | ProblemDetails | ApiException) => this.setProblemDetailsAndRethrow(error)),
        map((entity: T) => entity.clone()),
        tap((entity: T) => {
          this.id.next(entity.id);
          this.entity.next(entity);
          this.patchEntityToForm(entity);
          this.pristine.next(entity.clone());
        }),
        switchMap(() => of<null>(null)),
        finalize(() => this.isLoading.next(false))
      );
  }

  protected initFromRoute(options: InitFromRouteOptions | undefined, getByIdFunction?: (id: string) => Observable<T>): Observable<null> {
    this.problemDetails.next(undefined);
    options ??= new InitFromRouteOptions();

    const id = this.route.snapshot.paramMap.get(options!.parameterName);
    if (id == null) {
      return throwError(() => new Error(`Couldn't find parameter '${options!.parameterName}' in route parameters.`));
    }

    if (id === options!.newValue) {
      return this.new();
    } else {
      getByIdFunction ??= this.getByIdFunction;
      return this.getById(id, getByIdFunction);
    } 
  }

  protected create(createFunction?: (entity: T) => Observable<T>): Observable<T> {
    if (!this.form.valid) {
      return throwError(() => new Error('Form is not valid.'));
    }
    this.isSaving.next(true);
    this.problemDetails.next(undefined);
    this.patchFormToEntity();
    createFunction ??= this.createFunction;
    return createFunction(this.entity.value!)
      .pipe(
        catchError((error: ValidationProblemDetails | ProblemDetails | ApiException) => this.setProblemDetailsAndRethrow(error)),
        finalize(() => this.isSaving.next(false))
      )
  }

  protected update(updateFunction?: (entity: T) => Observable<T>): Observable<T> {
    if (!this.form.valid) {
      return throwError(() => new Error('Form is not valid.'));
    }
    this.isSaving.next(true);
    this.problemDetails.next(undefined);
    this.patchFormToEntity();
    updateFunction ??= this.updateFunction;
    return updateFunction(this.entity.value!)
      .pipe(
        catchError((error: ValidationProblemDetails | ProblemDetails | ApiException) => this.setProblemDetailsAndRethrow(error)),
        finalize(() => this.isSaving.next(false))
      )
  }

  protected upsert(createFunction?: (entity: T) => Observable<T>, updateFunction?: (entity: T) => Observable<T>): Observable<T> {
    return this.id.value == null ? this.create(createFunction): this.update(updateFunction);
  }

  protected delete(deleteFunction?: (id: string) => Observable<void>): Observable<void> {
    if (!this.id.value) {
      return throwError(() => new Error('\'Id\' is not defined.'));
    }
    this.problemDetails.next(undefined);
    this.isDeleting.next(true);
    deleteFunction ??= this.deleteFunction;
    return deleteFunction(this.id.value)
      .pipe(
        catchError((error: ValidationProblemDetails | ProblemDetails | ApiException) => this.setProblemDetailsAndRethrow(error)),
        finalize(() => this.isDeleting.next(false))
      )
  }

  public resolveMergeConflictWithTakeTheirs(): void {
    if (this.pristine.value == null) {
      return;
    }
    if (this.hasNewerVersionWithMergeConflict.value == false) {
      return;
    }
    this.patchEntityToForm(this.pristine.value);
    this.hasNewerVersionWithMergeConflict.next(false);
  }

  public hasErrors(): boolean {
    return this.problemDetails.value != null;
  }

  protected patchEntityToForm(entity: T): void {
    this.form.reset();
    this.form.patchValue({
      ...entity
    });
    this.afterPatchEntityToFormHook?.(entity);
  }

  protected tryMerge(updated: T, pristine: T, form: FormGroup): boolean {
    if (MergeUtil.hasMergeConflictInFormGroup(updated, pristine, form)) {
      return false;
    }
    MergeUtil.mergeIntoFormGroup(updated, form);
    return true;
  }

  protected patchFormToEntity(): void {
    this.entity.next(Object.assign(this.entity.value, this.form.getRawValue()));
  }

  protected setProblemDetailsAndRethrow(problemDetails: ValidationProblemDetails | ProblemDetails | ApiException): Observable<never> {
    this.problemDetails.next(problemDetails);
    return throwError(() => new Error('An error occured. See \'problemDetails\' for more information')); 
  }

  private subscribeToEntityUpdatedEvent(): void {
    this.entityUpdatedEvent$
      .pipe(
        takeUntil(this.onDestroy$),
        filter(([_, entity]) => this.id.value != null && entity.id == this.id.value),
      )
      .subscribe(([_, entity]) => {
        if (this.pristine.value == null) {
          return;
        }
        const currentLastModifiedOn = this.pristine.value.lastModifiedOn ?? this.pristine.value.createdOn;
        const newLastModifiedOn = entity.lastModifiedOn ?? entity.createdOn;
        if (newLastModifiedOn <= currentLastModifiedOn) {
          return;
        }
  
        const hasMergeConflict = !this.tryMerge(entity, this.pristine.value, this.form);
        if (hasMergeConflict) {
          this.hasNewerVersionWithMergeConflict.next(true);
        }

        this.pristine.next(entity.clone());
      });
  }

  protected reset(): void {
    this.id.next(undefined);
    this.entity.next(undefined);
    this.pristine.next(undefined);
    this.isLoading.next(false);
    this.isSaving.next(false);
    this.isDeleting.next(false);
    this.hasNewerVersionWithMergeConflict.next(false);
    this.problemDetails.next(undefined);
    this.form.reset();
  }

  ngOnDestroy(): void {
    this.onDestroy.next();
    this.onDestroy.complete();
  }
}