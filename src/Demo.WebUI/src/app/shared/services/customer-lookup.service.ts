import { Injectable } from '@angular/core';
import {
  ApiCustomersClient,
  CustomerLookupDto,
  CustomerLookupOrderByEnum
} from '@api/api.generated.clients';
import { CustomerEventsService } from '@api/signalr.generated.services';
import { LookupBase } from '@shared/base/lookup.base';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CustomerLookupService extends LookupBase<CustomerLookupDto> {
  protected itemUpdatedEvent$ = this.customerEventsService.customerUpdated$.pipe(map((x) => x.id));

  constructor(
    private readonly apiCustomersClient: ApiCustomersClient,
    private readonly customerEventsService: CustomerEventsService
  ) {
    super();
  }

  protected lookupFunction = (ids: string[]) => {
    return this.apiCustomersClient
      .lookup(CustomerLookupOrderByEnum.Name, false, 0, 999, undefined, ids)
      .pipe(map((response) => response.customers ?? []));
  };
}
