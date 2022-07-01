import { Pipe, PipeTransform } from '@angular/core';
import { RoleLookupService } from '@shared/services/role-lookup.service';
import { map, Observable, of } from 'rxjs';

@Pipe({
  name: 'roleIdsToNames'
})
export class RoleIdsToNamesPipe implements PipeTransform {
  constructor(private readonly roleLookupService: RoleLookupService) {}

  transform(ids: string[] | string | undefined): Observable<string | undefined> {
    if (!ids) {
      return of(undefined);
    }

    if (!Array.isArray(ids)) {
      ids = ids.split(',');
    }

    return ids && ids.length > 0
      ? this.roleLookupService.getByIds(ids).pipe(
          map((items) => items.map((item) => item.name)),
          map((x) => x?.join(', '))
        )
      : of(undefined);
  }
}
