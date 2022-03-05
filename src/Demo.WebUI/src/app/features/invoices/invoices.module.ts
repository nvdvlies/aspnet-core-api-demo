import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '@shared/shared.module';
import { InvoicesRoutingModule } from '@invoices/invoices-routing.module';
import { InvoiceListComponent } from './invoice-list/invoice-list.component';
import { InvoiceDetailsComponent } from './invoice-details/invoice-details.component';
import { InvoiceTableDataService } from '@invoices/invoice-list/invoice-table-data.service';

@NgModule({
  declarations: [InvoiceListComponent, InvoiceDetailsComponent],
  imports: [CommonModule, SharedModule, InvoicesRoutingModule],
  providers: [InvoiceTableDataService]
})
export class InvoicesModule {}
