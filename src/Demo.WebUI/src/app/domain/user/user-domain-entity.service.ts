import { Injectable, OnDestroy } from '@angular/core';
import {
  AbstractControl,
  AsyncValidatorFn,
  FormArray,
  FormControl,
  FormGroup,
  ValidationErrors,
  ValidatorFn,
  Validators
} from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { combineLatest, Observable, of, BehaviorSubject } from 'rxjs';
import { debounceTime, delay, map, switchMap, tap } from 'rxjs/operators';
import {
  UserDto,
  IUserDto,
  IUserRoleDto,
  ApiUsersClient,
  IsEmailAvailableQueryResult,
  UserRoleDto,
  UserTypeEnum
} from '@api/api.generated.clients';
import { UserStoreService } from '@domain/user/user-store.service';
import {
  DomainEntityBase,
  DomainEntityContext,
  IDomainEntityContext,
  InitFromRouteOptions
} from '@domain/shared/domain-entity-base';

export interface IUserDomainEntityContext extends IDomainEntityContext<UserDto> {
  isSystemUser: boolean;
}

export class UserDomainEntityContext
  extends DomainEntityContext<UserDto>
  implements IUserDomainEntityContext
{
  constructor() {
    super();
  }

  public isSystemUser: boolean = false;
}

type UserControls = { [key in keyof IUserDto]-?: AbstractControl };
export type UserFormGroup = FormGroup & { controls: UserControls };

type UserRoleControls = { [key in keyof IUserRoleDto]-?: AbstractControl };
export type UserRoleFormGroup = FormGroup & {
  controls: UserRoleControls;
};

export type UserRoleFormArray = FormArray & {
  controls: UserRoleFormGroup[];
};

export class UserInitFromRouteOptions extends InitFromRouteOptions {
  public isSystemUser: boolean | undefined;
}

@Injectable()
export class UserDomainEntityService extends DomainEntityBase<UserDto> implements OnDestroy {
  protected createFunction = (user: UserDto) => this.userStoreService.create(user);
  protected readFunction = (id?: string) => this.userStoreService.getById(id!);
  protected updateFunction = (user: UserDto) => this.userStoreService.update(user);
  protected deleteFunction = (id?: string) => this.userStoreService.delete(id!);
  protected entityUpdatedEvent$ = this.userStoreService.userUpdatedInStore$;

  protected readonly isSystemUser = new BehaviorSubject<boolean>(false);

  protected isSystemUser$ = this.isSystemUser.asObservable();

  public observe$: Observable<UserDomainEntityContext> = combineLatest([
    this.observeInternal$,
    this.isSystemUser$
  ]).pipe(
    debounceTime(0),
    map(([baseContext, isSystemUser]) => {
      const context: UserDomainEntityContext = {
        ...baseContext,
        isSystemUser
      };
      return context;
    })
  );

  constructor(
    route: ActivatedRoute,
    private readonly userStoreService: UserStoreService,
    private readonly apiUsersClient: ApiUsersClient
  ) {
    super(route);
    super.init();
  }

  public override get form(): UserFormGroup {
    return super.form as UserFormGroup;
  }
  public override set form(value: UserFormGroup) {
    super.form = value;
  }

  protected instantiateForm(): void {
    this.form = this.buildUserFormGroup();
  }

  private buildUserFormGroup(): UserFormGroup {
    const controls: UserControls = {
      id: new FormControl(super.readonlyFormState),
      externalId: new FormControl(super.readonlyFormState),
      fullname: new FormControl(super.readonlyFormState),
      givenName: new FormControl(null),
      familyName: new FormControl(null, [Validators.required], []),
      middleName: new FormControl(null),
      email: new FormControl(
        null,
        [Validators.required, Validators.email],
        [this.uniqueEmailValidator()]
      ),
      gender: new FormControl(null),
      userType: new FormControl(null),
      birthDate: new FormControl(null),
      userRoles: new FormArray(
        [] as UserRoleFormGroup[],
        [Validators.required, this.uniqueRolesValidator()],
        []
      ) as UserRoleFormArray,
      deleted: new FormControl(super.readonlyFormState),
      deletedBy: new FormControl(super.readonlyFormState),
      deletedOn: new FormControl(super.readonlyFormState),
      createdBy: new FormControl(super.readonlyFormState),
      createdOn: new FormControl(super.readonlyFormState),
      lastModifiedBy: new FormControl(super.readonlyFormState),
      lastModifiedOn: new FormControl(super.readonlyFormState),
      xmin: new FormControl(super.readonlyFormState)
    };
    return new FormGroup(controls) as UserFormGroup;
  }

  private buildUserRoleFormGroup(): UserRoleFormGroup {
    const controls: UserRoleControls = {
      roleId: new FormControl(null, [Validators.required], [])
    };
    return new FormGroup(controls) as UserRoleFormGroup;
  }

  public addUserRole(): void {
    const newUserRoleFormGroup = this.buildUserRoleFormGroup();
    this.userRoleFormArray.push(newUserRoleFormGroup);
  }

  public removeUserRole(index: number): void {
    if (this.userRoleFormArray.length > 1) {
      this.userRoleFormArray.removeAt(index);
    }
  }

  protected instantiateNewEntity(): Observable<UserDto> {
    const user = new UserDto();
    user.userRoles = [new UserRoleDto()];
    user.userType = this.isSystemUser.value ? UserTypeEnum.System : UserTypeEnum.Regular;
    return of(user);
  }

  public get userRoles(): UserRoleFormGroup[] {
    return this.userRoleFormArray.controls as UserRoleFormGroup[];
  }

  private get userRoleFormArray(): FormArray {
    return this.form.controls.userRoles as FormArray;
  }

  public override new(isSystemUser?: boolean): Observable<null> {
    if (isSystemUser) {
      this.isSystemUser.next(true);
    }
    return super.new().pipe(
      tap(() => {
        if (this.entity.value?.userType === UserTypeEnum.System) {
          this.updateFormForSystemUserType();
          this.isSystemUser.next(true);
        } else {
          this.isSystemUser.next(false);
        }
      })
    );
  }

  public override read(id: string): Observable<null> {
    return super.read(id).pipe(
      tap(() => {
        if (this.entity.value?.userType === UserTypeEnum.System) {
          this.updateFormForSystemUserType();
          this.isSystemUser.next(true);
        } else {
          this.isSystemUser.next(false);
        }
      })
    );
  }

  private updateFormForSystemUserType(): void {
    this.form.controls.externalId.addValidators(Validators.required);
    this.form.controls.externalId.enable();
  }

  public override initFromRoute(options?: UserInitFromRouteOptions | undefined): Observable<null> {
    if (options?.isSystemUser) {
      this.isSystemUser.next(true);
    }
    return super.initFromRoute(options);
  }

  public override create(): Observable<UserDto> {
    return super.create();
  }

  public override update(): Observable<UserDto> {
    return super.update();
  }

  public override upsert(): Observable<UserDto> {
    return super.upsert();
  }

  public override delete(): Observable<void> {
    return super.delete();
  }

  protected override afterPatchEntityToFormHook(user: UserDto): void {
    // form.patchValue doesnt modify FormArray structure, so we need to do this manually afterwards.
    this.patchUserRolesToForm(user);
  }

  private patchUserRolesToForm(user: UserDto): void {
    this.userRoleFormArray.clear();
    user.userRoles?.forEach((userRole) => {
      const userRoleFormGroup = this.buildUserRoleFormGroup();
      userRoleFormGroup.patchValue({ ...userRole });
      this.userRoleFormArray.push(userRoleFormGroup);
    });
  }

  private uniqueEmailValidator(): AsyncValidatorFn {
    return (control: AbstractControl): Observable<ValidationErrors | null> => {
      if (!control.value || !control.dirty) {
        return of(null);
      }
      return of(control.value).pipe(
        delay(1000),
        switchMap((value) =>
          this.apiUsersClient
            .isEmailAvailable(value, this.id.value)
            .pipe(
              map((response: IsEmailAvailableQueryResult) =>
                !response.isEmailAvailable ? { emailTaken: true } : null
              )
            )
        )
      );
    };
  }

  private uniqueRolesValidator(): ValidatorFn {
    return (formArray: AbstractControl): ValidationErrors | null => {
      let roleIds: string[] = [];
      if (formArray instanceof FormArray) {
        for (let userRoleFormGroup of formArray.controls as UserRoleFormGroup[]) {
          let roleId = userRoleFormGroup.controls.roleId.value;
          if (!roleId) {
            continue;
          }

          if (roleIds.includes(roleId)) {
            userRoleFormGroup.controls.roleId.setErrors({ isDuplicateRole: true });
          } else if (userRoleFormGroup.controls.roleId.hasError('isDuplicateRole')) {
            delete userRoleFormGroup.controls.roleId.errors?.['isDuplicateRole'];
            userRoleFormGroup.controls.roleId.updateValueAndValidity();
          }

          roleIds.push(roleId);
        }
      }
      return null;
    };
  }

  public override getErrorMessage(errorKey: string, errorValue: any): string | undefined {
    switch (errorKey) {
      case 'emailTaken':
        return 'This e-mail address is already taken.';
      case 'isDuplicateRole':
        return 'This role is already assigned.';
      default:
        return super.getErrorMessage(errorKey, errorValue);
    }
  }

  public override reset(): void {
    super.reset();
  }
}
