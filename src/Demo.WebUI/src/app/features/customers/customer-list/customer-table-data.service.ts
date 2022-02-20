import { Injectable } from '@angular/core';
import { FormControl } from '@angular/forms';
import { combineLatest, debounceTime, map, Observable } from 'rxjs';
import { ApiCustomersClient, SearchCustomerDto, SearchCustomersOrderByEnum } from '@api/api.generated.clients';
import { CustomerEventsService } from '@api/signalr.generated.services';
import { CustomerStoreService } from '@domain/customer/customer-store.service';
import { ITableFilterCriteria, TableFilterCriteria } from '@shared/directives/table-filter/table-filter-criteria';
import { TableDataBase, TableDataContext, TableDataSearchResult } from '@shared/services/table-data-base';

export declare type CustomerSortColumn = 'code' | 'name' | undefined;

export class CustomerTableFilterCriteria extends TableFilterCriteria implements ITableFilterCriteria {
  override sortColumn: CustomerSortColumn;

  constructor() {
    super();
    this.sortColumn = 'code';
    this.sortDirection = 'asc';
  }
}
export interface CustomerTableDataContext extends TableDataContext<SearchCustomerDto> {
  criteria: CustomerTableFilterCriteria | undefined;
}

@Injectable()
export class CustomerTableDataService extends TableDataBase<SearchCustomerDto> {
  public searchTerm = new FormControl();

  public observe$ = combineLatest([
    this.observeInternal$
  ])
    .pipe(
      debounceTime(0),
      map(([
        context,
      ]) => {
        return {
          ...context
        } as CustomerTableDataContext;
      })
    ) as Observable<CustomerTableDataContext>;

  protected entityUpdatedInStore$ = this.customerStoreService.customerUpdatedInStore$;
  protected entityDeletedEvent$ = this.customerEventsService.customerDeleted$;

  constructor(
    private readonly apiCustomersClient: ApiCustomersClient,
    private readonly customerStoreService: CustomerStoreService,
    private readonly customerEventsService: CustomerEventsService
  ) {
    super();
    this.init(new CustomerTableFilterCriteria());
    this.search();
  }

  protected searchFunction = (criteria: CustomerTableFilterCriteria) => {
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
          return {
            items: response.customers,
            pageIndex: response.pageIndex,
            pageSize: response.pageSize,
            totalItems: response.totalItems,
            totalPages: response.totalPages,
            hasPreviousPage: response.hasPreviousPage,
            hasNextPage: response.hasNextPage
          } as TableDataSearchResult<SearchCustomerDto>
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
        const exhaustiveCheck: never = sortColumn; // this line will result in a compiler error when CustomerSortColumn contains an option that's not handled in the switch statement.
        throw new Error(exhaustiveCheck);
      }
    }
  }

  protected getByIdFunction = (id: string) => this.customerStoreService
    .getById(id)
    .pipe(
      map(customer => {
        return new SearchCustomerDto(customer);
      })
    );
}