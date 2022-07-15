import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '@shared/shared.module';
import { RolesRoutingModule } from '@roles/roles-routing.module';
import { RoleListComponent } from '@roles/pages/role-list/role-list.component';
import { RoleDetailsComponent } from '@roles/pages/role-details/role-details.component';
import { RoleTableDataService } from '@roles/pages/role-list/role-table-data.service';
import { RoleListPageSettingsService } from './pages/role-list/role-list-page-settings.service';
import { RoleAuditlogComponent } from './pages/role-auditlog/role-auditlog.component';

@NgModule({
  declarations: [RoleListComponent, RoleDetailsComponent, RoleAuditlogComponent],
  imports: [CommonModule, SharedModule, RolesRoutingModule],
  providers: [RoleTableDataService, RoleListPageSettingsService]
})
export class RolesModule {}
