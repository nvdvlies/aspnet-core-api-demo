import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthGuard } from '@auth0/auth0-angular';
import { UserPreferencesDetailsComponent } from '@user-preferences/pages/user-preferences-details/user-preferences-details.component';
import { UnsavedChangesGuard } from '@shared/guards/unsaved-changes.guard';
import { AppRoutes } from 'src/app/app-routing.module';
import { UserPreferencesAuditlogComponent } from '@user-preferences/pages/user-preferences-auditlog/user-preferences-auditlog.component';

const routes: AppRoutes = [
  {
    path: '',
    canActivateChild: [AuthGuard],
    children: [
      {
        path: '',
        component: UserPreferencesDetailsComponent,
        canDeactivate: [UnsavedChangesGuard]
      },
      {
        path: 'auditlog',
        component: UserPreferencesAuditlogComponent
      }
    ] as AppRoutes
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserPreferencesRoutingModule {}
