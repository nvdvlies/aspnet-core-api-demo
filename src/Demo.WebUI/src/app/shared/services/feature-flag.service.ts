import { Injectable } from '@angular/core';
import { ApiCurrentUserClient } from '@api/api.generated.clients';
import { CurrentUserService } from '@core/services/current-user.service';
import { FeatureFlag } from '@shared/enums/feature-flag.enum';
import { BehaviorSubject, Observable, of, switchMap, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FeatureFlagService {
  private featureFlags: string[] | undefined;

  public isInitialized$ = new BehaviorSubject<boolean>(false);

  constructor(
    private readonly apiCurrentUserClient: ApiCurrentUserClient,
    private readonly currentUserService: CurrentUserService
  ) {
    this.currentUserService.isAuthenticated$.subscribe((isAuthenticated) => {
      if (!isAuthenticated) {
        this.featureFlags = undefined;
      }
    });
  }

  public init(skipCache: boolean = false): Observable<boolean> {
    if (skipCache || this.featureFlags == undefined) {
      return this.apiCurrentUserClient.getCurrentUserFeatureFlags().pipe(
        tap((response) => (this.featureFlags = response.featureFlags ?? [])),
        tap(() => this.isInitialized$.next(true)),
        switchMap(() => of(true))
      );
    } else {
      return of(true);
    }
  }

  public isEnabled(featureFlag: keyof typeof FeatureFlag): boolean {
    return this.featureFlags?.includes(featureFlag) ?? false;
  }
}
