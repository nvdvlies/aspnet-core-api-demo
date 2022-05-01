import { Injectable } from '@angular/core';
import {
  ApiException,
  ProblemDetails,
  UserPreferencesDto,
  UserPreferencesPreferencesDto
} from '@api/api.generated.clients';
import { AuthService } from '@auth0/auth0-angular';
import { UserPreferencesStoreService } from '@domain/user-preferences/user-preferences-store.service';
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
    private readonly userPreferencesStoreService: UserPreferencesStoreService,
    private readonly authService: AuthService
  ) {
    this.authService.isAuthenticated$.subscribe((isAuthenticated) => {
      if (!isAuthenticated) {
        this.userPreferences = undefined;
        if (this.isInitialized.value) {
          this.isInitialized.next(false);
        }
      }
    });

    this.userPreferencesStoreService.userPreferencesUpdatedInStore$.subscribe(
      ([_, userPreferences]) => {
        this.userPreferences = userPreferences;
        if (!this.isInitialized.value) {
          this.isInitialized.next(true);
        }
      }
    );
  }

  public init(skipCache: boolean = false): Observable<boolean> {
    if (skipCache || !this.isInitialized.value) {
      return this.userPreferencesStoreService.get().pipe(
        catchError((error: ProblemDetails | ApiException) => {
          this.initializationProblemDetails.next(error);
          return throwError(
            () => new Error('An error occured while initializing user preferences')
          );
        }),
        tap((userPreferences) => (this.userPreferences = userPreferences)),
        tap(() => this.isInitialized.next(true)),
        switchMap(() => of(true))
      );
    } else {
      return of(true);
    }
  }

  public get preferences(): UserPreferencesPreferencesDto {
    return this.userPreferences!.preferences!;
  }
}
