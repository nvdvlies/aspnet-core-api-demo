import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { ApiException, ProblemDetails } from '@api/api.generated.clients';
import { AuthService } from '@auth0/auth0-angular';
import { CurrentUserService } from '@shared/services/current-user.service';
import { ApplicationSettingsService } from '@shared/services/application-settings.service';
import { UserFeatureFlagService } from '@shared/services/user-feature-flag.service';
import { BehaviorSubject, combineLatest, map, Observable } from 'rxjs';
import { UserPreferencesService } from '@shared/services/user-preferences.service';
import { RoleService } from '@shared/services/role.service';
import { environment } from '@env/environment';
import { PermissionService } from '@shared/services/permission.service';
import { UserPermissionService } from '@shared/services/user-permission.service';
import { PermissionGroupService } from '@shared/services/permission-group.service';

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
    public readonly userFeatureFlagService: UserFeatureFlagService,
    public readonly applicationSettingsService: ApplicationSettingsService,
    public readonly currentUserService: CurrentUserService,
    public readonly userPreferencesService: UserPreferencesService,
    public readonly roleService: RoleService,
    public readonly permissionService: PermissionService,
    public readonly userPermissionService: UserPermissionService,
    public readonly permissionGroupService: PermissionGroupService
  ) {}

  public ngOnInit(): void {
    this.authService.isAuthenticated$.subscribe((isAuthenticated) => {
      if (!isAuthenticated) {
        this.authService.loginWithRedirect({ redirect_uri: environment.auth.redirectUri });
      }
    });

    combineLatest([
      this.userFeatureFlagService.isInitialized$,
      this.applicationSettingsService.isInitialized$,
      this.currentUserService.isInitialized$,
      this.userPreferencesService.isInitialized$,
      this.roleService.isInitialized$,
      this.permissionService.isInitialized$,
      this.userPermissionService.isInitialized$,
      this.permissionGroupService.isInitialized$
    ]).subscribe(
      ([
        featureFlags,
        applicationSettings,
        currentUser,
        userPreferences,
        roles,
        permissions,
        userPermissions,
        permissionGroups
      ]) =>
        this.resolversAreInitialized.next(
          featureFlags &&
            applicationSettings &&
            currentUser &&
            userPreferences &&
            roles &&
            permissions &&
            userPermissions &&
            permissionGroups
        )
    );

    combineLatest([
      this.userFeatureFlagService.initializationProblemDetails$,
      this.applicationSettingsService.initializationProblemDetails$,
      this.currentUserService.initializationProblemDetails$,
      this.userPreferencesService.initializationProblemDetails$,
      this.roleService.initializationProblemDetails$,
      this.permissionService.initializationProblemDetails$,
      this.userPermissionService.initializationProblemDetails$,
      this.permissionGroupService.initializationProblemDetails$
    ]).subscribe(
      ([
        featureFlags,
        applicationSettings,
        currentUser,
        userPreferences,
        roles,
        permissions,
        userPermissions,
        permissionGroups
      ]) =>
        this.resolverProblemDetails.next(
          featureFlags ??
            applicationSettings ??
            currentUser ??
            userPreferences ??
            roles ??
            permissions ??
            userPermissions ??
            permissionGroups
        )
    );
  }
}
