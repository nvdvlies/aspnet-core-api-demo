import { Injectable } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';

@Injectable({
  providedIn: 'root'
})
export class CurrentUserService {
  public id: string | undefined;

  constructor(private readonly authService: AuthService) {
    this.authService.user$.subscribe(user => {
      this.id = user?.sub;
    });
  }
}
