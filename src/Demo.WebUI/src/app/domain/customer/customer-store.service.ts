import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';
import { CustomerDto, ApiCustomersClient, CreateCustomerCommand, UpdateCustomerCommand, DeleteCustomerCommand } from '@api/api.generated.clients';
import { CustomerUpdatedEvent, CustomerDeletedEvent, CustomerEventsService } from '@api/signalr.generated.services';
import { StoreBase } from '@domain/shared/store-base';

@Injectable({
  providedIn: 'root'
})
export class CustomerStoreService extends StoreBase<CustomerDto> {
  public readonly customers$ = this.cache.asObservable();
  public readonly customerUpdatedInStore$ = this.entityUpdatedInStore.asObservable() as Observable<[CustomerUpdatedEvent, CustomerDto]>;
  public readonly customerDeletedFromStore$ = this.entityDeletedFromStore.asObservable() as Observable<CustomerDeletedEvent>;
  public readonly createdBySelf$ = this.createdBySelf.asObservable();

  protected entityUpdatedEvent$ = this.customerEventsService.customerUpdated$;
  protected entityDeletedEvent$ = this.customerEventsService.customerDeleted$;
  
  constructor(
    private apiCustomersClient: ApiCustomersClient,
    private customerEventsService: CustomerEventsService
  ) {
    super();
    super.init();
  }

  protected getByIdFunction = (id: string) => {
    return this.apiCustomersClient.getCustomerById(id).pipe(map(x => x.customer!));
  };

  public override getById(id: string, skipCache: boolean = false): Observable<CustomerDto> {
    return super.getById(id, skipCache);
  }

  protected createFunction = (customer: CustomerDto) => {
    const command = new CreateCustomerCommand({ ...customer });
    return this.apiCustomersClient.create(command).pipe(map(response => response.id));
  };

  public override create(customer: CustomerDto): Observable<CustomerDto> {
    return super.create(customer);
  }

  protected updateFunction = (customer: CustomerDto) => {
    const command = new UpdateCustomerCommand({ ...customer });
    return this.apiCustomersClient.update(customer.id, command);
  };

  public override update(customer: CustomerDto): Observable<CustomerDto> {
    return super.update(customer);
  }

  protected deleteFunction = (id: string) => {
    const command = new DeleteCustomerCommand();
    return this.apiCustomersClient.delete(id, command);
  };

  public override delete(id: string): Observable<void> {
    return super.delete(id);
  }
}