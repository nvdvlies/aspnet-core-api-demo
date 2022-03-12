import { SortDirection } from '@angular/material/sort';
import { ApiException, ProblemDetails } from '@api/api.generated.clients';
import {
  ITableFilterCriteria,
  TableFilterCriteria
} from '@shared/directives/table-filter/table-filter-criteria';
import {
  BehaviorSubject,
  catchError,
  combineLatest,
  debounceTime,
  filter,
  finalize,
  map,
  Observable,
  of
} from 'rxjs';

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
  problemDetails: ProblemDetails | ApiException | undefined;
  spotlightIdentifier: string | undefined;
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
  protected abstract searchFunction: (
    criteria: ITableFilterCriteria
  ) => Observable<TableDataSearchResult<T>>;
  protected abstract getByIdFunction: (id: string) => Observable<T>;

  protected abstract entityUpdatedInStore$: Observable<[IEntityUpdatedEvent, any]>;
  protected abstract entityDeletedEvent$: Observable<IEntityDeletedEvent>;
  protected readonly problemDetails = new BehaviorSubject<
    ProblemDetails | ApiException | undefined
  >(undefined);

  protected readonly searchResult = new BehaviorSubject<TableDataSearchResult<T> | undefined>(
    undefined
  );
  protected readonly criteria = new BehaviorSubject<ITableFilterCriteria | undefined>(undefined);
  protected readonly isLoadingForFirstTime = new BehaviorSubject<boolean>(true);
  protected readonly isLoading = new BehaviorSubject<boolean>(true);
  protected readonly spotlightIdentifier = new BehaviorSubject<string | undefined>(undefined);

  protected searchResult$ = this.searchResult.asObservable();
  protected criteria$ = this.criteria.asObservable();
  protected isLoadingForFirstTime$ = this.isLoadingForFirstTime.asObservable();
  protected isLoading$ = this.isLoading.asObservable();
  protected problemDetails$ = this.problemDetails.asObservable();
  protected spotlightIdentifier$ = this.spotlightIdentifier.asObservable();

  protected observeInternal$ = combineLatest([
    this.searchResult$,
    this.criteria$,
    this.isLoadingForFirstTime$,
    this.isLoading$,
    this.problemDetails$,
    this.spotlightIdentifier$
  ]).pipe(
    debounceTime(0),
    map(
      ([
        searchResult,
        criteria,
        isLoadingForFirstTime,
        isLoading,
        problemDetails,
        spotlightIdentifier
      ]) => {
        return {
          ...searchResult,
          criteria,
          isLoadingForFirstTime,
          isLoading,
          problemDetails,
          spotlightIdentifier
        } as TableDataContext<T>;
      }
    )
  ) as Observable<TableDataContext<T>>;

  protected init(defaultCriteria: TableFilterCriteria): void {
    this.criteria.next(defaultCriteria);
    this.subscribeToEvents();
  }

  public search(criteria?: ITableFilterCriteria): void {
    this.isLoading.next(true);
    this.searchResult.next(undefined);
    this.problemDetails.next(undefined);
    criteria ??= this.criteria.value;
    this.searchFunction(criteria!)
      .pipe(
        catchError((error: ProblemDetails | ApiException) => {
          this.problemDetails.next(error);
          return of({
            items: [],
            pageIndex: criteria!.pageIndex,
            pageSize: criteria!.pageSize,
            totalItems: 0,
            totalPages: 0,
            hasNextPage: false,
            hasPreviousPage: false
          } as TableDataSearchResult<T>);
        }),
        finalize(() => {
          this.isLoading.next(false);
          if (this.isLoadingForFirstTime.value === true) {
            this.isLoadingForFirstTime.next(false);
          }
        })
      )
      .subscribe((response) => {
        this.searchResult.next(response);
      });
  }

  public spotlight(id: string): void {
    if (!this.exists(id)) {
      // add a newly created entity on top of the list
      this.getByIdFunction(id).subscribe((item) => {
        if (this.searchResult.value != null) {
          const newItems = [item, ...(this.searchResult.value?.items ?? [])];
          this.searchResult.next({
            ...this.searchResult.value,
            items: newItems
          });
        } else {
          this.searchResult.next({
            items: [item],
            pageIndex: 0,
            pageSize: this.criteria.value?.pageSize ?? 10,
            totalItems: 1,
            totalPages: 1,
            hasPreviousPage: false,
            hasNextPage: false
          } as TableDataSearchResult<T>);
        }
        this.setSpotlightIdentifier(id);
      });
    } else {
      this.setSpotlightIdentifier(id);
    }
  }

  private setSpotlightIdentifier(id: string): void {
    this.spotlightIdentifier.next(id);
    setTimeout(() => {
      if (this.spotlightIdentifier.value === id) {
        this.spotlightIdentifier.next(undefined);
      }
    }, 4000);
  }

  public clearSpotlight(): void {
    if (this.spotlightIdentifier.value != undefined) {
      this.spotlightIdentifier.next(undefined);
    }
  }

  public onDatasourceConnect(): void {
    if (this.problemDetails.value != null) {
      this.search();
    }
  }

  private subscribeToEvents(): void {
    this.subscribeToUpdatedInStoreEvent();
    this.subscribeToDeletedEvent();
  }

  private subscribeToUpdatedInStoreEvent(): void {
    this.entityUpdatedInStore$
      .pipe(filter((event) => this.exists(event[0].id)))
      .subscribe(([event, entity]) => {
        if (this.searchResult.value?.items != null) {
          const newItems = this.searchResult.value.items.map((item, _) =>
            item.id !== event.id ? item : Object.assign(item, entity)
          );
          this.searchResult.next({
            ...this.searchResult.value,
            items: newItems
          });
        }
      });
  }

  private subscribeToDeletedEvent(): void {
    this.entityDeletedEvent$.pipe(filter((event) => this.exists(event.id))).subscribe((event) => {
      if (this.searchResult.value?.items != null) {
        const newItems = this.searchResult.value.items.filter((x) => x.id !== event.id);
        this.searchResult.next({
          ...this.searchResult.value,
          items: newItems
        });
      }
    });
  }

  protected sortbyDescending(sortDirection: SortDirection | undefined): boolean | undefined {
    return sortDirection === 'desc' ? true : sortDirection === 'asc' ? false : undefined;
  }

  private exists(id: string): boolean {
    return (
      this.searchResult.value?.items?.find((x) => x.id.toLowerCase() === id.toLowerCase()) != null
    );
  }
}
