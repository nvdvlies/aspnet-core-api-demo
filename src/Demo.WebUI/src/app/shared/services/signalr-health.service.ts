import { Injectable } from '@angular/core';
import { SignalRService } from '@api/signalr.generated.services';
import { AuthService } from '@auth0/auth0-angular';
import { BehaviorSubject, filter, map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SignalrHealthService {
  private isAuthenticated: boolean = false;

  private readonly isHealthy = new BehaviorSubject<boolean>(true);

  public isHealthy$ = this.isHealthy
    .asObservable()
    .pipe(map((isHealthy) => (this.isAuthenticated ? isHealthy : true)));

  constructor(
    private readonly authService: AuthService,
    private readonly signalRService: SignalRService
  ) {
    this.authService.isAuthenticated$.subscribe(
      (isAuthenticated) => (this.isAuthenticated = isAuthenticated)
    );

    this.signalRService.hubConnection.onreconnecting((error) => {
      this.isHealthy.next(false);
    });

    this.signalRService.hubConnection.onclose((error) => {
      this.isHealthy.next(false);
    });

    this.signalRService.hubConnection.onreconnected(() => {
      this.isHealthy.next(true);
    });
  }
}
