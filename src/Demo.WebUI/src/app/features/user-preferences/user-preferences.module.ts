import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '@shared/shared.module';
import { UserPreferencesRoutingModule } from '@user-preferences/user-preferences-routing.module';
import { UserPreferencesDetailsComponent } from '@user-preferences/pages/user-preferences-details/user-preferences-details.component';
import { UserPreferencesAuditlogComponent } from './pages/user-preferences-auditlog/user-preferences-auditlog.component';

@NgModule({
  declarations: [UserPreferencesDetailsComponent, UserPreferencesAuditlogComponent],
  imports: [CommonModule, SharedModule, UserPreferencesRoutingModule]
})
export class UserPreferencesModule {}
