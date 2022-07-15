import { Pipe, PipeTransform } from '@angular/core';
import { PermissionService } from '@shared/services/permission.service';

@Pipe({
  name: 'permissionIdsToNames'
})
export class PermissionIdsToNamesPipe implements PipeTransform {
  constructor(private readonly permissionService: PermissionService) {}

  transform(ids: string[] | string | undefined): string | undefined {
    if (!ids) {
      return undefined;
    }

    if (!Array.isArray(ids)) {
      ids = ids.split(',');
    }

    return ids && ids.length > 0
      ? this.permissionService.permissions
          .filter((x) => ids?.includes(x.id))
          .map((item) => item.name)
          .join(', ')
      : undefined;
  }
}
