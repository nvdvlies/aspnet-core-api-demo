import { Pipe, PipeTransform } from '@angular/core';
import { RoleLookupService } from '@shared/services/role-lookup.service';
import { map, Observable, of } from 'rxjs';

@Pipe({
  name: 'roleIdToName'
})
export class RoleIdToNamePipe implements PipeTransform {
  constructor(private readonly roleLookupService: RoleLookupService) {}

  transform(id: string | undefined): Observable<string | undefined> {
    return id && id.length > 0
      ? this.roleLookupService.getById(id).pipe(map((x) => x?.name))
      : of(undefined);
  }
}
