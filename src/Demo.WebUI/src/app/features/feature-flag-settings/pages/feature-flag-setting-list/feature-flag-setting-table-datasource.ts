import { CollectionViewer, DataSource } from '@angular/cdk/collections';
import { map, Observable } from 'rxjs';
import { FeatureFlagSettingTableDataService } from '@feature-flag-settings/pages/feature-flag-setting-list/feature-flag-setting-table-data.service';
import { FeatureFlagDto } from '@api/api.generated.clients';

export class FeatureFlagSettingTableDataSource implements DataSource<FeatureFlagDto> {
  constructor(private featureFlagSettingTableDataService: FeatureFlagSettingTableDataService) {}

  connect(collectionViewer: CollectionViewer): Observable<FeatureFlagDto[]> {
    this.featureFlagSettingTableDataService.onDatasourceConnect();
    return this.featureFlagSettingTableDataService.observe$.pipe(map((x) => x.items ?? []));
  }

  disconnect(collectionViewer: CollectionViewer): void {}
}
