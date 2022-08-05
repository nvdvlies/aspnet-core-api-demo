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
import { RoleDto, IRoleDto, IRolePermissionDto } from '@api/api.generated.clients';
import { RoleStoreService } from '@domain/role/role-store.service';
import {
  DomainEntityBase,
  DomainEntityContext,
  IDomainEntityContext,
  InitFromRouteOptions
} from '@domain/shared/domain-entity-base';
import { Permission } from '@shared/enums/permission.enum';
import { PermissionService } from '@shared/services/permission.service';
import { UserPermissionService } from '@shared/services/user-permission.service';
import { ExtendedAbstractControl } from '@domain/shared/form-controls-base';

export interface IRoleDomainEntityContext extends IDomainEntityContext<RoleDto> {}

export class RoleDomainEntityContext
  extends DomainEntityContext<RoleDto>
  implements IRoleDomainEntityContext
{
  constructor() {
    super();
  }
}

type RoleControls = { [key in keyof IRoleDto]-?: ExtendedAbstractControl };
export type RoleFormGroup = UntypedFormGroup & { controls: RoleControls };

interface IRolePermissionControls extends IRolePermissionDto {
  permissionGroupId: string;
  name: string;
  checked: boolean;
}

type RolePermissionControls = { [key in keyof IRolePermissionControls]-?: ExtendedAbstractControl };
export type RolePermissionFormGroup = UntypedFormGroup & {
  controls: RolePermissionControls;
};

export type RolePermissionFormArray = UntypedFormArray & {
  controls: RolePermissionFormGroup[];
};

@Injectable()
export class RoleDomainEntityService extends DomainEntityBase<RoleDto> implements OnDestroy {
  protected createFunction = (role: RoleDto) =>
    this.roleStoreService.create(this.removeUncheckedRolePermissions(role));
  protected readFunction = (id?: string) => this.roleStoreService.getById(id!);
  protected updateFunction = (role: RoleDto) =>
    this.roleStoreService.update(this.removeUncheckedRolePermissions(role));
  protected deleteFunction = (id?: string) => this.roleStoreService.delete(id!);
  protected entityUpdatedEvent$ = this.roleStoreService.roleUpdatedInStore$;
  protected readPermission?: keyof typeof Permission = 'RolesRead';
  protected writePermission?: keyof typeof Permission = 'RolesWrite';

  public observe$: Observable<RoleDomainEntityContext> = combineLatest([
    this.observeInternal$
  ]).pipe(
    debounceTime(0),
    map(([baseContext]) => {
      const context: RoleDomainEntityContext = {
        ...baseContext
      };
      return context;
    })
  );

  constructor(
    route: ActivatedRoute,
    userPermissionService: UserPermissionService,
    private readonly roleStoreService: RoleStoreService,
    private readonly permissionService: PermissionService
  ) {
    super(route, userPermissionService);
    super.init();
  }

  public override get form(): RoleFormGroup {
    return super.form as RoleFormGroup;
  }
  public override set form(value: RoleFormGroup) {
    super.form = value;
  }

  protected instantiateForm(): void {
    this.form = this.buildRoleFormGroup();
  }

  private buildRoleFormGroup(): RoleFormGroup {
    const controls: RoleControls = {
      id: new UntypedFormControl(super.readonlyFormState),
      name: new UntypedFormControl(null, [Validators.required], []),
      externalId: new UntypedFormControl(null, [Validators.required], []),
      rolePermissions: new UntypedFormArray(
        [] as RolePermissionFormGroup[],
        [],
        []
      ) as RolePermissionFormArray,
      deleted: new UntypedFormControl(super.readonlyFormState),
      deletedBy: new UntypedFormControl(super.readonlyFormState),
      deletedOn: new UntypedFormControl(super.readonlyFormState),
      createdBy: new UntypedFormControl(super.readonlyFormState),
      createdOn: new UntypedFormControl(super.readonlyFormState),
      lastModifiedBy: new UntypedFormControl(super.readonlyFormState),
      lastModifiedOn: new UntypedFormControl(super.readonlyFormState),
      xmin: new UntypedFormControl(super.readonlyFormState)
    };
    return new UntypedFormGroup(controls) as RoleFormGroup;
  }

  private buildRolePermissionFormGroup(): RolePermissionFormGroup {
    const controls: RolePermissionControls = {
      permissionId: new UntypedFormControl(null, [Validators.required], []),
      permissionGroupId: new UntypedFormControl(super.readonlyFormState),
      name: new UntypedFormControl(super.readonlyFormState),
      checked: new UntypedFormControl(null)
    };
    return new UntypedFormGroup(controls) as RolePermissionFormGroup;
  }

  protected instantiateNewEntity(): Observable<RoleDto> {
    const role = new RoleDto();
    role.rolePermissions = [];
    return of(role);
  }

  public get rolePermissions(): RolePermissionFormGroup[] {
    return this.rolePermissionFormArray.controls as RolePermissionFormGroup[];
  }

  public get checkedRolePermissions(): RolePermissionFormGroup[] {
    return this.rolePermissions.filter((x) => x.controls.checked);
  }

  private get rolePermissionFormArray(): UntypedFormArray {
    return this.form.controls.rolePermissions as UntypedFormArray;
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

  public override create(): Observable<RoleDto> {
    return super.create();
  }

  public override update(): Observable<RoleDto> {
    return super.update();
  }

  public override upsert(): Observable<RoleDto> {
    return super.upsert();
  }

  public override delete(): Observable<void> {
    return super.delete();
  }

  protected override afterPatchEntityToFormHook(role: RoleDto): void {
    // form.patchValue doesnt modify FormArray structure, so we need to do this manually afterwards.
    this.patchRolePermissionsToForm(role);
  }

  private patchRolePermissionsToForm(role: RoleDto): void {
    this.rolePermissionFormArray.clear();

    const allPermissions = this.permissionService.permissions;
    allPermissions.forEach((permission) => {
      const rolePermissionFormGroup = this.buildRolePermissionFormGroup();
      const rolePermission: IRolePermissionControls = {
        permissionId: permission.id,
        permissionGroupId: permission.permissionGroupId!,
        name: permission.name!,
        checked: role.rolePermissions?.find((x) => x.permissionId === permission.id) != null
      };
      rolePermissionFormGroup.patchValue({ ...rolePermission });
      this.rolePermissionFormArray.push(rolePermissionFormGroup);
    });
  }

  private removeUncheckedRolePermissions(role: RoleDto): RoleDto {
    role.rolePermissions = role.rolePermissions?.filter(
      (x) =>
        this.checkedRolePermissions.find(
          (formGroup) =>
            formGroup.controls.checked.value &&
            formGroup.controls.permissionId.value === x.permissionId
        ) != null
    );
    return role;
  }

  public override reset(): void {
    super.reset();
  }
}
