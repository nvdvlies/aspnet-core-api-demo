import { Injectable } from '@angular/core';
import { Resolve, RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router';
import { PermissionGroupService } from '@shared/services/permission-group.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PermissionGroupsResolver implements Resolve<boolean> {
  constructor(private readonly permissionGroupService: PermissionGroupService) {}

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    return this.permissionGroupService.init();
  }
}
