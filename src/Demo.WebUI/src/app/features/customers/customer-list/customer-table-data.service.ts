import { Injectable } from '@angular/core';
import { FormControl } from '@angular/forms';
import { ApiCustomersClient, SearchCustomerDto, SearchCustomersOrderByEnum, SearchCustomersQueryResult } from '@api/api.generated.clients';
import { TableFilterCriteria } from '@shared/directives/table-filter/table-filter-criteria';
import { BehaviorSubject, combineLatest, debounceTime, finalize, map, Observable } from 'rxjs';

export interface TableDataContext<T> {
  items: T[] | undefined;
  criteria: TableFilterCriteria | undefined;
  isLoadingForFirstTime: boolean;
  isLoading: boolean;
  pageIndex: number | undefined;
  pageSize: number | undefined;
  totalItems: number | undefined;
  totalPages: number | undefined;
  hasPreviousPage: boolean | undefined;
  hasNextPage: boolean | undefined;
}

export interface CustomerTableDataContext extends TableDataContext<SearchCustomerDto> {
}

@Injectable()
export class CustomerTableDataService {
  private readonly searchResult = new BehaviorSubject<SearchCustomersQueryResult | undefined>(undefined);
  private readonly criteria = new BehaviorSubject<TableFilterCriteria>(new TableFilterCriteria());
  private readonly isLoadingForFirstTime = new BehaviorSubject<boolean>(true);
  private readonly isLoading = new BehaviorSubject<boolean>(true);

  private searchResult$ = this.searchResult.asObservable();
  private criteria$ = this.criteria.asObservable();
  private isLoadingForFirstTime$ = this.isLoadingForFirstTime.asObservable();
  private isLoading$ = this.isLoading.asObservable();

  public observe$ = combineLatest([
    this.searchResult$,
    this.criteria$,
    this.isLoadingForFirstTime$,
    this.isLoading$
  ])
    .pipe(
      debounceTime(0),
      map(([
        searchResult,
        criteria,
        isLoadingForFirstTime,
        isLoading
      ]) => {
        return {
          ...searchResult,
          items: searchResult?.customers,
          criteria,
          isLoadingForFirstTime,
          isLoading
        } as CustomerTableDataContext;
      })
    ) as Observable<CustomerTableDataContext>;

  public searchTerm = new FormControl();
  
  constructor(private readonly apiCustomersClient: ApiCustomersClient) {
    this.search(this.criteria.value);
  }

  public search(criteria: TableFilterCriteria): void {
    this.isLoading.next(true);
    this.searchResult.next(undefined);
    this.apiCustomersClient
      .search(
        criteria.sortColumn == "code" ? SearchCustomersOrderByEnum.Code : criteria.sortColumn == "name" ? SearchCustomersOrderByEnum.Name : undefined,
        criteria.sortDirection == 'desc' ? true : criteria.sortDirection == 'asc' ? false : undefined,
        criteria.pageIndex,
        criteria.pageSize,
        criteria.filters.get("searchTerm")
      )
      .pipe(
        finalize(() => {
          this.isLoading.next(false);
          if (this.isLoadingForFirstTime.value) {
            this.isLoadingForFirstTime.next(false);
          }
        })
      )
      .subscribe(response => {
        this.searchResult.next(response);
      });
  }
}