import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { FeatureFlagService } from '@shared/services/feature-flag.service';
import { Observable } from 'rxjs';
import { RouteData } from 'src/app/app-routing.module';

@Injectable({
  providedIn: 'root'
})
export class FeatureFlagGuard implements CanActivate {
  constructor(private readonly featureFlagService: FeatureFlagService) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    const featureFlag = (route.data as RouteData).featureFlag;
    if (featureFlag) {
      return this.featureFlagService.isEnabled(featureFlag);
    } else {
      console.warn(
        `Route '${state.url}' has a 'FeatureFlagGuard' but is missing a required 'featureFlag' property in the route data.`
      );
      return false;
    }
  }
}
