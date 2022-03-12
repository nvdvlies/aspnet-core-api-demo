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
  protected entityUpdatedEvent$ = this.customerEventsService.customerUpdated$.pipe(
    map((x) => x.id)
  );

  constructor(
    private readonly apiCustomersClient: ApiCustomersClient,
    private readonly customerEventsService: CustomerEventsService
  ) {
    super();
  }

  protected getByIdFunction = (id: string) => {
    return this.apiCustomersClient
      .lookup(CustomerLookupOrderByEnum.Name, false, 0, 1, undefined, [id])
      .pipe(map((response) => response.customers?.[0]));
  };
}
