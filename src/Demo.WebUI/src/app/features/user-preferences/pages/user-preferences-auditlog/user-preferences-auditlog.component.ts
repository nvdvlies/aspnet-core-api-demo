import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ApiUserPreferencesClient } from '@api/api.generated.clients';
import { AuditlogBase, AuditlogContext, IAuditlog } from '@shared/base/auditlog-base';
import { UserLookupService } from '@shared/services/user-lookup.service';
import { combineLatest, debounceTime, map, Observable, tap } from 'rxjs';

class ViewModel extends AuditlogContext {}

@Component({
  selector: 'app-user-preferences-auditlog',
  templateUrl: './user-preferences-auditlog.component.html',
  styleUrls: ['./user-preferences-auditlog.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UserPreferencesAuditlogComponent extends AuditlogBase implements OnInit {
  public entityName = 'UserPreferences';
  protected searchFunction = (pageIndex?: number) => {
    return this.apiUserPreferencesClient.getUserPreferencesAuditlog(pageIndex, this.pageSize).pipe(
      map((response) => {
        const auditlog: IAuditlog = {
          items: response?.auditlogs ?? [],
          ...response
        };
        return auditlog;
      })
    );
  };

  private vm: Readonly<ViewModel> | undefined;

  public vm$ = combineLatest([this.context$]).pipe(
    debounceTime(0),
    map(([context]) => {
      const vm: ViewModel = {
        ...context
      };
      return vm;
    }),
    tap((vm) => (this.vm = vm))
  ) as Observable<ViewModel>;

  constructor(
    private readonly route: ActivatedRoute,
    private readonly apiUserPreferencesClient: ApiUserPreferencesClient,
    userLookupService: UserLookupService
  ) {
    super(userLookupService);
  }

  public ngOnInit(): void {
    this.search();
  }
}
