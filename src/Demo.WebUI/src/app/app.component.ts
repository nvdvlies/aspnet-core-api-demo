import { Component, OnInit } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import { ApplicationSettingsService } from '@shared/services/application-settings.service';
import { FeatureFlagService } from '@shared/services/feature-flag.service';
import { combineLatest } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  public resolversAreInitialized = false;

  constructor(
    public readonly authService: AuthService,
    public readonly featureFlagService: FeatureFlagService,
    public readonly applicationSettingsService: ApplicationSettingsService
  ) {
    combineLatest([
      this.featureFlagService.isInitialized$,
      this.applicationSettingsService.isInitialized$
    ]).subscribe(
      ([featureFlagsAreInitialized, applicationSettingsAreInitialized]) =>
        (this.resolversAreInitialized =
          featureFlagsAreInitialized && applicationSettingsAreInitialized)
    );
  }

  public ngOnInit(): void {
    this.authService.isAuthenticated$.subscribe((isAuthenticated) => {
      if (!isAuthenticated) {
        this.authService.loginWithRedirect();
      }
    });
  }
}
