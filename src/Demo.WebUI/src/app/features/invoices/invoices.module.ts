import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '@shared/shared.module';
import { InvoicesComponent } from '@invoices/invoices.component';
import { InvoicesRoutingModule } from '@invoices/invoices-routing.module';

@NgModule({
  declarations: [
    InvoicesComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    InvoicesRoutingModule
  ]
})
export class InvoicesModule { }
