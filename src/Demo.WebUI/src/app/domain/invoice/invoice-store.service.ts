import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';
import { InvoiceDto, ApiInvoicesClient, CreateInvoiceCommand, UpdateInvoiceCommand, DeleteInvoiceCommand, MarkInvoiceAsPaidCommand } from '@api/api.generated.clients';
import { InvoiceUpdatedEvent, InvoiceDeletedEvent, InvoiceEventsService } from '@api/signalr.generated.services';
import { StoreBase } from '@domain/shared/store-base';

@Injectable({
  providedIn: 'root'
})
export class InvoiceStoreService extends StoreBase<InvoiceDto> {
  public readonly invoices$ = this.cache.asObservable();
  public readonly invoiceUpdatedInStore$ = this.entityUpdatedInStore.asObservable() as Observable<[InvoiceUpdatedEvent, InvoiceDto]>;
  public readonly invoiceDeletedFromStore$ = this.entityDeletedFromStore.asObservable() as Observable<InvoiceDeletedEvent>;
  public readonly createdBySelf$ = this.createdBySelf.asObservable();

  protected entityUpdatedEvent$ = this.invoiceEventsService.invoiceUpdated$;
  protected entityDeletedEvent$ = this.invoiceEventsService.invoiceDeleted$;
  
  constructor(
    private apiInvoicesClient: ApiInvoicesClient,
    private invoiceEventsService: InvoiceEventsService
  ) {
    super();
    super.init();
  }

  protected getByIdFunction = (id: string) => {
    return this.apiInvoicesClient.getInvoiceById(id).pipe(map(x => x.invoice!));
  };

  public override getById(id: string, skipCache: boolean = false): Observable<InvoiceDto> {
    return super.getById(id, skipCache);
  }

  protected createFunction = (invoice: InvoiceDto) => {
    const command = new CreateInvoiceCommand({ ...invoice });
    return this.apiInvoicesClient.create(command).pipe(map(response => response.id));
  };

  public override create(invoice: InvoiceDto): Observable<InvoiceDto> {
    return super.create(invoice);
  }

  protected updateFunction = (invoice: InvoiceDto) => {
    const command = new UpdateInvoiceCommand({ ...invoice });
    return this.apiInvoicesClient.update(invoice.id, command);
  };

  public override update(invoice: InvoiceDto): Observable<InvoiceDto> {
    return super.update(invoice);
  }

  public markAsPaid(invoice: InvoiceDto): Observable<InvoiceDto> {
    const command = new MarkInvoiceAsPaidCommand();
    return super.update(invoice, (invoice: InvoiceDto) => this.apiInvoicesClient.markAsPaid(invoice.id, command))
  }

  protected deleteFunction = (id: string) => {
    const command = new DeleteInvoiceCommand();
    return this.apiInvoicesClient.delete(id, command);
  };

  public override delete(id: string): Observable<void> {
    return super.delete(id);
  }
}