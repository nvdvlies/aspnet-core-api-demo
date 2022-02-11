import { TableFilterCriteria } from '@shared/directives/table-filter/table-filter-criteria';
import { BehaviorSubject, combineLatest, debounceTime, filter, finalize, map, Observable } from 'rxjs';

export interface TableDataSearchResult<T> {
  items: T[] | undefined;
  pageIndex: number | undefined;
  pageSize: number | undefined;
  totalItems: number | undefined;
  totalPages: number | undefined;
  hasPreviousPage: boolean | undefined;
  hasNextPage: boolean | undefined;
}

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

export interface IEntityUpdatedEvent {
  id: string;
  updatedBy: string;
}

export interface IEntityDeletedEvent {
  id: string;
  deletedBy: string;
}

export interface ITableDataSearchResultItem {
  id: string;
}

export abstract class TableDataBase<T extends ITableDataSearchResultItem> {
  protected abstract searchFunction: (criteria: TableFilterCriteria) => Observable<TableDataSearchResult<T>>;

  protected abstract entityUpdatedInStore$: Observable<[IEntityUpdatedEvent, any]>;
  protected abstract entityDeletedEvent$: Observable<IEntityDeletedEvent>;

  protected readonly searchResult = new BehaviorSubject<TableDataSearchResult<T> | undefined>(undefined);
  protected readonly criteria = new BehaviorSubject<TableFilterCriteria>(new TableFilterCriteria());
  protected readonly isLoadingForFirstTime = new BehaviorSubject<boolean>(true);
  protected readonly isLoading = new BehaviorSubject<boolean>(true);

  protected searchResult$ = this.searchResult.asObservable();
  protected criteria$ = this.criteria.asObservable();
  protected isLoadingForFirstTime$ = this.isLoadingForFirstTime.asObservable();
  protected isLoading$ = this.isLoading.asObservable();

  protected observeInternal$ = combineLatest([
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
          criteria,
          isLoadingForFirstTime,
          isLoading
        } as TableDataContext<T>;
      })
    ) as Observable<TableDataContext<T>>;


  public search(criteria: TableFilterCriteria): void {
    this.isLoading.next(true);
    this.searchResult.next(undefined);
    this.searchFunction(criteria)
      .pipe(
        finalize(() => {
          this.isLoading.next(false);
          if (this.isLoadingForFirstTime.value === true) {
            this.isLoadingForFirstTime.next(false);
          }
        })
      )
      .subscribe(response => {
        this.searchResult.next(response);
      });
  }

  protected init(): void {
    this.subscribeToEvents();
  }

  private subscribeToEvents(): void {
    this.subscribeToUpdatedInStoreEvent();
    this.subscribeToDeletedEvent();
  }

  private subscribeToUpdatedInStoreEvent(): void {
    this.entityUpdatedInStore$
      .pipe(
        filter(event => this.searchResult.value?.items?.find(x => x.id === event[0].id) != null),
      )
      .subscribe(([event, entity]) => {
        if (this.searchResult.value?.items != null) {
          const newItems = this.searchResult.value.items.map((item, _) => item.id !== event.id ? item : Object.assign(item, entity));
          this.searchResult.next({ ...this.searchResult.value, items: newItems});
        }
      });
  }

  private subscribeToDeletedEvent(): void {
    this.entityDeletedEvent$
      .pipe(
        filter(event => this.searchResult.value?.items?.find(x => x.id === event.id) != null),
      )
      .subscribe(event => {
        if (this.searchResult.value?.items != null) {
          const newItems = this.searchResult.value.items.filter(x => x.id !== event.id);
          this.searchResult.next({ ...this.searchResult.value, items: newItems});
        }
      });
  }
}