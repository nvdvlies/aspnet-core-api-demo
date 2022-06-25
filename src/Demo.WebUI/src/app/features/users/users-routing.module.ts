import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthGuard } from '@auth0/auth0-angular';
import { UserDetailsComponent } from '@users/pages/user-details/user-details.component';
import { UserListComponent } from '@users/pages/user-list/user-list.component';
import { UnsavedChangesGuard } from '@shared/guards/unsaved-changes.guard';
import { AppRoutes } from 'src/app/app-routing.module';
import { UserAuditlogComponent } from '@users/pages/user-auditlog/user-auditlog.component';

const routes: AppRoutes = [
  {
    path: '',
    canActivateChild: [AuthGuard],
    children: [
      {
        path: '',
        component: UserListComponent
      },
      {
        path: ':id/auditlog',
        component: UserAuditlogComponent
      },
      {
        path: ':id',
        component: UserDetailsComponent,
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
