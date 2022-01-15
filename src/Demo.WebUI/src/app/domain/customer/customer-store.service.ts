import { Injectable } from '@angular/core';
import { combineLatest, Observable, of, Subject } from 'rxjs';
import { tap, map, switchMap, filter, mergeMap } from 'rxjs/operators';
import { CustomerDto, ApiCustomersClient, CreateCustomerCommand, UpdateCustomerCommand, DeleteCustomerCommand } from '@api/api.generated.clients';
import { CustomerUpdatedEvent, CustomerDeletedEvent, CustomerEventsService } from '@api/signalr.generated.services';
import { StoreBase } from '@domain/shared/store-base';

@Injectable({
  providedIn: 'root'
})
export class CustomerStoreService extends StoreBase<CustomerDto> {
  private readonly customerUpdatedInStore = new Subject<[CustomerUpdatedEvent, CustomerDto]>();
  private readonly customerDeletedFromStore = new Subject<CustomerDeletedEvent>();
  private readonly createdBySelf = new Subject<CustomerDto>();

  public readonly customers$ = this.cache.asObservable();
  public readonly customerUpdatedInStore$ = this.customerUpdatedInStore.asObservable();
  public readonly customerDeletedFromStore$ = this.customerDeletedFromStore.asObservable();
  public readonly createdBySelf$ = this.createdBySelf.asObservable();

  constructor(
    private apiCustomersClient: ApiCustomersClient,
    private customerEventsService: CustomerEventsService
  ) {
    super();
    this.subscribeToEvents();
  }

  public getById(id: string, skipCache: boolean = false): Observable<CustomerDto> {
    let customer = !skipCache
      ? this.cache.value.find(x => x.id === id)
      : null;

    return customer != null
      ? of(customer)
      : this.apiCustomersClient.getCustomerById(id)
        .pipe(
          map((response) => {
            customer = new CustomerDto({ ...response.customer! });
            this.addToOrReplaceCache(customer);
            return customer;
          })
        );
  }

  public create(customer: CustomerDto): Observable<CustomerDto> {
    const command = new CreateCustomerCommand({ ...customer });
    return this.apiCustomersClient.create(command)
      .pipe(
        switchMap((response) => {
          return this.getById(response.id);
        }),
        tap((createdCustomer) => {
          this.createdBySelf.next(createdCustomer);
        })
      );
  }

  public update(customer: CustomerDto): Observable<CustomerDto> {
    const command = new UpdateCustomerCommand({
      ...customer
    });
    return this.apiCustomersClient.update(customer.id, command)
      .pipe(
        switchMap(() => {
          return this.getById(customer.id, true);
        }),
      );
  }

  public delete(id: string): Observable<void> {
    const command = new DeleteCustomerCommand();
    return this.apiCustomersClient.delete(id, command)
      .pipe(
        tap(() => {
          this.removeFromCache(id);
        }),
      );
  }

  private subscribeToEvents(): void {
    this.subscribeToUpdatedEvent();
    this.subscribeToDeletedEvent();
  }

  private subscribeToUpdatedEvent(): void {
    this.customerEventsService.customerUpdated$
      .pipe(
        filter(event => this.cache.value.find(x => x.id === event.id) != null),
        // todo: filter created by self
        mergeMap(event => combineLatest([of(event), this.getById(event.id, true)]))
      )
      .subscribe(([event, customer]) => {
        super.addToOrReplaceCache(customer);
        this.customerUpdatedInStore.next([event, customer]);
      });
  }

  private subscribeToDeletedEvent(): void {
    this.customerEventsService.customerDeleted$
      .pipe(
        filter(event => this.cache.value.find(x => x.id === event.id) != null),
      )
      .subscribe(event => {
        this.removeFromCache(event.id);
        this.customerDeletedFromStore.next(event);
      });
  }
}
