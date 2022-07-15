import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { UserPermissionService } from '@shared/services/user-permission.service';
import { Observable } from 'rxjs';
import { RouteData } from 'src/app/app-routing.module';

@Injectable({
  providedIn: 'root'
})
export class PermissionGuard implements CanActivate {
  constructor(private readonly userPermissionService: UserPermissionService) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    const permission = (route.data as RouteData).permission;
    if (permission) {
      return this.userPermissionService.hasPermission$(permission);
    } else {
      console.warn(
        `Route '${state.url}' has a 'PermissionGuard' but is missing a required 'permission' property in the route data.`
      );
      return false;
    }
  }
}
