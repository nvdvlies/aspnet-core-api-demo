import { Injectable } from '@angular/core';
import { Resolve, RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router';
import { UserFeatureFlagService } from '@shared/services/user-feature-flag.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FeatureFlagResolver implements Resolve<boolean> {
  constructor(private readonly userFeatureFlagService: UserFeatureFlagService) {}

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    return this.userFeatureFlagService.init();
  }
}
