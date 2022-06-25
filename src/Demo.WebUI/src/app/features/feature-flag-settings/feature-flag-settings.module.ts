import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '@shared/shared.module';
import { FeatureFlagsSettingsRoutingModule } from '@feature-flag-settings/feature-flag-settings-routing.module';
import { FeatureFlagSettingListComponent } from '@feature-flag-settings/pages/feature-flag-setting-list/feature-flag-setting-list.component';
import { FeatureFlagSettingDetailsComponent } from '@feature-flag-settings/pages/feature-flag-setting-details/feature-flag-setting-details.component';
import { FeatureFlagSettingTableDataService } from '@feature-flag-settings/pages/feature-flag-setting-list/feature-flag-setting-table-data.service';
import { FeatureFlagSettingListPageSettingsService } from './pages/feature-flag-setting-list/feature-flag-setting-list-page-settings.service';
import { FeatureFlagSettingAuditlogComponent } from './pages/feature-flag-setting-auditlog/feature-flag-setting-auditlog.component';

@NgModule({
  declarations: [FeatureFlagSettingListComponent, FeatureFlagSettingDetailsComponent, FeatureFlagSettingAuditlogComponent],
  imports: [CommonModule, SharedModule, FeatureFlagsSettingsRoutingModule],
  providers: [FeatureFlagSettingTableDataService, FeatureFlagSettingListPageSettingsService]
})
export class FeatureFlagsSettingsModule {}
