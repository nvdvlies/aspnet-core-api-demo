import { Pipe, PipeTransform } from '@angular/core';
import { UserLookupService } from '@shared/services/user-lookup.service';
import { map, Observable, of } from 'rxjs';

@Pipe({
  name: 'userIdsToNames'
})
export class UserIdsToNamesPipe implements PipeTransform {
  constructor(private readonly userLookupService: UserLookupService) {}

  transform(ids: string[] | string | undefined): Observable<string | undefined> {
    if (!ids) {
      return of(undefined);
    }

    if (!Array.isArray(ids)) {
      ids = ids.split(',');
    }

    return ids && ids.length > 0
      ? this.userLookupService.getByIds(ids).pipe(
          map((items) => items.map((item) => item.fullname)),
          map((x) => x?.join(', '))
        )
      : of(undefined);
  }
}
