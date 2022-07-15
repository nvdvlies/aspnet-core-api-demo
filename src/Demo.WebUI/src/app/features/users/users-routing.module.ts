import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthGuard } from '@auth0/auth0-angular';
import { UserDetailsComponent } from '@users/pages/user-details/user-details.component';
import { UserListComponent } from '@users/pages/user-list/user-list.component';
import { UnsavedChangesGuard } from '@shared/guards/unsaved-changes.guard';
import { AppRoutes, RouteData } from 'src/app/app-routing.module';
import { UserAuditlogComponent } from '@users/pages/user-auditlog/user-auditlog.component';
import { PermissionGuard } from '@shared/guards/permission.guard';

const routes: AppRoutes = [
  {
    path: '',
    canActivateChild: [AuthGuard],
    children: [
      {
        path: '',
        component: UserListComponent,
        canActivate: [PermissionGuard],
        data: {
          permission: 'UsersRead'
        } as RouteData
      },
      {
        path: ':id/auditlog',
        component: UserAuditlogComponent,
        canActivate: [PermissionGuard],
        data: {
          permission: 'UsersRead'
        } as RouteData
      },
      {
        path: ':id',
        component: UserDetailsComponent,
        canActivate: [PermissionGuard],
        data: {
          permission: 'UsersRead'
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
export class UsersRoutingModule {}
