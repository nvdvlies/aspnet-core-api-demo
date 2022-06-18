import { Pipe, PipeTransform } from '@angular/core';
import { InvoiceLookupService } from '@shared/services/invoice-lookup.service';
import { map, Observable, of } from 'rxjs';

@Pipe({
  name: 'invoiceIdToNumber'
})
export class InvoiceIdToNumberPipe implements PipeTransform {
  constructor(private readonly invoiceLookupService: InvoiceLookupService) {}

  transform(id: string | undefined): Observable<string | undefined> {
    return id
      ? this.invoiceLookupService.getById(id).pipe(map((x) => x?.invoiceNumber))
      : of(undefined);
  }
}
