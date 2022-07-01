import { Injectable } from '@angular/core';
import { ApiException, CurrentUserDto, ProblemDetails } from '@api/api.generated.clients';
import { AuthService } from '@auth0/auth0-angular';
import { CurrentUserStoreService } from '@domain/current-user/current-user-store.service';
import { Role } from '@shared/enums/role.enum';
import { BehaviorSubject, catchError, Observable, of, switchMap, tap, throwError } from 'rxjs';
import { RoleService } from './role.service';

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
    private readonly roleService: RoleService,
    private readonly authService: AuthService
  ) {
    this.authService.isAuthenticated$.subscribe((isAuthenticated) => {
      if (!isAuthenticated) {
        this.currentUser = undefined;
        this.isInitialized.next(false);
      }
    });
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

  public hasRole(roleName: keyof typeof Role): boolean {
    return this.roleService.roles.some(
      (role) =>
        role.name === roleName &&
        this.currentUser?.userRoles?.some((userRole) => userRole.roleId === role.id)
    );
  }

  public hasAnyRole(roleNames: Array<keyof typeof Role>): boolean {
    return this.roleService.roles.some(
      (role) =>
        roleNames.some((roleName) => roleName === role.name) &&
        this.currentUser?.userRoles?.some((userRole) => userRole.roleId === role.id)
    );
  }

  public hasAllRoles(roleNames: Array<keyof typeof Role>): boolean {
    return roleNames.every((roleName) =>
      this.roleService.roles.some(
        (role) =>
          role.name === roleName &&
          this.currentUser?.userRoles?.some((userRole) => userRole.roleId === role.id)
      )
    );
  }
}
