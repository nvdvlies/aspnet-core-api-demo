import { Injectable } from '@angular/core';
import {
  ApiLocationsClient,
  LocationLookupDto,
  LocationLookupOrderByEnum
} from '@api/api.generated.clients';
import { LocationEventsService } from '@api/signalr.generated.services';
import { LookupBase } from '@shared/base/lookup-base';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LocationLookupService extends LookupBase<LocationLookupDto> {
  protected itemUpdatedEvent$ = this.locationEventsService.locationUpdated$.pipe(map((x) => x.id));

  constructor(
    private readonly apiLocationsClient: ApiLocationsClient,
    private readonly locationEventsService: LocationEventsService
  ) {
    super();
  }

  protected lookupFunction = (ids: string[]) => {
    return this.apiLocationsClient
      .lookup(LocationLookupOrderByEnum.DisplayName, false, 0, 999, ids)
      .pipe(map((response) => response.locations ?? []));
  };
}
