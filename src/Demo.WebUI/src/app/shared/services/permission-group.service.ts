import { Injectable } from '@angular/core';
import {
  ApiException,
  ProblemDetails,
  PermissionGroupDto,
  ApiPermissionsClient
} from '@api/api.generated.clients';
import { AuthService } from '@auth0/auth0-angular';
import { BehaviorSubject, catchError, Observable, of, switchMap, tap, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PermissionGroupService {
  public permissionGroups: PermissionGroupDto[] = [];

  private isInitialized = new BehaviorSubject<boolean>(false);
  private initializationProblemDetails = new BehaviorSubject<
    ProblemDetails | ApiException | undefined
  >(undefined);

  public isInitialized$ = this.isInitialized.asObservable();
  public initializationProblemDetails$ = this.initializationProblemDetails.asObservable();

  constructor(
    private readonly apiPermissionsClient: ApiPermissionsClient,
    private readonly authService: AuthService
  ) {
    this.authService.isAuthenticated$.subscribe((isAuthenticated) => {
      if (!isAuthenticated) {
        this.permissionGroups = [];
        if (this.isInitialized.value) {
          this.isInitialized.next(false);
        }
      }
    });
  }

  public init(skipCache: boolean = false): Observable<boolean> {
    if (skipCache || !this.isInitialized.value) {
      return this.apiPermissionsClient.getAllPermissionGroups().pipe(
        catchError((error: ProblemDetails | ApiException) => {
          this.initializationProblemDetails.next(error);
          return throwError(
            () => new Error('An error occured while initializing permission groups')
          );
        }),
        tap((result) => (this.permissionGroups = result.permissionGroups ?? [])),
        tap(() => this.isInitialized.next(true)),
        switchMap(() => of(true))
      );
    } else {
      return of(true);
    }
  }
}
