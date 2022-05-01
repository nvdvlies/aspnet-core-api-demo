import { Injectable, OnDestroy } from '@angular/core';
import {
  AbstractControl,
  AsyncValidatorFn,
  FormArray,
  FormControl,
  FormGroup,
  ValidationErrors,
  Validators
} from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { combineLatest, Observable, of } from 'rxjs';
import { debounceTime, map } from 'rxjs/operators';
import { UserDto, IUserDto, IUserRoleDto } from '@api/api.generated.clients';
import { UserStoreService } from '@domain/user/user-store.service';
import {
  DomainEntityBase,
  DomainEntityContext,
  IDomainEntityContext,
  InitFromRouteOptions
} from '@domain/shared/domain-entity-base';

export interface IUserDomainEntityContext extends IDomainEntityContext<UserDto> {}

export class UserDomainEntityContext
  extends DomainEntityContext<UserDto>
  implements IUserDomainEntityContext
{
  constructor() {
    super();
  }
}

type UserControls = { [key in keyof IUserDto]: AbstractControl };
export type UserFormGroup = FormGroup & { controls: UserControls };

type UserRoleControls = { [key in keyof IUserRoleDto]: AbstractControl };
export type UserRoleFormGroup = FormGroup & {
  controls: UserRoleControls;
};

export type UserRoleFormArray = FormArray & {
  controls: UserRoleFormGroup[];
};

@Injectable()
export class UserDomainEntityService extends DomainEntityBase<UserDto> implements OnDestroy {
  protected createFunction = (user: UserDto) => this.userStoreService.create(user);
  protected readFunction = (id?: string) => this.userStoreService.getById(id!);
  protected updateFunction = (user: UserDto) => this.userStoreService.update(user);
  protected deleteFunction = (id?: string) => this.userStoreService.delete(id!);
  protected entityUpdatedEvent$ = this.userStoreService.userUpdatedInStore$;

  public observe$ = combineLatest([this.observeInternal$]).pipe(
    debounceTime(0),
    map(([context]) => {
      return {
        ...context
      } as UserDomainEntityContext;
    })
  ) as Observable<UserDomainEntityContext>;

  constructor(route: ActivatedRoute, private readonly userStoreService: UserStoreService) {
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
    return new FormGroup(
      {
        id: new FormControl(super.readonlyFormState),
        externalId: new FormControl(super.readonlyFormState),
        fullname: new FormControl(super.readonlyFormState),
        givenName: new FormControl(null),
        familyName: new FormControl(null, [Validators.required], []),
        middleName: new FormControl(null),
        email: new FormControl(null, [Validators.required], [this.emailValidator()]),
        gender: new FormControl(null),
        birthDate: new FormControl(null),
        zoneInfo: new FormControl(null),
        locale: new FormControl(null),
        userRoles: new FormArray(
          [] as UserRoleFormGroup[],
          [Validators.required],
          []
        ) as UserRoleFormArray,
        deleted: new FormControl(super.readonlyFormState),
        deletedBy: new FormControl(super.readonlyFormState),
        deletedOn: new FormControl(super.readonlyFormState),
        createdBy: new FormControl(super.readonlyFormState),
        createdOn: new FormControl(super.readonlyFormState),
        lastModifiedBy: new FormControl(super.readonlyFormState),
        lastModifiedOn: new FormControl(super.readonlyFormState),
        timestamp: new FormControl(super.readonlyFormState)
      } as UserControls,
      { updateOn: 'blur' }
    ) as UserFormGroup;
  }

  private buildUserRoleFormGroup(): UserRoleFormGroup {
    return new FormGroup({
      roleId: new FormControl(null, [Validators.required], [])
    } as UserRoleControls) as UserRoleFormGroup;
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
    user.userRoles = [];
    return of(user);
  }

  public get userRoles(): UserRoleFormGroup[] {
    return this.userRoleFormArray.controls as UserRoleFormGroup[];
  }

  private get userRoleFormArray(): FormArray {
    return this.form.controls.userRoles as FormArray;
  }

  public override new(): Observable<null> {
    return super.new();
  }

  public override read(id: string): Observable<null> {
    return super.read(id);
  }

  public override initFromRoute(options?: InitFromRouteOptions | undefined): Observable<null> {
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

  private emailValidator(): AsyncValidatorFn {
    return (control: AbstractControl): Observable<ValidationErrors | null> => {
      return of(false).pipe(
        // TODO
        map((isTaken: boolean) => {
          return isTaken ? { emailTaken: true } : null;
        })
      );
    };
  }

  public override getErrorMessage(errorKey: string, errorValue: any): string | undefined {
    switch (errorKey) {
      case 'emailTaken':
        return 'This e-mail address is already taken.';
      default:
        return super.getErrorMessage(errorKey, errorValue);
    }
  }

  public override reset(): void {
    super.reset();
  }
}
