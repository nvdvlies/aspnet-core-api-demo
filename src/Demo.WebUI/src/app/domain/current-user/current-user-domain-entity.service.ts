import { Injectable, OnDestroy } from '@angular/core';
import { AbstractControl, FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { combineLatest, Observable, of } from 'rxjs';
import { debounceTime, map } from 'rxjs/operators';
import { CurrentUserDto, ICurrentUserDto, IUserRoleDto } from '@api/api.generated.clients';
import { CurrentUserStoreService } from '@domain/current-user/current-user-store.service';
import {
  DomainEntityBase,
  DomainEntityContext,
  IDomainEntityContext
} from '@domain/shared/domain-entity-base';

export interface ICurrentUserDomainEntityContext extends IDomainEntityContext<CurrentUserDto> {}

export class CurrentUserDomainEntityContext
  extends DomainEntityContext<CurrentUserDto>
  implements ICurrentUserDomainEntityContext
{
  constructor() {
    super();
  }
}

type CurrentUserControls = { [key in keyof ICurrentUserDto]-?: AbstractControl };
export type CurrentUserFormGroup = FormGroup & { controls: CurrentUserControls };

type UserRoleControls = { [key in keyof IUserRoleDto]-?: AbstractControl };
export type UserRoleFormGroup = FormGroup & {
  controls: UserRoleControls;
};

export type UserRoleFormArray = FormArray & {
  controls: UserRoleFormGroup[];
};

@Injectable()
export class CurrentUserDomainEntityService
  extends DomainEntityBase<CurrentUserDto>
  implements OnDestroy
{
  protected createFunction = (currentUser: CurrentUserDto) =>
    this.currentUserStoreService.save(currentUser);
  protected readFunction = (id?: string) => this.currentUserStoreService.get();
  protected updateFunction = (currentUser: CurrentUserDto) =>
    this.currentUserStoreService.save(currentUser);
  protected deleteFunction = (id?: string) => of(void 0);
  protected entityUpdatedEvent$ = this.currentUserStoreService.currentUserUpdatedInStore$;

  public observe$: Observable<CurrentUserDomainEntityContext> = combineLatest([
    this.observeInternal$
  ]).pipe(
    debounceTime(0),
    map(([baseContext]) => {
      const context: CurrentUserDomainEntityContext = {
        ...baseContext
      };
      return context;
    })
  );

  constructor(
    route: ActivatedRoute,
    private readonly currentUserStoreService: CurrentUserStoreService
  ) {
    super(route);
    super.init();
  }

  public override get form(): CurrentUserFormGroup {
    return super.form as CurrentUserFormGroup;
  }
  public override set form(value: CurrentUserFormGroup) {
    super.form = value;
  }

  protected instantiateForm(): void {
    this.form = this.buildFormGroup();
  }

  private buildFormGroup(): CurrentUserFormGroup {
    const controls: CurrentUserControls = {
      id: new FormControl(super.readonlyFormState),
      birthDate: new FormControl(super.readonlyFormState),
      email: new FormControl(super.readonlyFormState),
      externalId: new FormControl(super.readonlyFormState),
      givenName: new FormControl(null),
      middleName: new FormControl(null),
      familyName: new FormControl(null, [Validators.required]),
      fullname: new FormControl(super.readonlyFormState),
      gender: new FormControl(super.readonlyFormState),
      userRoles: new FormArray([] as UserRoleFormGroup[]) as UserRoleFormArray,
      deleted: new FormControl(super.readonlyFormState),
      deletedBy: new FormControl(super.readonlyFormState),
      deletedOn: new FormControl(super.readonlyFormState),
      createdBy: new FormControl(super.readonlyFormState),
      createdOn: new FormControl(super.readonlyFormState),
      lastModifiedBy: new FormControl(super.readonlyFormState),
      lastModifiedOn: new FormControl(super.readonlyFormState),
      xmin: new FormControl(super.readonlyFormState)
    };
    return new FormGroup(controls) as CurrentUserFormGroup;
  }

  protected instantiateNewEntity(): Observable<CurrentUserDto> {
    return of(new CurrentUserDto());
  }

  public override read(): Observable<null> {
    return super.read();
  }

  public save(): Observable<CurrentUserDto> {
    return super.upsert();
  }

  public override reset(): void {
    super.reset();
  }
}
