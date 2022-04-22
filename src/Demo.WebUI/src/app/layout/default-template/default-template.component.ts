import { ChangeDetectionStrategy, Component } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';

@Component({
  templateUrl: './default-template.component.html',
  styleUrls: ['./default-template.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DefaultTemplateComponent {
  constructor(public authService: AuthService) {}

  public logout(): void {
    this.authService.logout();
  }
}
