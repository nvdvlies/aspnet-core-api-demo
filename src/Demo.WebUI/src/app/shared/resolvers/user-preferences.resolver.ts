import { Injectable } from '@angular/core';
import { Resolve, RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router';
import { UserPreferencesService } from '@shared/services/user-preferences.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserPreferencesResolver implements Resolve<boolean> {
  constructor(private readonly userPreferencesService: UserPreferencesService) {}

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    return this.userPreferencesService.init();
  }
}
