import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthGuard } from '@auth0/auth0-angular';
import { RoleDetailsComponent } from '@roles/pages/role-details/role-details.component';
import { RoleListComponent } from '@roles/pages/role-list/role-list.component';
import { UnsavedChangesGuard } from '@shared/guards/unsaved-changes.guard';
import { AppRoutes, RouteData } from 'src/app/app-routing.module';
import { RoleAuditlogComponent } from '@roles/pages/role-auditlog/role-auditlog.component';
import { PermissionGuard } from '@shared/guards/permission.guard';

const routes: AppRoutes = [
  {
    path: '',
    canActivateChild: [AuthGuard],
    children: [
      {
        path: '',
        component: RoleListComponent,
        canActivate: [PermissionGuard],
        data: {
          permission: 'RolesRead'
        } as RouteData
      },
      {
        path: ':id/auditlog',
        component: RoleAuditlogComponent,
        canActivate: [PermissionGuard],
        data: {
          permission: 'RolesRead'
        } as RouteData
      },
      {
        path: ':id',
        component: RoleDetailsComponent,
        canActivate: [PermissionGuard],
        data: {
          permission: 'RolesRead'
        } as RouteData,
        canDeactivate: [UnsavedChangesGuard]
      }
    ] as AppRoutes
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RolesRoutingModule {}
