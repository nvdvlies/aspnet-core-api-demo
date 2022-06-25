import { Injectable, OnDestroy } from '@angular/core';
import { ApiException, AuditlogDto, ProblemDetails } from '@api/api.generated.clients';
import { UserLookupService } from '@shared/services/user-lookup.service';
import {
  BehaviorSubject,
  combineLatest,
  switchMap,
  map,
  Observable,
  of,
  Subject,
  debounceTime,
  catchError,
  finalize
} from 'rxjs';

export class AuditlogContext implements IAuditlog {
  items: AuditlogDto[] = [];
  isLoadingForFirstTime: boolean = true;
  isLoading: boolean = true;
  pageIndex: number | undefined;
  pageSize: number | undefined;
  totalItems: number | undefined;
  totalPages: number | undefined;
  hasPreviousPage: boolean | undefined;
  hasNextPage: boolean | undefined;
  problemDetails: ProblemDetails | ApiException | undefined;
}

export interface IAuditlog {
  items: AuditlogDto[];
  pageIndex: number | undefined;
  pageSize: number | undefined;
  totalItems: number | undefined;
  totalPages: number | undefined;
  hasPreviousPage: boolean | undefined;
  hasNextPage: boolean | undefined;
}

@Injectable()
export abstract class AuditlogBase implements OnDestroy {
  protected abstract searchFunction: (pageIndex?: number) => Observable<IAuditlog>;

  protected readonly pageSize = 10;
  protected readonly isLoadingForFirstTime = new BehaviorSubject<boolean>(true);
  protected readonly isLoading = new BehaviorSubject<boolean>(true);
  protected readonly auditlog = new BehaviorSubject<IAuditlog | undefined>(undefined);
  protected readonly problemDetails = new BehaviorSubject<
    ProblemDetails | ApiException | undefined
  >(undefined);
  protected readonly onDestroy = new Subject<void>();

  protected isLoadingForFirstTime$ = this.isLoadingForFirstTime.asObservable();
  protected isLoading$ = this.isLoading.asObservable();
  protected problemDetails$ = this.problemDetails.asObservable();
  protected auditlog$ = this.auditlog.asObservable();
  protected onDestroy$ = this.onDestroy.asObservable();

  protected abstract entityName: string | undefined;

  public context$ = combineLatest([
    this.isLoadingForFirstTime$,
    this.isLoading$,
    this.problemDetails$,
    this.auditlog$
  ]).pipe(
    debounceTime(0),
    map(([isLoadingForFirstTime, isLoading, problemDetails, auditlog]) => {
      const vm: AuditlogContext = {
        items: auditlog?.items ?? [],
        isLoadingForFirstTime,
        isLoading,
        problemDetails,
        pageIndex: auditlog?.pageIndex,
        pageSize: this.pageSize,
        totalItems: auditlog?.totalItems,
        totalPages: auditlog?.totalPages,
        hasPreviousPage: auditlog?.hasPreviousPage,
        hasNextPage: auditlog?.hasNextPage
      };
      return vm;
    })
  ) as Observable<AuditlogContext>;

  constructor(protected readonly userLookupService: UserLookupService) {}

  public search(pageIndex?: number): void {
    this.isLoading.next(true);
    this.auditlog.next(undefined);
    this.problemDetails.next(undefined);

    this.searchFunction(pageIndex)
      .pipe(
        switchMap((response) => {
          const uniqueUserIds = [...new Set(response.items?.map((x) => x.modifiedBy!))];
          return this.userLookupService.getByIds(uniqueUserIds).pipe(map(() => response));
        }),
        catchError((error: ProblemDetails | ApiException) => {
          this.problemDetails.next(error);
          const result: IAuditlog = {
            items: [],
            pageIndex: pageIndex ?? 0,
            pageSize: this.pageSize,
            totalItems: 0,
            totalPages: 0,
            hasNextPage: false,
            hasPreviousPage: false
          };
          return of(result);
        }),
        finalize(() => {
          this.isLoading.next(false);
          if (this.isLoadingForFirstTime.value === true) {
            this.isLoadingForFirstTime.next(false);
          }
        })
      )
      .subscribe((response) => {
        this.auditlog.next(response);
      });
  }

  public ngOnDestroy(): void {
    this.onDestroy.next();
    this.onDestroy.complete();
  }
}
