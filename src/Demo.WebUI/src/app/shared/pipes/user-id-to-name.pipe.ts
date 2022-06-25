import { Pipe, PipeTransform } from '@angular/core';
import { UserLookupService } from '@shared/services/user-lookup.service';
import { map, Observable, of } from 'rxjs';

@Pipe({
  name: 'userIdToName'
})
export class UserIdToNamePipe implements PipeTransform {
  constructor(private readonly userLookupService: UserLookupService) {}

  transform(id: string | undefined): Observable<string | undefined> {
    return id && id.length > 0
      ? this.userLookupService.getById(id).pipe(map((x) => x?.fullname))
      : of(undefined);
  }
}
