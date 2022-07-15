import { Injectable } from '@angular/core';
import {
  ApiCurrentUserClient,
  ApiException,
  ProblemDetails,
  PermissionDto
} from '@api/api.generated.clients';
import { CurrentUserEventsService, RoleEventsService } from '@api/signalr.generated.services';
import { AuthService } from '@auth0/auth0-angular';
import { Permission } from '@shared/enums/permission.enum';
import {
  BehaviorSubject,
  catchError,
  map,
  Observable,
  of,
  Subject,
  switchMap,
  tap,
  throwError
} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserPermissionService {
  public permissions: PermissionDto[] = [];

  private isInitialized = new BehaviorSubject<boolean>(false);
  private initializationProblemDetails = new BehaviorSubject<
    ProblemDetails | ApiException | undefined
  >(undefined);
  private updated = new Subject<void>();

  public isInitialized$ = this.isInitialized.asObservable();
  public initializationProblemDetails$ = this.initializationProblemDetails.asObservable();
  public updated$ = this.updated.asObservable();

  constructor(
    private readonly apiCurrentUserClient: ApiCurrentUserClient,
    private readonly authService: AuthService,
    private readonly roleEventsService: RoleEventsService,
    private readonly currentUserEventsService: CurrentUserEventsService
  ) {
    this.authService.isAuthenticated$.subscribe((isAuthenticated) => {
      if (!isAuthenticated) {
        this.permissions = [];
        if (this.isInitialized.value) {
          this.isInitialized.next(false);
        }
      }
    });

    this.roleEventsService.roleUpdated$
      .pipe(
        switchMap(() => {
          return this.init(true);
        })
      )
      .subscribe();

    this.currentUserEventsService.currentUserUpdated$
      .pipe(
        switchMap(() => {
          return this.init(true);
        })
      )
      .subscribe();
  }

  public init(skipCache: boolean = false): Observable<boolean> {
    if (skipCache || !this.isInitialized.value) {
      const wasAlreadyInitialized = this.isInitialized.value;
      return this.apiCurrentUserClient.getCurrentUserPermissions().pipe(
        catchError((error: ProblemDetails | ApiException) => {
          this.initializationProblemDetails.next(error);
          return throwError(
            () => new Error('An error occured while initializing user permissions')
          );
        }),
        tap((result) => (this.permissions = result.permissions ?? [])),
        tap(() => {
          this.isInitialized.next(true);
          if (wasAlreadyInitialized) {
            this.updated.next();
          }
        }),
        switchMap(() => of(true))
      );
    } else {
      return of(true);
    }
  }

  public hasPermission$(permission: keyof typeof Permission): Observable<boolean> {
    return this.init().pipe(map(() => this.hasPermission(permission)));
  }

  public hasPermission(permission: keyof typeof Permission): boolean {
    const hasPermission = this.permissions?.find((x) => x.name === Permission[permission]) != null;
    return hasPermission;
  }

  public hasAnyPermission$(permissions: Array<keyof typeof Permission>): Observable<boolean> {
    return this.init().pipe(map(() => this.hasAnyPermission(permissions)));
  }

  public hasAnyPermission(permissions: Array<keyof typeof Permission>): boolean {
    const hasAnyPermission = this.permissions?.some((grantedPermission) =>
      permissions.some(
        (requiredPermission) => Permission[requiredPermission] === grantedPermission.name
      )
    );
    return hasAnyPermission;
  }

  public hasAllPermissions$(permissions: Array<keyof typeof Permission>): Observable<boolean> {
    return this.init().pipe(map(() => this.hasAllPermissions(permissions)));
  }

  public hasAllPermissions(permissions: Array<keyof typeof Permission>): boolean {
    const hasAnyPermission = permissions?.every((requiredPermission) =>
      this.permissions.some(
        (grantedPermission) => Permission[requiredPermission] === grantedPermission.name
      )
    );
    return hasAnyPermission;
  }
}
