import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { ApiException, ProblemDetails } from '@api/api.generated.clients';
import { AuthService } from '@auth0/auth0-angular';
import { CurrentUserService } from '@shared/services/current-user.service';
import { ApplicationSettingsService } from '@shared/services/application-settings.service';
import { FeatureFlagService } from '@shared/services/feature-flag.service';
import { BehaviorSubject, combineLatest, map, Observable } from 'rxjs';
import { UserPreferencesService } from '@shared/services/user-preferences.service';
import { RoleService } from '@shared/services/role.service';
import { environment } from '@env/environment';

interface ViewModel {
  resolversAreInitialized: boolean;
  resolverProblemDetails: ProblemDetails | ApiException | undefined;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AppComponent implements OnInit {
  protected readonly resolversAreInitialized = new BehaviorSubject<boolean>(false);
  protected readonly resolverProblemDetails = new BehaviorSubject<
    ProblemDetails | ApiException | undefined
  >(undefined);

  protected resolversAreInitialized$ = this.resolversAreInitialized.asObservable();
  protected resolverProblemDetails$ = this.resolverProblemDetails.asObservable();

  public vm$: Observable<ViewModel> = combineLatest([
    this.resolversAreInitialized$,
    this.resolverProblemDetails$
  ]).pipe(
    map(([resolversAreInitialized, resolverProblemDetails]) => {
      const vm: ViewModel = {
        resolversAreInitialized,
        resolverProblemDetails
      };
      return vm;
    })
  );

  constructor(
    public readonly authService: AuthService,
    public readonly featureFlagService: FeatureFlagService,
    public readonly applicationSettingsService: ApplicationSettingsService,
    public readonly currentUserService: CurrentUserService,
    public readonly userPreferencesService: UserPreferencesService,
    public readonly roleService: RoleService
  ) {}

  public ngOnInit(): void {
    this.authService.isAuthenticated$.subscribe((isAuthenticated) => {
      if (!isAuthenticated) {
        this.authService.loginWithRedirect({ redirect_uri: environment.auth.redirectUri });
      }
    });

    combineLatest([
      this.featureFlagService.isInitialized$,
      this.applicationSettingsService.isInitialized$,
      this.currentUserService.isInitialized$,
      this.userPreferencesService.isInitialized$,
      this.roleService.isInitialized$
    ]).subscribe(([featureFlags, applicationSettings, currentUser, userPreferences, roles]) =>
      this.resolversAreInitialized.next(
        featureFlags && applicationSettings && currentUser && userPreferences && roles
      )
    );

    combineLatest([
      this.featureFlagService.initializationProblemDetails$,
      this.applicationSettingsService.initializationProblemDetails$,
      this.currentUserService.initializationProblemDetails$,
      this.userPreferencesService.initializationProblemDetails$,
      this.roleService.initializationProblemDetails$
    ]).subscribe(([featureFlags, applicationSettings, currentUser, userPreferences, roles]) =>
      this.resolverProblemDetails.next(
        featureFlags ?? applicationSettings ?? currentUser ?? userPreferences ?? roles
      )
    );
  }
}
