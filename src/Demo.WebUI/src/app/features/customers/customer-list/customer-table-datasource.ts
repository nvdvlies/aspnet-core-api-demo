import { CollectionViewer, DataSource } from '@angular/cdk/collections';
import { map, Observable } from 'rxjs';
import { SearchCustomerDto } from '@api/api.generated.clients';
import { CustomerTableDataService } from '@customers/customer-list/customer-table-data.service';

export class CustomerTableDataSource implements DataSource<SearchCustomerDto> {
  constructor(private customerTableDataService: CustomerTableDataService) {}

  connect(collectionViewer: CollectionViewer): Observable<SearchCustomerDto[]> {
    return this.customerTableDataService.observe$.pipe(map((x) => x.items ?? []));
  }

  disconnect(collectionViewer: CollectionViewer): void {}
}
