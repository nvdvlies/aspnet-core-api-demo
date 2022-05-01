import { Injectable } from '@angular/core';
import {
  ApiException,
  ProblemDetails,
  ApplicationSettingsDto,
  ApplicationSettingsSettingsDto
} from '@api/api.generated.clients';
import { AuthService } from '@auth0/auth0-angular';
import { ApplicationSettingsStoreService } from '@domain/application-settings/application-settings-store.service';
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
    private readonly applicationSettingsStoreService: ApplicationSettingsStoreService,
    private readonly authService: AuthService
  ) {
    this.authService.isAuthenticated$.subscribe((isAuthenticated) => {
      if (!isAuthenticated) {
        this.applicationSettings = undefined;
        if (this.isInitialized.value) {
          this.isInitialized.next(false);
        }
      }
    });

    this.applicationSettingsStoreService.applicationSettingsUpdatedInStore$.subscribe(
      ([_, applicationSettings]) => {
        this.applicationSettings = applicationSettings;
        if (!this.isInitialized.value) {
          this.isInitialized.next(true);
        }
      }
    );
  }

  public init(skipCache: boolean = false): Observable<boolean> {
    if (skipCache || !this.isInitialized.value) {
      return this.applicationSettingsStoreService.get().pipe(
        catchError((error: ProblemDetails | ApiException) => {
          this.initializationProblemDetails.next(error);
          return throwError(
            () => new Error('An error occured while initializing application settings')
          );
        }),
        tap((applicationSettings) => (this.applicationSettings = applicationSettings)),
        tap(() => this.isInitialized.next(true)),
        switchMap(() => of(true))
      );
    } else {
      return of(true);
    }
  }

  public get settings(): ApplicationSettingsSettingsDto {
    return this.applicationSettings!.settings!;
  }
}
