import { Pipe, PipeTransform } from '@angular/core';
import { CustomerLookupService } from '@shared/services/customer-lookup.service';
import { map, Observable, of } from 'rxjs';

export interface CustomerIdToNamePipeOptions {
  includeCode: boolean;
}

@Pipe({
  name: 'customerIdToName'
})
export class CustomerIdToNamePipe implements PipeTransform {
  constructor(private readonly customerLookupService: CustomerLookupService) {}

  transform(
    id: string | undefined,
    options?: CustomerIdToNamePipeOptions
  ): Observable<string | undefined> {
    return id
      ? this.customerLookupService
          .getById(id)
          .pipe(map((x) => (options?.includeCode ? `${x?.code} - ${x?.name}` : x?.name)))
      : of(undefined);
  }
}
