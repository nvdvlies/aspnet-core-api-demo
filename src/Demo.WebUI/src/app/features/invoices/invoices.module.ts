import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '@shared/shared.module';
import { InvoicesRoutingModule } from '@invoices/invoices-routing.module';
import { InvoiceListComponent } from '@invoices/pages/invoice-list/invoice-list.component';
import { InvoiceDetailsComponent } from '@invoices/pages/invoice-details/invoice-details.component';
import { InvoiceTableDataService } from '@invoices/pages/invoice-list/invoice-table-data.service';
import { InvoiceListPageSettingsService } from './pages/invoice-list-page-settings/invoice-list-page-settings.service';
import { InvoiceAuditlogComponent } from './pages/invoice-auditlog/invoice-auditlog.component';
import { InvoicelistPageSettingsComponent } from './pages/invoice-list-page-settings/invoice-list-page-settings.component';

@NgModule({
  declarations: [
    InvoiceListComponent,
    InvoiceDetailsComponent,
    InvoiceAuditlogComponent,
    InvoicelistPageSettingsComponent
  ],
  imports: [CommonModule, SharedModule, InvoicesRoutingModule],
  providers: [InvoiceTableDataService, InvoiceListPageSettingsService]
})
export class InvoicesModule {}
