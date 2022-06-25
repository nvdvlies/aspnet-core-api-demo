import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ApiFeatureFlagSettingsClient } from '@api/api.generated.clients';
import { AuditlogBase, AuditlogContext, IAuditlog } from '@shared/base/auditlog-base';
import { UserLookupService } from '@shared/services/user-lookup.service';
import { BehaviorSubject, combineLatest, debounceTime, map, Observable, tap } from 'rxjs';

class ViewModel extends AuditlogContext {
  name: string | undefined;
}

@Component({
  selector: 'app-feature-flag-setting-auditlog',
  templateUrl: './feature-flag-setting-auditlog.component.html',
  styleUrls: ['./feature-flag-setting-auditlog.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class FeatureFlagSettingAuditlogComponent extends AuditlogBase implements OnInit {
  public entityName = 'FeatureFlagSetting';
  protected searchFunction = (pageIndex?: number) => {
    return this.apiFeatureFlagSettingsClient
      .getFeatureFlagSettingsAuditlog(this.name.value!, pageIndex, this.pageSize)
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

  private readonly name = new BehaviorSubject<string | undefined>(undefined);

  protected name$ = this.name.asObservable();

  private vm: Readonly<ViewModel> | undefined;

  public vm$ = combineLatest([this.name$, this.context$]).pipe(
    debounceTime(0),
    map(([name, context]) => {
      const vm: ViewModel = {
        name,
        ...context
      };
      return vm;
    }),
    tap((vm) => (this.vm = vm))
  ) as Observable<ViewModel>;

  constructor(
    private readonly route: ActivatedRoute,
    private readonly apiFeatureFlagSettingsClient: ApiFeatureFlagSettingsClient,
    userLookupService: UserLookupService
  ) {
    super(userLookupService);
  }

  public ngOnInit(): void {
    const name = this.route.snapshot.paramMap.get('name');
    if (name == null) {
      throw new Error(`Couldn't find parameter 'name' in route parameters.`);
    }
    this.name.next(name);

    this.search();
  }
}
