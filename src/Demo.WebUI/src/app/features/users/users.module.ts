import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '@shared/shared.module';
import { UsersRoutingModule } from '@users/users-routing.module';
import { UserListComponent } from '@users/pages/user-list/user-list.component';
import { UserDetailsComponent } from '@users/pages/user-details/user-details.component';
import { UserTableDataService } from '@users/pages/user-list/user-table-data.service';
import { UserListPageSettingsService } from './pages/user-list/user-list-page-settings.service';
import { UserAuditlogComponent } from './pages/user-auditlog/user-auditlog.component';

@NgModule({
  declarations: [UserListComponent, UserDetailsComponent, UserAuditlogComponent],
  imports: [CommonModule, SharedModule, UsersRoutingModule],
  providers: [UserTableDataService, UserListPageSettingsService]
})
export class UsersModule {}
