import { ChangeDetectionStrategy, Component, HostListener, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { combineLatest, debounceTime, EMPTY, map, Observable, switchMap, tap } from 'rxjs';
import { DomainEntityService } from '@domain/shared/domain-entity-base';
import {
  RoleDomainEntityService,
  RoleFormGroup,
  IRoleDomainEntityContext,
  RolePermissionFormArray,
  RolePermissionFormGroup
} from '@domain/role/role-domain-entity.service';
import { IHasForm } from '@shared/guards/unsaved-changes.guard';
import { RoleListRouteState } from '@roles/pages/role-list/role-list.component';
import { ModalService } from '@shared/services/modal.service';
import { ConfirmDeleteModalComponent } from '@shared/modals/confirm-delete-modal/confirm-delete-modal.component';
import { PermissionDto, PermissionGroupDto } from '@api/api.generated.clients';
import { PermissionGroupService } from '@shared/services/permission-group.service';

interface ViewModel extends IRoleDomainEntityContext {}

@Component({
  templateUrl: './role-details.component.html',
  styleUrls: ['./role-details.component.scss'],
  providers: [
    RoleDomainEntityService,
    {
      provide: DomainEntityService,
      useExisting: RoleDomainEntityService
    }
  ],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class RoleDetailsComponent implements OnInit, IHasForm {
  public initFromRoute$ = this.roleDomainEntityService.initFromRoute();

  private vm: Readonly<ViewModel> | undefined;

  public vm$: Observable<ViewModel> = combineLatest([this.roleDomainEntityService.observe$]).pipe(
    debounceTime(0),
    map(([domainEntityContext]) => {
      const context: ViewModel = {
        ...domainEntityContext
      };
      return context;
    }),
    tap((vm) => (this.vm = vm))
  );

  public form: RoleFormGroup = this.roleDomainEntityService.form;

  public get rolePermissions(): RolePermissionFormArray {
    return this.form.controls.rolePermissions as RolePermissionFormArray;
  }

  public permissionGroups: PermissionGroupDto[] = [];

  constructor(
    private readonly router: Router,
    private readonly roleDomainEntityService: RoleDomainEntityService,
    private readonly modalService: ModalService,
    private readonly permissionGroupService: PermissionGroupService
  ) {}

  ngOnInit(): void {
    this.permissionGroups = this.permissionGroupService.permissionGroups;
  }

  public save(): void {
    if (!this.form.valid) {
      return;
    }

    this.roleDomainEntityService.upsert().subscribe((role) => {
      this.router.navigateByUrl('/roles', {
        state: { spotlightIdentifier: role.id } as RoleListRouteState
      });
    });
  }

  public delete(): void {
    this.modalService
      .confirmWithModal(ConfirmDeleteModalComponent)
      .pipe(switchMap((confirmed) => (confirmed ? this.roleDomainEntityService.delete() : EMPTY)))
      .subscribe(() => {
        this.router.navigateByUrl('/roles');
      });
  }

  public allChildPermissionsChecked(permissionGroup: PermissionGroupDto): boolean {
    return (
      this.roleDomainEntityService.rolePermissions.find(
        (x) =>
          x.controls.permissionGroupId.value === permissionGroup.id && !x.controls.checked.value
      ) == null
    );
  }

  public togglePermissionGroup(permissionGroup: PermissionGroupDto, checked: boolean): void {
    this.roleDomainEntityService.rolePermissions.forEach((rolePermissionFormGroup) => {
      if (rolePermissionFormGroup.controls.permissionGroupId.value === permissionGroup.id) {
        rolePermissionFormGroup.controls.checked.setValue(checked);
      }
    });
  }

  public resolveMergeConflictWithTakeTheirs(): void {
    this.roleDomainEntityService.resolveMergeConflictWithTakeTheirs();
  }

  @HostListener('document:keydown.shift.alt.s', ['$event'])
  public saveShortcut(event: KeyboardEvent) {
    this.form.markAllAsTouched();
    this.save();
    event.preventDefault();
  }

  @HostListener('document:keydown.shift.alt.d', ['$event'])
  public deleteShortcut(event: KeyboardEvent) {
    if (!this.vm?.id) {
      return;
    }
    this.delete();
    event.preventDefault();
  }

  @HostListener('document:keydown.shift.alt.c', ['$event'])
  public closeShortcut(event: KeyboardEvent) {
    this.router.navigateByUrl('/roles');
    event.preventDefault();
  }
}
