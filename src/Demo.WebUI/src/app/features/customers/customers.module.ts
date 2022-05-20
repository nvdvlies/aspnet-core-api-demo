import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '@shared/shared.module';
import { CustomersRoutingModule } from '@customers/customers-routing.module';
import { CustomerListComponent } from '@customers/pages/customer-list/customer-list.component';
import { CustomerDetailsComponent } from '@customers/pages/customer-details/customer-details.component';
import { CustomerTableDataService } from '@customers/pages/customer-list/customer-table-data.service';
import { CustomerListPageSettingsService } from './pages/customer-list/customer-list-page-settings.service';

@NgModule({
  declarations: [CustomerListComponent, CustomerDetailsComponent],
  imports: [CommonModule, SharedModule, CustomersRoutingModule],
  providers: [CustomerTableDataService, CustomerListPageSettingsService]
})
export class CustomersModule {}
