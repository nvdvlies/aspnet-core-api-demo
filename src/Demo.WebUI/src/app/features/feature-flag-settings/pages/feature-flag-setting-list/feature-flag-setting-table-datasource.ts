import { CollectionViewer, DataSource } from '@angular/cdk/collections';
import { map, Observable } from 'rxjs';
import {
  FeatureFlagSettingTableDataService,
  IFeatureFlagSettingsTableRow
} from '@feature-flag-settings/pages/feature-flag-setting-list/feature-flag-setting-table-data.service';

export class FeatureFlagSettingTableDataSource implements DataSource<IFeatureFlagSettingsTableRow> {
  constructor(private featureFlagSettingTableDataService: FeatureFlagSettingTableDataService) {}

  connect(collectionViewer: CollectionViewer): Observable<IFeatureFlagSettingsTableRow[]> {
    this.featureFlagSettingTableDataService.onDatasourceConnect();
    return this.featureFlagSettingTableDataService.observe$.pipe(map((x) => x.items ?? []));
  }

  disconnect(collectionViewer: CollectionViewer): void {}
}
