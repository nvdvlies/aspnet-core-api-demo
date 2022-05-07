import { Injectable } from '@angular/core';
import { ApiException, ApiRolesClient, ProblemDetails, RoleDto } from '@api/api.generated.clients';
import { AuthService } from '@auth0/auth0-angular';
import { BehaviorSubject, catchError, Observable, of, switchMap, tap, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RoleService {
  public roles: RoleDto[] = [];

  private isInitialized = new BehaviorSubject<boolean>(false);
  private initializationProblemDetails = new BehaviorSubject<
    ProblemDetails | ApiException | undefined
  >(undefined);

  public isInitialized$ = this.isInitialized.asObservable();
  public initializationProblemDetails$ = this.initializationProblemDetails.asObservable();

  constructor(
    private readonly apiRolesClient: ApiRolesClient,
    private readonly authService: AuthService
  ) {
    this.authService.isAuthenticated$.subscribe((isAuthenticated) => {
      if (!isAuthenticated) {
        this.roles = [];
        if (this.isInitialized.value) {
          this.isInitialized.next(false);
        }
      }
    });
  }

  public init(skipCache: boolean = false): Observable<boolean> {
    if (skipCache || !this.isInitialized.value) {
      return this.apiRolesClient.search(undefined, undefined, 0, 9999, undefined).pipe(
        catchError((error: ProblemDetails | ApiException) => {
          this.initializationProblemDetails.next(error);
          return throwError(() => new Error('An error occured while initializing roles'));
        }),
        tap((result) => (this.roles = result.roles ?? [])),
        tap(() => this.isInitialized.next(true)),
        switchMap(() => of(true))
      );
    } else {
      return of(true);
    }
  }
}
