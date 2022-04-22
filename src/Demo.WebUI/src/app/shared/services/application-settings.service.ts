import { Injectable } from '@angular/core';
import {
  ApiApplicationSettingsClient,
  ApiException,
  ApplicationSettingsDto,
  ProblemDetails
} from '@api/api.generated.clients';
import { AuthService } from '@auth0/auth0-angular';
import { BehaviorSubject, catchError, Observable, of, switchMap, tap, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApplicationSettingsService {
  private applicationSettings: ApplicationSettingsDto | undefined;

  private isInitialized = new BehaviorSubject<boolean>(false);
  private initializationProblemDetails = new BehaviorSubject<
    ProblemDetails | ApiException | undefined
  >(undefined);

  public isInitialized$ = this.isInitialized.asObservable();
  public initializationProblemDetails$ = this.initializationProblemDetails.asObservable();

  constructor(
    private readonly apiApplicationSettingsClient: ApiApplicationSettingsClient,
    private readonly authService: AuthService
  ) {
    this.authService.isAuthenticated$.subscribe((isAuthenticated) => {
      if (!isAuthenticated) {
        this.applicationSettings = undefined;
        this.isInitialized.next(false);
      }
    });
  }

  public init(skipCache: boolean = false): Observable<boolean> {
    if (skipCache || this.applicationSettings == undefined) {
      return this.apiApplicationSettingsClient.get().pipe(
        catchError((error: ProblemDetails | ApiException) => {
          this.initializationProblemDetails.next(error);
          return throwError(
            () => new Error('An error occured while initializing applicationsettings')
          );
        }),
        tap((response) => (this.applicationSettings = response.applicationSettings)),
        tap(() => this.isInitialized.next(true)),
        switchMap(() => of(true))
      );
    } else {
      return of(true);
    }
  }

  public get setting1(): boolean {
    return this.applicationSettings?.settings?.setting1 ?? false;
  }
}
