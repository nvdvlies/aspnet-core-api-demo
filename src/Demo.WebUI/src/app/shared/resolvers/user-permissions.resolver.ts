import { Injectable } from '@angular/core';
import { Resolve, RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router';
import { UserPermissionService } from '@shared/services/user-permission.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserPermissionsResolver implements Resolve<boolean> {
  constructor(private readonly userPermissionService: UserPermissionService) {}

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    return this.userPermissionService.init();
  }
}
