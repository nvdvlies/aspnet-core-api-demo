import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import {
  ApiCustomersClient,
  ApiException,
  AuditlogDto,
  GetCustomerAuditlogQueryResult,
  ProblemDetails
} from '@api/api.generated.clients';
import { AuditlogTableData } from '@shared/components/auditlog-table/auditlog-table.component';
import {
  BehaviorSubject,
  catchError,
  combineLatest,
  debounceTime,
  finalize,
  map,
  Observable,
  of,
  tap
} from 'rxjs';

class ViewModel implements AuditlogTableData {
  id: string | undefined;
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

@Component({
  selector: 'app-customer-auditlog',
  templateUrl: './customer-auditlog.component.html',
  styleUrls: ['./customer-auditlog.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CustomerAuditlogComponent implements OnInit {
  private readonly pageSize = 10;
  private readonly id = new BehaviorSubject<string | undefined>(undefined);
  private readonly isLoadingForFirstTime = new BehaviorSubject<boolean>(true);
  private readonly isLoading = new BehaviorSubject<boolean>(true);
  private readonly queryResult = new BehaviorSubject<GetCustomerAuditlogQueryResult | undefined>(
    undefined
  );
  private readonly problemDetails = new BehaviorSubject<ProblemDetails | ApiException | undefined>(
    undefined
  );

  private id$ = this.id.asObservable();
  private isLoadingForFirstTime$ = this.isLoadingForFirstTime.asObservable();
  private isLoading$ = this.isLoading.asObservable();
  private queryResult$ = this.queryResult.asObservable();
  private problemDetails$ = this.problemDetails.asObservable();

  private vm: Readonly<ViewModel> | undefined;

  public vm$ = combineLatest([
    this.id$,
    this.isLoadingForFirstTime$,
    this.isLoading$,
    this.queryResult$,
    this.problemDetails$
  ]).pipe(
    debounceTime(0),
    map(([id, isLoadingForFirstTime, isLoading, queryResult, problemDetails]) => {
      const vm: ViewModel = {
        id,
        items: queryResult?.auditlogs ?? [],
        isLoadingForFirstTime,
        isLoading,
        problemDetails,
        pageIndex: queryResult?.pageIndex,
        pageSize: this.pageSize,
        totalItems: queryResult?.totalItems,
        totalPages: queryResult?.totalPages,
        hasPreviousPage: queryResult?.hasPreviousPage,
        hasNextPage: queryResult?.hasNextPage
      };

      return vm;
    }),
    tap((vm) => (this.vm = vm))
  ) as Observable<ViewModel>;

  constructor(
    private readonly route: ActivatedRoute,
    private readonly apiCustomersClient: ApiCustomersClient
  ) {}

  public ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id == null) {
      throw new Error(`Couldn't find parameter 'id' in route parameters.`);
    }
    this.id.next(id);

    this.search(0);
  }

  public search(pageIndex: number): void {
    this.isLoading.next(true);
    this.queryResult.next(undefined);
    this.problemDetails.next(undefined);

    this.apiCustomersClient
      .getCustomerAuditlog(this.id.value!, pageIndex, this.pageSize)
      .pipe(
        catchError((error: ProblemDetails | ApiException) => {
          this.problemDetails.next(error);
          const result = new GetCustomerAuditlogQueryResult({
            customerId: this.id.value!,
            auditlogs: [],
            pageIndex: pageIndex,
            pageSize: this.pageSize,
            totalItems: 0,
            totalPages: 0,
            hasNextPage: false,
            hasPreviousPage: false
          });
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
        this.queryResult.next(response);
      });
  }
}
