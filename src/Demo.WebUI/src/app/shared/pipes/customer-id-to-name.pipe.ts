import { Pipe, PipeTransform } from '@angular/core';
import { CustomerLookupService } from '@shared/services/customer-lookup.service';
import { map, Observable, of } from 'rxjs';

@Pipe({
  name: 'customerIdToName'
})
export class CustomerIdToNamePipe implements PipeTransform {
  constructor(private readonly customerLookupService: CustomerLookupService) {}

  transform(id: string | undefined): Observable<string | undefined> {
    return id ? this.customerLookupService.getById(id).pipe(map((x) => x?.name)) : of(undefined);
  }
}
