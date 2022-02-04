import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '@shared/shared.module';
import { CustomersRoutingModule } from '@customers/customers-routing.module';
import { CustomerListComponent } from './customer-list/customer-list.component';
import { CustomerDetailsComponent } from './customer-details/customer-details.component';
import { CustomerTableDataService } from '@customers/customer-list/customer-table-data.service';

@NgModule({
  declarations: [
    CustomerListComponent,
    CustomerDetailsComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    CustomersRoutingModule
  ],
  providers: [
    CustomerTableDataService
  ]
})
export class CustomersModule { }
