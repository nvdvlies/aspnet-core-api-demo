import { Injectable } from '@angular/core';
import {
  ApiInvoicesClient,
  InvoiceLookupDto,
  InvoiceLookupOrderByEnum
} from '@api/api.generated.clients';
import { InvoiceEventsService } from '@api/signalr.generated.services';
import { LookupBase } from '@shared/base/lookup-base';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class InvoiceLookupService extends LookupBase<InvoiceLookupDto> {
  protected itemUpdatedEvent$ = this.invoiceEventsService.invoiceUpdated$.pipe(map((x) => x.id));

  constructor(
    private readonly apiInvoicesClient: ApiInvoicesClient,
    private readonly invoiceEventsService: InvoiceEventsService
  ) {
    super();
  }

  protected lookupFunction = (ids: string[]) => {
    return this.apiInvoicesClient
      .lookup(InvoiceLookupOrderByEnum.InvoiceNumber, true, 0, 999, undefined, ids)
      .pipe(map((response) => response.invoices ?? []));
  };
}
