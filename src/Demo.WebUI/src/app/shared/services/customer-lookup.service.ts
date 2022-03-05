import { Injectable } from '@angular/core';
import {
  ApiCustomersClient,
  CustomerLookupDto,
  CustomerLookupOrderByEnum
} from '@api/api.generated.clients';
import { map, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CustomerLookupService {
  constructor(private readonly apiCustomersClient: ApiCustomersClient) {}

  public getById(id: string): Observable<CustomerLookupDto | undefined> {
    return this.apiCustomersClient
      .lookup(CustomerLookupOrderByEnum.Name, false, 0, 1, undefined, [id])
      .pipe(map((x) => x.customers?.[0]));
  }
}
