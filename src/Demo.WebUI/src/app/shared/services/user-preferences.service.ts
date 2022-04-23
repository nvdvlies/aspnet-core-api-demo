import { Injectable } from '@angular/core';
import {
  ApiException,
  ApiUserPreferencesClient,
  ProblemDetails,
  UserPreferencesDto
} from '@api/api.generated.clients';
import { AuthService } from '@auth0/auth0-angular';
import { BehaviorSubject, catchError, Observable, of, switchMap, tap, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserPreferencesService {
  private userPreferences: UserPreferencesDto | undefined;

  private isInitialized = new BehaviorSubject<boolean>(false);
  private initializationProblemDetails = new BehaviorSubject<
    ProblemDetails | ApiException | undefined
  >(undefined);

  public isInitialized$ = this.isInitialized.asObservable();
  public initializationProblemDetails$ = this.initializationProblemDetails.asObservable();

  constructor(
    private readonly apiUserPreferencesClient: ApiUserPreferencesClient,
    private readonly authService: AuthService
  ) {
    this.authService.isAuthenticated$.subscribe((isAuthenticated) => {
      if (!isAuthenticated) {
        this.userPreferences = undefined;
        this.isInitialized.next(false);
      }
    });
  }

  public init(skipCache: boolean = false): Observable<boolean> {
    if (skipCache || this.userPreferences == undefined) {
      return this.apiUserPreferencesClient.get().pipe(
        catchError((error: ProblemDetails | ApiException) => {
          this.initializationProblemDetails.next(error);
          return throwError(
            () => new Error('An error occured while initializing user preferences')
          );
        }),
        tap((response) => (this.userPreferences = response.userPreferences)),
        tap(() => this.isInitialized.next(true)),
        switchMap(() => of(true))
      );
    } else {
      return of(true);
    }
  }

  public get setting1(): boolean {
    return this.userPreferences?.preferences?.setting1 ?? false;
  }
}
