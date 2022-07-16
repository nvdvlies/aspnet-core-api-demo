import { Injectable, OnDestroy } from '@angular/core';
import {
  AbstractControl,
  UntypedFormArray,
  UntypedFormControl,
  UntypedFormGroup,
  Validators
} from '@angular/forms';
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
import { Permission } from '@shared/enums/permission.enum';
import { UserPermissionService } from '@shared/services/user-permission.service';

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
export type CurrentUserFormGroup = UntypedFormGroup & { controls: CurrentUserControls };

type UserRoleControls = { [key in keyof IUserRoleDto]-?: AbstractControl };
export type UserRoleFormGroup = UntypedFormGroup & {
  controls: UserRoleControls;
};

export type UserRoleFormArray = UntypedFormArray & {
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
  protected readPermission?: keyof typeof Permission = undefined;
  protected writePermission?: keyof typeof Permission = undefined;

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
    userPermissionService: UserPermissionService,
    private readonly currentUserStoreService: CurrentUserStoreService
  ) {
    super(route, userPermissionService);
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
      id: new UntypedFormControl(super.readonlyFormState),
      birthDate: new UntypedFormControl(super.readonlyFormState),
      email: new UntypedFormControl(super.readonlyFormState),
      externalId: new UntypedFormControl(super.readonlyFormState),
      givenName: new UntypedFormControl(null),
      middleName: new UntypedFormControl(null),
      familyName: new UntypedFormControl(null, [Validators.required]),
      fullname: new UntypedFormControl(super.readonlyFormState),
      gender: new UntypedFormControl(super.readonlyFormState),
      userRoles: new UntypedFormArray([] as UserRoleFormGroup[]) as UserRoleFormArray,
      deleted: new UntypedFormControl(super.readonlyFormState),
      deletedBy: new UntypedFormControl(super.readonlyFormState),
      deletedOn: new UntypedFormControl(super.readonlyFormState),
      createdBy: new UntypedFormControl(super.readonlyFormState),
      createdOn: new UntypedFormControl(super.readonlyFormState),
      lastModifiedBy: new UntypedFormControl(super.readonlyFormState),
      lastModifiedOn: new UntypedFormControl(super.readonlyFormState),
      xmin: new UntypedFormControl(super.readonlyFormState)
    };
    return new UntypedFormGroup(controls) as CurrentUserFormGroup;
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
