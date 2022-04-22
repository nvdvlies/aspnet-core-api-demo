import { Component, OnInit } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import { FeatureFlagService } from '@shared/services/feature-flag.service';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  public resolversAreInitialized = false;

  constructor(
    public readonly authService: AuthService,
    public readonly featureFlagService: FeatureFlagService
  ) {
    forkJoin([this.featureFlagService.isInitialized$]).subscribe(
      () => (this.resolversAreInitialized = true)
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
