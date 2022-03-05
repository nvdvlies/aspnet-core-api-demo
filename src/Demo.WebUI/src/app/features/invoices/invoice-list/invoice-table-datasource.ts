import { CollectionViewer, DataSource } from '@angular/cdk/collections';
import { map, Observable } from 'rxjs';
import { SearchInvoiceDto } from '@api/api.generated.clients';
import { InvoiceTableDataService } from '@invoices/invoice-list/invoice-table-data.service';

export class InvoiceTableDataSource implements DataSource<SearchInvoiceDto> {
  constructor(private invoiceTableDataService: InvoiceTableDataService) {}

  connect(collectionViewer: CollectionViewer): Observable<SearchInvoiceDto[]> {
    return this.invoiceTableDataService.observe$.pipe(map((x) => x.items ?? []));
  }

  disconnect(collectionViewer: CollectionViewer): void {}
}
