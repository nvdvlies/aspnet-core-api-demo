import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ApiApplicationSettingsClient } from '@api/api.generated.clients';
import { AuditlogBase, AuditlogContext, IAuditlog } from '@shared/base/auditlog-base';
import { UserLookupService } from '@shared/services/user-lookup.service';
import { combineLatest, debounceTime, map, Observable, tap } from 'rxjs';

class ViewModel extends AuditlogContext {}

@Component({
  selector: 'app-application-settings-auditlog',
  templateUrl: './application-settings-auditlog.component.html',
  styleUrls: ['./application-settings-auditlog.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ApplicationSettingsAuditlogComponent extends AuditlogBase implements OnInit {
  public entityName = 'ApplicationSettings';
  protected searchFunction = (pageIndex?: number) => {
    return this.apiApplicationSettingsClient
      .getApplicationSettingsAuditlog(pageIndex, this.pageSize)
      .pipe(
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
    private readonly apiApplicationSettingsClient: ApiApplicationSettingsClient,
    userLookupService: UserLookupService
  ) {
    super(userLookupService);
  }

  public ngOnInit(): void {
    this.search();
  }
}
