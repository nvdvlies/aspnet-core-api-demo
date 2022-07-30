import { Pipe, PipeTransform } from '@angular/core';
import { LocationLookupService } from '@shared/services/location-lookup.service';
import { map, Observable, of } from 'rxjs';

export interface LocationIdToNamePipeOptions {}

@Pipe({
  name: 'locationIdToName'
})
export class LocationIdToNamePipe implements PipeTransform {
  constructor(private readonly locationLookupService: LocationLookupService) {}

  transform(
    id: string | undefined,
    options?: LocationIdToNamePipeOptions
  ): Observable<string | undefined> {
    return id && id.length > 0
      ? this.locationLookupService.getById(id).pipe(map((x) => x?.displayName))
      : of(undefined);
  }
}
