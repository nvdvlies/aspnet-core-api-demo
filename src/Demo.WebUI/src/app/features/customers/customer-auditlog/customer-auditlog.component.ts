import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ApiCustomersClient, AuditlogDto } from '@api/api.generated.clients';
import { combineLatest, debounceTime, map, Observable } from 'rxjs';

interface ViewModel {
  auditlogs?: AuditlogDto[];
}

@Component({
  templateUrl: './customer-auditlog.component.html',
  styleUrls: ['./customer-auditlog.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CustomerAuditlogComponent implements OnInit {
  // public vm$ = combineLatest([]).pipe(
  //   debounceTime(0),
  //   map(([]) => {
  //     return {} as ViewModel;
  //   })
  // ) as Observable<ViewModel>;

  constructor(
    private readonly route: ActivatedRoute,
    private readonly apiCustomersClient: ApiCustomersClient
  ) {}

  public ngOnInit(): void {
    const customerId = this.route.snapshot.paramMap.get('id');
    if (customerId) {
      this.apiCustomersClient.getCustomerAuditlog(customerId, 0, 25).subscribe((x) => {});
    }
  }
}
