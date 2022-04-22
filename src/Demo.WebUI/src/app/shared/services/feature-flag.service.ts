import { Injectable } from '@angular/core';
import { ApiCurrentUserClient, ApiException, ProblemDetails } from '@api/api.generated.clients';
import { AuthService } from '@auth0/auth0-angular';
import { FeatureFlag } from '@shared/enums/feature-flag.enum';
import { BehaviorSubject, catchError, Observable, of, switchMap, tap, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FeatureFlagService {
  private featureFlags: string[] | undefined;

  private isInitialized = new BehaviorSubject<boolean>(false);
  private initializationProblemDetails = new BehaviorSubject<
    ProblemDetails | ApiException | undefined
  >(undefined);

  public isInitialized$ = this.isInitialized.asObservable();
  public initializationProblemDetails$ = this.initializationProblemDetails.asObservable();

  constructor(
    private readonly apiCurrentUserClient: ApiCurrentUserClient,
    private readonly authService: AuthService
  ) {
    this.authService.isAuthenticated$.subscribe((isAuthenticated) => {
      if (!isAuthenticated) {
        this.featureFlags = undefined;
        this.isInitialized.next(false);
      }
    });
  }

  public init(skipCache: boolean = false): Observable<boolean> {
    if (skipCache || this.featureFlags == undefined) {
      return this.apiCurrentUserClient.getCurrentUserFeatureFlags().pipe(
        catchError((error: ProblemDetails | ApiException) => {
          this.initializationProblemDetails.next(error);
          return throwError(() => new Error('An error occured while initializing feature flags'));
        }),
        tap((response) => (this.featureFlags = response.featureFlags ?? [])),
        tap(() => this.isInitialized.next(true)),
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
