import { Injectable } from '@angular/core';
import { ApiUsersClient, UserLookupDto, UserLookupOrderByEnum } from '@api/api.generated.clients';
import { UserEventsService } from '@api/signalr.generated.services';
import { LookupBase } from '@shared/base/lookup.base';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserLookupService extends LookupBase<UserLookupDto> {
  protected entityUpdatedEvent$ = this.userEventsService.userUpdated$.pipe(map((x) => x.id));

  constructor(
    private readonly apiUsersClient: ApiUsersClient,
    private readonly userEventsService: UserEventsService
  ) {
    super();
  }

  protected getByIdFunction = (id: string) => {
    return this.apiUsersClient
      .userLookup(UserLookupOrderByEnum.FamilyName, false, 0, 1, undefined, [id])
      .pipe(map((response) => response.users?.[0]));
  };

  protected getBatchByIdFunction = (ids: string[]) => {
    return this.apiUsersClient
      .userLookup(UserLookupOrderByEnum.FamilyName, false, 0, 1, undefined, ids)
      .pipe(map((response) => response.users ?? []));
  };
}
