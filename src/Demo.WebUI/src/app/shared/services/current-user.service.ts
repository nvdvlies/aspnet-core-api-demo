import { Injectable } from '@angular/core';
import { ApiException, CurrentUserDto, ProblemDetails } from '@api/api.generated.clients';
import { CurrentUserEventsService } from '@api/signalr.generated.services';
import { AuthService } from '@auth0/auth0-angular';
import { CurrentUserStoreService } from '@domain/current-user/current-user-store.service';
import { BehaviorSubject, catchError, Observable, of, switchMap, tap, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CurrentUserService {
  private currentUser: CurrentUserDto | undefined;

  private isInitialized = new BehaviorSubject<boolean>(false);
  private initializationProblemDetails = new BehaviorSubject<
    ProblemDetails | ApiException | undefined
  >(undefined);

  public isInitialized$ = this.isInitialized.asObservable();
  public initializationProblemDetails$ = this.initializationProblemDetails.asObservable();

  constructor(
    private readonly currentUserStoreService: CurrentUserStoreService,
    private readonly authService: AuthService
  ) {
    this.authService.isAuthenticated$.subscribe((isAuthenticated) => {
      if (!isAuthenticated) {
        this.currentUser = undefined;
        this.isInitialized.next(false);
      }
    });

    this.currentUserStoreService.currentUserUpdatedInStore$
      .pipe(switchMap(() => this.init(true)))
      .subscribe();
  }

  public init(skipCache: boolean = false): Observable<boolean> {
    if (skipCache || this.currentUser == undefined) {
      return this.currentUserStoreService.get().pipe(
        catchError((error: ProblemDetails | ApiException) => {
          this.initializationProblemDetails.next(error);
          return throwError(() => new Error('An error occured while initializing current user'));
        }),
        tap((currentUser) => (this.currentUser = currentUser)),
        tap(() => this.isInitialized.next(true)),
        switchMap(() => of(true))
      );
    } else {
      return of(true);
    }
  }

  public get id(): string | undefined {
    return this.currentUser?.id;
  }

  public get externalId(): string | undefined {
    return this.currentUser?.externalId;
  }

  public get fullname(): string | undefined {
    return this.currentUser?.fullname;
  }

  public get email(): string | undefined {
    return this.currentUser?.email;
  }
}
