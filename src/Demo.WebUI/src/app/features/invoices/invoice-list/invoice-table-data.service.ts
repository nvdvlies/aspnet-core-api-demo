import { Injectable } from '@angular/core';
import { FormControl } from '@angular/forms';
import { combineLatest, debounceTime, forkJoin, map, Observable, switchMap } from 'rxjs';
import {
  ApiInvoicesClient,
  CustomerLookupDto,
  InvoiceDto,
  SearchInvoiceDto,
  SearchInvoicesOrderByEnum
} from '@api/api.generated.clients';
import { InvoiceEventsService } from '@api/signalr.generated.services';
import { InvoiceStoreService } from '@domain/invoice/invoice-store.service';
import {
  ITableFilterCriteria,
  TableFilterCriteria
} from '@shared/directives/table-filter/table-filter-criteria';
import {
  TableDataBase,
  TableDataContext,
  TableDataSearchResult
} from '@shared/base/table-data-base';
import { CustomerLookupService } from '@shared/services/customer-lookup.service';

export declare type InvoiceSortColumn = 'InvoiceNumber' | 'InvoiceDate' | undefined;

export class InvoiceTableFilterCriteria
  extends TableFilterCriteria
  implements ITableFilterCriteria
{
  override sortColumn: InvoiceSortColumn;

  constructor() {
    super();
    this.sortColumn = 'InvoiceNumber';
    this.sortDirection = 'desc';
  }
}
export interface InvoiceTableDataContext extends TableDataContext<SearchInvoiceDto> {
  criteria: InvoiceTableFilterCriteria | undefined;
}

@Injectable()
export class InvoiceTableDataService extends TableDataBase<SearchInvoiceDto> {
  public searchTerm = new FormControl();

  public observe$ = combineLatest([this.observeInternal$]).pipe(
    debounceTime(0),
    map(([context]) => {
      return {
        ...context
      } as InvoiceTableDataContext;
    })
  ) as Observable<InvoiceTableDataContext>;

  protected entityUpdatedInStore$ = this.invoiceStoreService.invoiceUpdatedInStore$;
  protected entityDeletedEvent$ = this.invoiceEventsService.invoiceDeleted$;

  constructor(
    private readonly apiInvoicesClient: ApiInvoicesClient,
    private readonly invoiceStoreService: InvoiceStoreService,
    private readonly invoiceEventsService: InvoiceEventsService,
    private readonly customerLookupService: CustomerLookupService
  ) {
    super();
    this.init(new InvoiceTableFilterCriteria());
    this.search();
  }

  protected searchFunction = (criteria: InvoiceTableFilterCriteria) => {
    return this.apiInvoicesClient
      .search(
        this.sortColumn(criteria.sortColumn),
        this.sortbyDescending(criteria.sortDirection),
        criteria.pageIndex,
        criteria.pageSize,
        this.searchTerm.value
      )
      .pipe(
        map((response) => {
          return {
            items: response.invoices,
            pageIndex: response.pageIndex,
            pageSize: response.pageSize,
            totalItems: response.totalItems,
            totalPages: response.totalPages,
            hasPreviousPage: response.hasPreviousPage,
            hasNextPage: response.hasNextPage
          } as TableDataSearchResult<SearchInvoiceDto>;
        })
      );
  };

  private sortColumn(sortColumn: InvoiceSortColumn): SearchInvoicesOrderByEnum | undefined {
    switch (sortColumn) {
      case undefined:
        return undefined;
      case 'InvoiceNumber':
        return SearchInvoicesOrderByEnum.InvoiceNumber;
      case 'InvoiceDate':
        return SearchInvoicesOrderByEnum.InvoiceDate;
      default: {
        const exhaustiveCheck: never = sortColumn; // this line will result in a compiler error when InvoiceSortColumn contains an option that's not handled in the switch statement.
        throw new Error(exhaustiveCheck);
      }
    }
  }

  protected getByIdFunction = (id: string) =>
    this.invoiceStoreService.getById(id).pipe(
      map((invoice) => {
        return new SearchInvoiceDto(invoice);
      })
    );
}
