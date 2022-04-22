import { Injectable } from '@angular/core';
import { Resolve, RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router';
import { ApplicationSettingsService } from '@shared/services/application-settings.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApplicationSettingsResolver implements Resolve<boolean> {
  constructor(private readonly applicationSettingsService: ApplicationSettingsService) {}

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    return this.applicationSettingsService.init();
  }
}
