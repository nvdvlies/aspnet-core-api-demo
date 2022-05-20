import { Injectable } from '@angular/core';
import { FormControl } from '@angular/forms';
import { combineLatest, debounceTime, map, Observable, tap } from 'rxjs';
import {
  ApiCustomersClient,
  SearchCustomerDto,
  SearchCustomersOrderByEnum
} from '@api/api.generated.clients';
import { CustomerEventsService } from '@api/signalr.generated.services';
import { CustomerStoreService } from '@domain/customer/customer-store.service';
import {
  ITableFilterCriteria,
  TableDataBase,
  TableDataContext,
  TableDataSearchResult,
  TableFilterCriteria
} from '@shared/base/table-data-base';
import {
  CustomerListPageSettings,
  CustomerListPageSettingsService
} from './customer-list-page-settings.service';

export declare type CustomerSortColumn = 'code' | 'name' | undefined;

export class CustomerTableFilterCriteria
  extends TableFilterCriteria
  implements ITableFilterCriteria
{
  override sortColumn: CustomerSortColumn;

  constructor(pageSettings?: CustomerListPageSettings) {
    super(pageSettings?.pageSize);
    this.sortColumn = pageSettings?.sortColumn ?? 'code';
    this.sortDirection = pageSettings?.sortDirection ?? 'asc';
  }
}
export interface CustomerTableDataContext extends TableDataContext<SearchCustomerDto> {
  criteria: CustomerTableFilterCriteria | undefined;
}

@Injectable()
export class CustomerTableDataService extends TableDataBase<SearchCustomerDto> {
  public searchTerm = new FormControl();

  public observe$: Observable<CustomerTableDataContext> = combineLatest([
    this.observeInternal$
  ]).pipe(
    debounceTime(0),
    map(([baseContext]) => {
      const context: CustomerTableDataContext = {
        ...baseContext,
        criteria: baseContext.criteria as CustomerTableFilterCriteria
      };
      return context;
    })
  );

  protected entityUpdatedInStore$ = this.customerStoreService.customerUpdatedInStore$;
  protected entityDeletedEvent$ = this.customerEventsService.customerDeleted$;

  constructor(
    private readonly apiCustomersClient: ApiCustomersClient,
    private readonly customerStoreService: CustomerStoreService,
    private readonly customerEventsService: CustomerEventsService,
    private readonly customerListPageSettingsService: CustomerListPageSettingsService
  ) {
    super();
    const pageSettings = this.customerListPageSettingsService.settings;
    this.init(new CustomerTableFilterCriteria(pageSettings));
    this.search();
  }

  protected searchFunction = (criteria: CustomerTableFilterCriteria) => {
    this.customerListPageSettingsService.update({
      pageSize: criteria.pageSize,
      sortColumn: criteria.sortColumn,
      sortDirection: criteria.sortDirection
    });

    return this.apiCustomersClient
      .search(
        this.sortColumn(criteria.sortColumn),
        this.sortbyDescending(criteria.sortDirection),
        criteria.pageIndex,
        criteria.pageSize,
        this.searchTerm.value
      )
      .pipe(
        map((response) => {
          const result: TableDataSearchResult<SearchCustomerDto> = {
            items: response.customers,
            pageIndex: response.pageIndex,
            pageSize: response.pageSize,
            totalItems: response.totalItems,
            totalPages: response.totalPages,
            hasPreviousPage: response.hasPreviousPage,
            hasNextPage: response.hasNextPage
          };
          return result;
        })
      );
  };

  private sortColumn(sortColumn: CustomerSortColumn): SearchCustomersOrderByEnum | undefined {
    switch (sortColumn) {
      case undefined:
        return undefined;
      case 'code':
        return SearchCustomersOrderByEnum.Code;
      case 'name':
        return SearchCustomersOrderByEnum.Name;
      default: {
        const exhaustiveCheck: never = sortColumn; // force compiler error on missing options
        throw new Error(exhaustiveCheck);
      }
    }
  }

  protected getByIdFunction = (id: string) =>
    this.customerStoreService.getById(id).pipe(
      map((customer) => {
        return new SearchCustomerDto(customer);
      })
    );
}
