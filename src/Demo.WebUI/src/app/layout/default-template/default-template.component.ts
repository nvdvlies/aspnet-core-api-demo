import { ChangeDetectionStrategy, Component } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import { environment } from '@env/environment';

@Component({
  templateUrl: './default-template.component.html',
  styleUrls: ['./default-template.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DefaultTemplateComponent {
  constructor(public authService: AuthService) {}

  public logout(): void {
    this.authService.logout({ returnTo: environment.auth.redirectUri });
  }
}
