import { Injectable } from '@angular/core';
import { FormControl } from '@angular/forms';
import { combineLatest, debounceTime, map, Observable } from 'rxjs';
import { ApiCustomersClient, SearchCustomerDto, SearchCustomersOrderByEnum } from '@api/api.generated.clients';
import { CustomerEventsService } from '@api/signalr.generated.services';
import { CustomerStoreService } from '@domain/customer/customer-store.service';
import { TableFilterCriteria } from '@shared/directives/table-filter/table-filter-criteria';
import { TableDataBase, TableDataContext, TableDataSearchResult } from '@shared/services/table-data-base';

export interface CustomerTableDataContext extends TableDataContext<SearchCustomerDto> {
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
    this.init();
    this.search(this.criteria.value);
  }

  protected searchFunction = (criteria: TableFilterCriteria) => {
    return this.apiCustomersClient
      .search(
        criteria.sortColumn === 'code' ? SearchCustomersOrderByEnum.Code 
          : criteria.sortColumn === 'name' ? SearchCustomersOrderByEnum.Name 
          : undefined,
        criteria.sortDirection === 'desc' ? true 
          : criteria.sortDirection === 'asc' ? false 
          : undefined,
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
}