import { CollectionViewer, DataSource } from '@angular/cdk/collections';
import { map, Observable } from 'rxjs';
import { SearchRoleDto } from '@api/api.generated.clients';
import { RoleTableDataService } from '@roles/pages/role-list/role-table-data.service';

export class RoleTableDataSource implements DataSource<SearchRoleDto> {
  constructor(private roleTableDataService: RoleTableDataService) {}

  connect(collectionViewer: CollectionViewer): Observable<SearchRoleDto[]> {
    this.roleTableDataService.onDatasourceConnect();
    return this.roleTableDataService.observe$.pipe(map((x) => x.items ?? []));
  }

  disconnect(collectionViewer: CollectionViewer): void {}
}
