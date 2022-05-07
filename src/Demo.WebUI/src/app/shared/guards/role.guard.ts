import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { CurrentUserService } from '@shared/services/current-user.service';
import { Observable } from 'rxjs';
import { RouteData } from 'src/app/app-routing.module';

@Injectable({
  providedIn: 'root'
})
export class RoleGuard implements CanActivate {
  constructor(private readonly currentUserService: CurrentUserService) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    const roleNames = (route.data as RouteData).roleNames;
    if (roleNames) {
      return this.currentUserService.hasAnyRole(roleNames);
    } else {
      console.warn(
        `Route '${state.url}' has a 'RoleGuard' but is missing a required 'roleName' property in the route data.`
      );
      return false;
    }
  }
}
