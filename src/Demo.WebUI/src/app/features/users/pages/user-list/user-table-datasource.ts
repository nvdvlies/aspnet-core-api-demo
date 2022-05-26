import { CollectionViewer, DataSource } from '@angular/cdk/collections';
import { map, Observable } from 'rxjs';
import { SearchUserDto } from '@api/api.generated.clients';
import { UserTableDataService } from '@users/pages/user-list/user-table-data.service';

export class UserTableDataSource implements DataSource<SearchUserDto> {
  constructor(private userTableDataService: UserTableDataService) {}

  connect(collectionViewer: CollectionViewer): Observable<SearchUserDto[]> {
    this.userTableDataService.onDatasourceConnect();
    return this.userTableDataService.observe$.pipe(map((x) => x.items ?? []));
  }

  disconnect(collectionViewer: CollectionViewer): void {}
}
