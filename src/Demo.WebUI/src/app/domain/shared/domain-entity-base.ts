import { Injectable, OnDestroy } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { BehaviorSubject, combineLatest, Observable, of, Subject, throwError } from 'rxjs';
import {
  catchError,
  debounceTime,
  filter,
  finalize,
  map,
  switchMap,
  takeUntil,
  tap
} from 'rxjs/operators';
import { ApiException, ProblemDetails, ValidationProblemDetails } from '@api/api.generated.clients';
import { MergeUtil } from '@domain/shared/merge.util';

export class InitFromRouteOptions {
  parameterName: string = 'id';
  newValue: string = 'new';
}

export interface IDomainEntityContext<T> {
  id: string | null;
  entity: Readonly<T> | undefined;
  pristine: Readonly<T> | undefined;
  isLoading: boolean;
  isSaving: boolean;
  isDeleting: boolean;
  hasNewerVersionWithMergeConflict: boolean;
  problemDetails: ValidationProblemDetails | ProblemDetails | ApiException | undefined;
  loadingEntityFailed: boolean;
}

export class DomainEntityContext<T> implements IDomainEntityContext<T> {
  constructor() {
    this.id = null;
    this.isLoading = false;
    this.isSaving = false;
    this.isDeleting = false;
    this.hasNewerVersionWithMergeConflict = false;
    this.loadingEntityFailed = false;
  }

  id: string | null;
  entity: Readonly<T> | undefined;
  pristine: Readonly<T> | undefined;
  isLoading: boolean;
  isSaving: boolean;
  isDeleting: boolean;
  hasNewerVersionWithMergeConflict: boolean;
  problemDetails: ValidationProblemDetails | ProblemDetails | ApiException | undefined;
  loadingEntityFailed: boolean;
}

export interface IDomainEntity<T> {
  id: string | undefined;
  createdOn: Date | undefined;
  lastModifiedOn?: Date | undefined;
  clone(): T;
}

@Injectable()
export abstract class BaseDomainEntityService {
  public abstract getErrorMessage(errorKey: string, errorValue: any): string | undefined;
}

@Injectable()
export abstract class DomainEntityBase<T extends IDomainEntity<T>>
  implements BaseDomainEntityService, OnDestroy
{
  protected abstract instantiateForm(): void;
  protected abstract instantiateNewEntity(): Observable<T>;
  protected abstract createFunction: (entity: T) => Observable<T>;
  protected abstract readFunction: (id?: string) => Observable<T>;
  protected abstract updateFunction: (entity: T) => Observable<T>;
  protected abstract deleteFunction: (id?: string) => Observable<void>;
  protected afterPatchEntityToFormHook?(entity: T): void;
  public afterNewHook?: (entity: T) => Observable<null>;
  public afterReadHook?: (entity: T) => Observable<null>;
  protected abstract entityUpdatedEvent$: Observable<[any, T]>;

  protected readonly id = new BehaviorSubject<string | undefined>(undefined);
  protected readonly entity = new BehaviorSubject<Readonly<T> | undefined>(undefined);
  protected readonly pristine = new BehaviorSubject<Readonly<T> | undefined>(undefined);
  protected readonly isLoading = new BehaviorSubject<boolean>(false);
  protected readonly isSaving = new BehaviorSubject<boolean>(false);
  protected readonly isDeleting = new BehaviorSubject<boolean>(false);
  protected readonly hasNewerVersionWithMergeConflict = new BehaviorSubject<boolean>(false);
  protected readonly problemDetails = new BehaviorSubject<
    ValidationProblemDetails | ProblemDetails | ApiException | undefined
  >(undefined);
  protected readonly loadingEntityFailed = new BehaviorSubject<boolean>(false);
  protected readonly onDestroy = new Subject<void>();

  protected id$ = this.id.asObservable();
  protected entity$ = this.entity.asObservable();
  protected pristine$ = this.pristine.asObservable();
  protected isLoading$ = this.isLoading.asObservable();
  protected isSaving$ = this.isSaving.asObservable();
  protected isDeleting$ = this.isDeleting.asObservable();
  protected hasNewerVersionWithMergeConflict$ =
    this.hasNewerVersionWithMergeConflict.asObservable();
  protected problemDetails$ = this.problemDetails.asObservable();
  protected loadingEntityFailed$ = this.loadingEntityFailed.asObservable();
  protected onDestroy$ = this.onDestroy.asObservable();

  protected get readonlyFormState(): any {
    return { value: null, disabled: true };
  }

  protected observeInternal$ = combineLatest([
    this.id$,
    this.entity$,
    this.pristine$,
    this.isLoading$,
    this.isSaving$,
    this.isDeleting$,
    this.hasNewerVersionWithMergeConflict$,
    this.problemDetails$,
    this.loadingEntityFailed$
  ]).pipe(
    debounceTime(0),
    map(
      ([
        id,
        entity,
        pristine,
        isLoading,
        isSaving,
        isDeleting,
        hasNewerVersionWithMergeConflict,
        problemDetails,
        loadingEntityFailed
      ]) => {
        return {
          id,
          entity,
          pristine,
          isLoading,
          isSaving,
          isDeleting,
          hasNewerVersionWithMergeConflict,
          problemDetails,
          loadingEntityFailed
        } as DomainEntityContext<T>;
      }
    )
  ) as Observable<DomainEntityContext<T>>;

  private _form: FormGroup | undefined;
  protected get form(): FormGroup {
    return this._form!;
  }
  protected set form(value: FormGroup) {
    this._form = value;
  }

  constructor(protected readonly route: ActivatedRoute) {}

  protected init(): void {
    this.instantiateForm();
    this.subscribeToEntityUpdatedEvent();
  }

  protected new(): Observable<null> {
    this.reset();
    this.isLoading.next(true);
    return this.instantiateNewEntity().pipe(
      map((entity: T) => {
        this.entity.next(entity);
        this.patchEntityToForm(entity);
        this.pristine.next(entity.clone());
        return entity;
      }),
      switchMap((entity: T) => this.afterNewHook?.(entity).pipe(map(() => null)) ?? of<null>(null)),
      finalize(() => this.isLoading.next(false))
    );
  }

  protected read(id?: string, readFunction?: (id?: string) => Observable<T>): Observable<null> {
    this.reset();
    this.isLoading.next(true);
    readFunction ??= this.readFunction;
    return readFunction(id).pipe(
      catchError((error: ValidationProblemDetails | ProblemDetails | ApiException) => {
        this.loadingEntityFailed.next(true);
        return this.setProblemDetailsAndRethrow(error);
      }),
      map((entity: T) => entity.clone()),
      tap((entity: T) => {
        this.id.next(entity.id);
        this.entity.next(entity);
        this.patchEntityToForm(entity);
        this.pristine.next(entity.clone());
      }),
      switchMap(
        (entity: T) => this.afterReadHook?.(entity).pipe(map(() => null)) ?? of<null>(null)
      ),
      finalize(() => this.isLoading.next(false))
    );
  }

  protected initFromRoute(
    options: InitFromRouteOptions | undefined,
    readFunction?: (id?: string) => Observable<T>
  ): Observable<null> {
    this.problemDetails.next(undefined);
    options ??= new InitFromRouteOptions();

    const id = this.route.snapshot.paramMap.get(options!.parameterName);
    if (id == null) {
      return throwError(
        () => new Error(`Couldn't find parameter '${options!.parameterName}' in route parameters.`)
      );
    }

    if (id === options!.newValue) {
      return this.new();
    } else {
      readFunction ??= this.readFunction;
      return this.read(id, readFunction);
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
    return createFunction(this.entity.value!).pipe(
      catchError((error: ValidationProblemDetails | ProblemDetails | ApiException) =>
        this.setProblemDetailsAndRethrow(error)
      ),
      map((entity: T) => entity.clone()),
      tap((entity) => {
        this.entity.next(entity);
        this.patchEntityToForm(entity);
        this.pristine.next(entity.clone());
      }),
      finalize(() => this.isSaving.next(false))
    );
  }

  protected update(updateFunction?: (entity: T) => Observable<T>): Observable<T> {
    if (!this.form.valid) {
      return throwError(() => new Error('Form is not valid.'));
    }
    this.isSaving.next(true);
    this.problemDetails.next(undefined);
    this.patchFormToEntity();
    updateFunction ??= this.updateFunction;
    return updateFunction(this.entity.value!).pipe(
      catchError((error: ValidationProblemDetails | ProblemDetails | ApiException) =>
        this.setProblemDetailsAndRethrow(error)
      ),
      map((entity: T) => entity.clone()),
      tap((entity) => {
        this.entity.next(entity);
        this.patchEntityToForm(entity);
        this.pristine.next(entity.clone());
      }),
      finalize(() => this.isSaving.next(false))
    );
  }

  protected upsert(
    createFunction?: (entity: T) => Observable<T>,
    updateFunction?: (entity: T) => Observable<T>
  ): Observable<T> {
    return this.id.value == null ? this.create(createFunction) : this.update(updateFunction);
  }

  protected delete(deleteFunction?: (id: string) => Observable<void>): Observable<void> {
    if (!this.id.value) {
      return throwError(() => new Error("'Id' is not defined."));
    }
    this.problemDetails.next(undefined);
    this.isDeleting.next(true);
    deleteFunction ??= this.deleteFunction;
    return deleteFunction(this.id.value).pipe(
      catchError((error: ValidationProblemDetails | ProblemDetails | ApiException) =>
        this.setProblemDetailsAndRethrow(error)
      ),
      tap(() => this.form.reset()),
      finalize(() => this.isDeleting.next(false))
    );
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
    this.form.reset({
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

  protected setProblemDetailsAndRethrow(
    problemDetails: ValidationProblemDetails | ProblemDetails | ApiException
  ): Observable<never> {
    this.problemDetails.next(problemDetails);
    return throwError(
      () => new Error("An error occured. See 'problemDetails' for more information")
    );
  }

  private subscribeToEntityUpdatedEvent(): void {
    this.entityUpdatedEvent$
      .pipe(
        takeUntil(this.onDestroy$),
        filter(() => this.isSaving.value == false),
        filter(([_, entity]) => this.id.value != null && entity.id == this.id.value)
      )
      .subscribe(([_, entity]) => {
        if (this.pristine.value == null) {
          return;
        }
        const currentLastModifiedOn =
          this.pristine.value.lastModifiedOn ?? this.pristine.value.createdOn;
        const newLastModifiedOn = entity.lastModifiedOn ?? entity.createdOn!;
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

  public getErrorMessage(errorKey: string, errorValue: any): string | undefined {
    switch (errorKey) {
      case 'required':
        return 'Field is required.';
      case 'maxlength':
        return `Field must be ${errorValue.requiredLength} characters or fewer. You entered ${errorValue.actualLength} character(s).`;
      case 'minlength':
        return `Field must be at least ${errorValue.requiredLength} characters. You entered ${errorValue.actualLength} character(s).`;
      case 'min':
        return `Field must be greater than or equal to ${errorValue.min}`;
      case 'max':
        return `Field must be less than or equal to ${errorValue.max}`;
      case 'pattern':
        return 'Field is not in the correct format.';
      case 'email':
        return 'Field must be a valid email address.';
      case 'serverError':
        return errorValue;
      default:
        return undefined;
    }
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
    this.loadingEntityFailed.next(false);
    this.form.reset();
  }

  ngOnDestroy(): void {
    this.onDestroy.next();
    this.onDestroy.complete();
  }
}
