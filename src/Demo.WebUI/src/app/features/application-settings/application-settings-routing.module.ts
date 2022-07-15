import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthGuard } from '@auth0/auth0-angular';
import { ApplicationSettingsDetailsComponent } from '@application-settings/pages/application-settings-details/application-settings-details.component';
import { UnsavedChangesGuard } from '@shared/guards/unsaved-changes.guard';
import { AppRoutes, RouteData } from 'src/app/app-routing.module';
import { ApplicationSettingsAuditlogComponent } from '@application-settings//pages/application-settings-auditlog/application-settings-auditlog.component';
import { PermissionGuard } from '@shared/guards/permission.guard';

const routes: AppRoutes = [
  {
    path: '',
    canActivateChild: [AuthGuard],
    children: [
      {
        path: '',
        component: ApplicationSettingsDetailsComponent,
        canActivate: [PermissionGuard],
        data: {
          permission: 'ApplicationSettingsRead'
        } as RouteData,
        canDeactivate: [UnsavedChangesGuard]
      },
      {
        path: 'auditlog',
        component: ApplicationSettingsAuditlogComponent,
        canActivate: [PermissionGuard],
        data: {
          permission: 'ApplicationSettingsRead'
        } as RouteData
      }
    ] as AppRoutes
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ApplicationSettingsRoutingModule {}
