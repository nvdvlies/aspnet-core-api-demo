import { Pipe, PipeTransform } from '@angular/core';
import { Observable, of } from 'rxjs';
import { CustomerIdToNamePipe } from './customer-id-to-name.pipe';
import { RoleIdsToNamesPipe } from './roles-ids-to-names.pipe';
import { UserIdsToNamesPipe } from './user-ids-to-names.pipe';

@Pipe({
  name: 'auditlogItemValue'
})
export class AuditlogItemValuePipe implements PipeTransform {
  constructor(
    private readonly customerIdToNamePipe: CustomerIdToNamePipe,
    private readonly userIdsToNamesPipe: UserIdsToNamesPipe,
    private readonly rolesIdsToNamesPipe: RoleIdsToNamesPipe
  ) {}

  transform(
    value: string | undefined,
    entityName: string | undefined,
    propertyName: string | undefined,
    parentPropertyName: string | undefined
  ): Observable<string | undefined> {
    switch (entityName) {
      case 'Invoice':
        if (propertyName === 'CustomerId') {
          return this.customerIdToNamePipe.transform(value);
        }
        break;
      case 'ApplicationSettings':
        if (propertyName === 'Setting4') {
          return this.customerIdToNamePipe.transform(value);
        }
        break;
      case 'UserPreferences':
        if (propertyName === 'Setting4') {
          return this.customerIdToNamePipe.transform(value);
        }
        break;
      case 'FeatureFlagSetting':
        if (propertyName === 'EnabledForUsers') {
          return this.userIdsToNamesPipe.transform(value);
        }
        break;
      case 'User':
        if (propertyName === 'UserRoles') {
          return this.rolesIdsToNamesPipe.transform(value);
        }
        break;
    }
    return of(value);
  }
}
