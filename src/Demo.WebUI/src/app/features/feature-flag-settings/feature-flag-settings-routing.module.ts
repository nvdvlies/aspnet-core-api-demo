import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthGuard } from '@auth0/auth0-angular';
import { FeatureFlagSettingDetailsComponent } from '@feature-flag-settings/pages/feature-flag-setting-details/feature-flag-setting-details.component';
import { FeatureFlagSettingListComponent } from '@feature-flag-settings/pages/feature-flag-setting-list/feature-flag-setting-list.component';
import { UnsavedChangesGuard } from '@shared/guards/unsaved-changes.guard';
import { AppRoutes, RouteData } from 'src/app/app-routing.module';
import { FeatureFlagSettingAuditlogComponent } from '@feature-flag-settings/pages/feature-flag-setting-auditlog/feature-flag-setting-auditlog.component';
import { PermissionGuard } from '@shared/guards/permission.guard';

const routes: AppRoutes = [
  {
    path: '',
    canActivateChild: [AuthGuard],
    children: [
      {
        path: '',
        component: FeatureFlagSettingListComponent,
        canActivate: [PermissionGuard],
        data: {
          permission: 'FeatureFlagSettingsRead'
        } as RouteData
      },
      {
        path: ':name/auditlog',
        component: FeatureFlagSettingAuditlogComponent,
        canActivate: [PermissionGuard],
        data: {
          permission: 'FeatureFlagSettingsRead'
        } as RouteData
      },
      {
        path: ':name',
        component: FeatureFlagSettingDetailsComponent,
        canActivate: [PermissionGuard],
        data: {
          permission: 'FeatureFlagSettingsRead'
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
export class FeatureFlagsSettingsRoutingModule {}
