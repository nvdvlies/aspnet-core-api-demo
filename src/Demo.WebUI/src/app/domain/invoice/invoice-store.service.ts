import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import {
  InvoiceDto,
  ApiInvoicesClient,
  CreateInvoiceCommand,
  UpdateInvoiceCommand,
  DeleteInvoiceCommand,
  MarkInvoiceAsPaidCommand,
  MarkInvoiceAsCancelledCommand,
  MarkInvoiceAsSentCommand
} from '@api/api.generated.clients';
import {
  InvoiceUpdatedEvent,
  InvoiceDeletedEvent,
  InvoiceEventsService
} from '@api/signalr.generated.services';
import { StoreBase } from '@domain/shared/store-base';

@Injectable({
  providedIn: 'root'
})
export class InvoiceStoreService extends StoreBase<InvoiceDto> {
  public readonly invoices$ = this.cache.asObservable();
  public readonly invoiceUpdatedInStore$ = this.entityUpdatedInStore.asObservable() as Observable<
    [InvoiceUpdatedEvent, InvoiceDto]
  >;
  public readonly invoiceDeletedFromStore$ =
    this.entityDeletedFromStore.asObservable() as Observable<InvoiceDeletedEvent>;

  protected entityUpdatedEvent$ = this.invoiceEventsService.invoiceUpdated$;
  protected entityDeletedEvent$ = this.invoiceEventsService.invoiceDeleted$;

  constructor(
    private readonly apiInvoicesClient: ApiInvoicesClient,
    private readonly invoiceEventsService: InvoiceEventsService
  ) {
    super();
    super.init();
  }

  protected getByIdFunction = (id: string) => {
    return this.apiInvoicesClient.getInvoiceById(id).pipe(map((x) => x.invoice!));
  };

  public override getById(id: string, skipCache: boolean = false): Observable<InvoiceDto> {
    return super.getById(id, skipCache);
  }

  protected createFunction = (invoice: InvoiceDto) => {
    const command = new CreateInvoiceCommand();
    command.init({ ...invoice });
    return this.apiInvoicesClient.create(command).pipe(map((response) => response.id));
  };

  public override create(invoice: InvoiceDto): Observable<InvoiceDto> {
    return super.create(invoice);
  }

  protected updateFunction = (invoice: InvoiceDto) => {
    const command = new UpdateInvoiceCommand();
    command.init({ ...invoice });
    return this.apiInvoicesClient.update(invoice.id, command);
  };

  public override update(invoice: InvoiceDto): Observable<InvoiceDto> {
    return super.update(invoice);
  }

  protected markAsSentFunction = (invoice: InvoiceDto) => {
    const command = new MarkInvoiceAsSentCommand();
    return this.apiInvoicesClient.markAsSent(invoice.id, command);
  };

  public markAsSent(invoice: InvoiceDto): Observable<InvoiceDto> {
    return super.update(invoice, this.markAsSentFunction);
  }

  protected markAsPaidFunction = (invoice: InvoiceDto) => {
    const command = new MarkInvoiceAsPaidCommand();
    return this.apiInvoicesClient.markAsPaid(invoice.id, command);
  };

  public markAsPaid(invoice: InvoiceDto): Observable<InvoiceDto> {
    return super.update(invoice, this.markAsPaidFunction);
  }

  protected markAsCancelledFunction = (invoice: InvoiceDto) => {
    const command = new MarkInvoiceAsCancelledCommand();
    return this.apiInvoicesClient.markAsCancelled(invoice.id, command);
  };

  public markAsCancelled(invoice: InvoiceDto): Observable<InvoiceDto> {
    return super.update(invoice, this.markAsCancelledFunction);
  }

  protected deleteFunction = (id: string) => {
    const command = new DeleteInvoiceCommand();
    return this.apiInvoicesClient.delete(id, command);
  };

  public override delete(id: string): Observable<void> {
    return super.delete(id);
  }
}
