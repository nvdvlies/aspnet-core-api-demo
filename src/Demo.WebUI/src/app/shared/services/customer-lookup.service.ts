import { Injectable } from '@angular/core';
import {
  ApiCustomersClient,
  CustomerLookupDto,
  CustomerLookupOrderByEnum
} from '@api/api.generated.clients';
import { BehaviorSubject, map, Observable, of, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CustomerLookupService {
  private readonly cache = new BehaviorSubject<CustomerLookupDto[]>([]);

  private get items(): CustomerLookupDto[] {
    return this.cache.getValue();
  }

  private set items(value: CustomerLookupDto[]) {
    this.cache.next(value);
  }

  private addToCache(item: CustomerLookupDto): void {
    this.items = [...this.items, item];
  }

  constructor(private readonly apiCustomersClient: ApiCustomersClient) {}

  public getById(id: string): Observable<CustomerLookupDto | undefined> {
    const cachedItem = this.cache.value.find((x) => x.id?.toLowerCase() === id.toLowerCase());
    if (cachedItem) {
      return of(cachedItem);
    } else {
      return this.apiCustomersClient
        .lookup(CustomerLookupOrderByEnum.Name, false, 0, 1, undefined, [id])
        .pipe(
          map((response) => response.customers?.[0]),
          tap((customer) => {
            if (customer) {
              this.addToCache(customer);
            }
          })
        );
    }
  }
}
