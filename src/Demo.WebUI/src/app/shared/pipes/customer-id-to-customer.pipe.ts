import { Pipe, PipeTransform } from '@angular/core';
import { CustomerDto } from '@api/api.generated.clients';
import { CustomerStoreService } from '@domain/customer/customer-store.service';
import { combineLatest, filter, map, Observable, startWith } from 'rxjs';

export interface CustomerIdToCustomerPipe {}

@Pipe({
  name: 'customer'
})
export class CustomerIdToCustomerPipe implements PipeTransform {
  constructor(private readonly customerStoreService: CustomerStoreService) {}

  transform(
    id: string | undefined,
    options?: CustomerIdToCustomerPipe
  ): Observable<CustomerDto | undefined> {
    return combineLatest([
      this.customerStoreService.getById(id!),
      this.customerStoreService.customerUpdatedInStore$.pipe(
        filter(([customerUpdateEvent, _]) => customerUpdateEvent.id === id),
        startWith([null, null])
      )
    ]).pipe(map(([customer, [_, updatedCustomer]]) => updatedCustomer ?? customer));
  }
}
