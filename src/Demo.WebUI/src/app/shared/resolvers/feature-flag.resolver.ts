import { Injectable } from '@angular/core';
import { Resolve, RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router';
import { FeatureFlagService } from '@shared/services/feature-flag.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FeatureFlagResolver implements Resolve<boolean> {
  constructor(private readonly featureFlagService: FeatureFlagService) {}

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    return this.featureFlagService.init();
  }
}
