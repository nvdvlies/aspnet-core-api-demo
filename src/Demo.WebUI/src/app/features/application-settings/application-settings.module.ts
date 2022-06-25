import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '@shared/shared.module';
import { ApplicationSettingsRoutingModule } from '@application-settings/application-settings-routing.module';
import { ApplicationSettingsDetailsComponent } from '@application-settings/pages/application-settings-details/application-settings-details.component';
import { ApplicationSettingsAuditlogComponent } from './pages/application-settings-auditlog/application-settings-auditlog.component';

@NgModule({
  declarations: [ApplicationSettingsDetailsComponent, ApplicationSettingsAuditlogComponent],
  imports: [CommonModule, SharedModule, ApplicationSettingsRoutingModule]
})
export class ApplicationSettingsModule {}
