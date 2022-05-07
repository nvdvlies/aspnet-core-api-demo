import { Injectable } from '@angular/core';
import { Resolve, RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router';
import { RoleService } from '@shared/services/role.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RolesResolver implements Resolve<boolean> {
  constructor(private readonly roleService: RoleService) {}

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    return this.roleService.init();
  }
}
