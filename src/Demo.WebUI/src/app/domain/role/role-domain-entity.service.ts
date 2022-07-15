import { Injectable, OnDestroy } from '@angular/core';
import { AbstractControl, FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
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

export interface IRoleDomainEntityContext extends IDomainEntityContext<RoleDto> {}

export class RoleDomainEntityContext
  extends DomainEntityContext<RoleDto>
  implements IRoleDomainEntityContext
{
  constructor() {
    super();
  }
}

type RoleControls = { [key in keyof IRoleDto]-?: AbstractControl };
export type RoleFormGroup = FormGroup & { controls: RoleControls };

interface IRolePermissionControls extends IRolePermissionDto {
  permissionGroupId: string;
  name: string;
  checked: boolean;
}

type RolePermissionControls = { [key in keyof IRolePermissionControls]-?: AbstractControl };
export type RolePermissionFormGroup = FormGroup & {
  controls: RolePermissionControls;
};

export type RolePermissionFormArray = FormArray & {
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
      id: new FormControl(super.readonlyFormState),
      name: new FormControl(null, [Validators.required], []),
      externalId: new FormControl(null, [Validators.required], []),
      rolePermissions: new FormArray(
        [] as RolePermissionFormGroup[],
        [],
        []
      ) as RolePermissionFormArray,
      deleted: new FormControl(super.readonlyFormState),
      deletedBy: new FormControl(super.readonlyFormState),
      deletedOn: new FormControl(super.readonlyFormState),
      createdBy: new FormControl(super.readonlyFormState),
      createdOn: new FormControl(super.readonlyFormState),
      lastModifiedBy: new FormControl(super.readonlyFormState),
      lastModifiedOn: new FormControl(super.readonlyFormState),
      xmin: new FormControl(super.readonlyFormState)
    };
    return new FormGroup(controls) as RoleFormGroup;
  }

  private buildRolePermissionFormGroup(): RolePermissionFormGroup {
    const controls: RolePermissionControls = {
      permissionId: new FormControl(null, [Validators.required], []),
      permissionGroupId: new FormControl(super.readonlyFormState),
      name: new FormControl(super.readonlyFormState),
      checked: new FormControl(null)
    };
    return new FormGroup(controls) as RolePermissionFormGroup;
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

  private get rolePermissionFormArray(): FormArray {
    return this.form.controls.rolePermissions as FormArray;
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
