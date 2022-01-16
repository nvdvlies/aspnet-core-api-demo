import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedModule } from '@shared/shared.module';
import { CustomersComponent } from '@customers/customers.component';
import { CustomersRoutingModule } from '@customers/customers-routing.module';


@NgModule({
  declarations: [
    CustomersComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    CustomersRoutingModule
  ]
})
export class CustomersModule { }
