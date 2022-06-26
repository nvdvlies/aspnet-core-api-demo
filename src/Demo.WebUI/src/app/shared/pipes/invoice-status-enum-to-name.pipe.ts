import { Pipe, PipeTransform } from '@angular/core';
import { InvoiceStatusEnum } from '@api/api.generated.clients';

@Pipe({
  name: 'invoiceStatusEnumToName'
})
export class InvoiceStatusEnumToNamePipe implements PipeTransform {
  transform(value: InvoiceStatusEnum): string {
    return InvoiceStatusEnum[value];
  }
}
