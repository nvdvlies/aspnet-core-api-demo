import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '@shared/shared.module';
import { InvoicesRoutingModule } from '@invoices/invoices-routing.module';
import { InvoiceListComponent } from '@invoices/pages/invoice-list/invoice-list.component';
import { InvoiceDetailsComponent } from '@invoices/pages/invoice-details/invoice-details.component';
import { InvoiceTableDataService } from '@invoices/pages/invoice-list/invoice-table-data.service';
import { InvoiceListPageSettingsService } from './pages/invoice-list/invoice-list-page-settings.service';

@NgModule({
  declarations: [InvoiceListComponent, InvoiceDetailsComponent],
  imports: [CommonModule, SharedModule, InvoicesRoutingModule],
  providers: [InvoiceTableDataService, InvoiceListPageSettingsService]
})
export class InvoicesModule {}
