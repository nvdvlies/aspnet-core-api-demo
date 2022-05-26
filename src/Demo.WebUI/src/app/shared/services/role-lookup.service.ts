import { Injectable } from '@angular/core';
import { ApiRolesClient, RoleLookupDto, RoleLookupOrderByEnum } from '@api/api.generated.clients';
import { RoleEventsService } from '@api/signalr.generated.services';
import { LookupBase } from '@shared/base/lookup-base';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RoleLookupService extends LookupBase<RoleLookupDto> {
  protected itemUpdatedEvent$ = this.roleEventsService.roleUpdated$.pipe(map((x) => x.id));

  constructor(
    private readonly apiRolesClient: ApiRolesClient,
    private readonly roleEventsService: RoleEventsService
  ) {
    super();
  }

  protected lookupFunction = (ids: string[]) => {
    return this.apiRolesClient
      .lookup(RoleLookupOrderByEnum.Name, false, 0, 999, undefined, ids)
      .pipe(map((response) => response.roles ?? []));
  };
}
