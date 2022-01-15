import { Injectable } from '@angular/core';
import { combineLatest, Observable, of, Subject } from 'rxjs';
import { tap, map, switchMap, filter, mergeMap } from 'rxjs/operators';
import { InvoiceDto, ApiInvoicesClient, CreateInvoiceCommand, UpdateInvoiceCommand, DeleteInvoiceCommand, MarkInvoiceAsPaidCommand } from '@api/api.generated.clients';
import { InvoiceUpdatedEvent, InvoiceDeletedEvent, InvoiceEventsService } from '@api/signalr.generated.services';
import { StoreBase } from '@domain/shared/store-base';

@Injectable({
  providedIn: 'root'
})
export class InvoiceStoreService extends StoreBase<InvoiceDto> {
  private readonly invoiceUpdatedInStore = new Subject<[InvoiceUpdatedEvent, InvoiceDto]>();
  private readonly invoiceDeletedFromStore = new Subject<InvoiceDeletedEvent>();
  private readonly createdBySelf = new Subject<InvoiceDto>();

  public readonly invoices$ = this.cache.asObservable();
  public readonly invoiceUpdatedInStore$ = this.invoiceUpdatedInStore.asObservable();
  public readonly invoiceDeletedFromStore$ = this.invoiceDeletedFromStore.asObservable();
  public readonly createdBySelf$ = this.createdBySelf.asObservable();

  constructor(
    private apiInvoicesClient: ApiInvoicesClient,
    private invoiceEventsService: InvoiceEventsService
  ) {
    super();
    this.subscribeToEvents();
  }

  public getById(id: string, skipCache: boolean = false): Observable<InvoiceDto> {
    let invoice = !skipCache
      ? this.cache.value.find(x => x.id === id)
      : null;

    return invoice != null
      ? of(invoice)
      : this.apiInvoicesClient.getInvoiceById(id)
        .pipe(
          map((response) => {
            invoice = new InvoiceDto({ ...response.invoice! });
            this.addToOrReplaceCache(invoice);
            return invoice;
          })
        );
  }

  public create(invoice: InvoiceDto): Observable<InvoiceDto> {
    const command = new CreateInvoiceCommand({ ...invoice });
    return this.apiInvoicesClient.create(command)
      .pipe(
        switchMap((response) => {
          return this.getById(response.id);
        }),
        tap((createdInvoice) => {
          this.createdBySelf.next(createdInvoice);
        })
      );
  }

  public update(invoice: InvoiceDto): Observable<InvoiceDto> {
    const command = new UpdateInvoiceCommand({
      ...invoice
    });
    return this.apiInvoicesClient.update(invoice.id, command)
      .pipe(
        switchMap(() => {
          return this.getById(invoice.id, true);
        }),
      );
  }

  public markAsPaid(invoice: InvoiceDto): Observable<InvoiceDto> {
    const command = new MarkInvoiceAsPaidCommand();
    return this.apiInvoicesClient.markAsPaid(invoice.id, command)
      .pipe(
        switchMap(() => {
          return this.getById(invoice.id, true);
        }),
      );
  }

  public delete(id: string): Observable<void> {
    const command = new DeleteInvoiceCommand();
    return this.apiInvoicesClient.delete(id, command)
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
    this.invoiceEventsService.invoiceUpdated$
      .pipe(
        filter(event => this.cache.value.find(x => x.id === event.id) != null),
        // todo: filter created by self
        mergeMap(event => combineLatest([of(event), this.getById(event.id, true)]))
      )
      .subscribe(([event, invoice]) => {
        super.addToOrReplaceCache(invoice);
        this.invoiceUpdatedInStore.next([event, invoice]);
      });
  }

  private subscribeToDeletedEvent(): void {
    this.invoiceEventsService.invoiceDeleted$
      .pipe(
        filter(event => this.cache.value.find(x => x.id === event.id) != null),
      )
      .subscribe(event => {
        this.removeFromCache(event.id);
        this.invoiceDeletedFromStore.next(event);
      });
  }
}
