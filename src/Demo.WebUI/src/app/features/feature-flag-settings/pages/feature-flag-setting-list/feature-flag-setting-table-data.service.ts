import { Injectable } from '@angular/core';
import { FormControl } from '@angular/forms';
import { combineLatest, debounceTime, map, Observable, of } from 'rxjs';
import { FeatureFlagDto } from '@api/api.generated.clients';
import { FeatureFlagSettingsStoreService } from '@domain/feature-flag-settings/feature-flag-settings-store.service';
import {
  ITableFilterCriteria,
  TableDataBase,
  TableDataContext,
  TableDataSearchResult,
  TableFilterCriteria
} from '@shared/base/table-data-base';
import {
  FeatureFlagSettingListPageSettings,
  FeatureFlagSettingListPageSettingsService
} from './feature-flag-setting-list-page-settings.service';
import { FeatureFlag } from '@shared/enums/feature-flag.enum';

export declare type FeatureFlagSettingSortColumn = 'name' | undefined;

export class FeatureFlagSettingTableFilterCriteria
  extends TableFilterCriteria
  implements ITableFilterCriteria
{
  override sortColumn: FeatureFlagSettingSortColumn;

  constructor(pageSettings?: FeatureFlagSettingListPageSettings) {
    super(pageSettings?.pageSize);
    this.sortColumn = pageSettings?.sortColumn ?? 'name';
    this.sortDirection = pageSettings?.sortDirection ?? 'asc';
  }
}
export interface FeatureFlagSettingTableDataContext extends TableDataContext<FeatureFlagDto> {
  criteria: FeatureFlagSettingTableFilterCriteria | undefined;
}

@Injectable()
export class FeatureFlagSettingTableDataService extends TableDataBase<FeatureFlagDto> {
  public searchTerm = new FormControl();

  public observe$: Observable<FeatureFlagSettingTableDataContext> = combineLatest([
    this.observeInternal$
  ]).pipe(
    debounceTime(0),
    map(([baseContext]) => {
      const context: FeatureFlagSettingTableDataContext = {
        ...baseContext,
        criteria: baseContext.criteria as FeatureFlagSettingTableFilterCriteria
      };
      return context;
    })
  );

  protected entityUpdatedInStore$ =
    this.featureFlagSettingsStoreService.featureFlagSettingsUpdatedInStore$;
  protected entityDeletedEvent$ = undefined;

  constructor(
    private readonly featureFlagSettingsStoreService: FeatureFlagSettingsStoreService,
    private readonly featureFlagSettingListPageSettingsService: FeatureFlagSettingListPageSettingsService
  ) {
    super();
    const pageSettings = this.featureFlagSettingListPageSettingsService.settings;
    this.init(new FeatureFlagSettingTableFilterCriteria(pageSettings));
    this.search();
  }

  protected searchFunction = (criteria: FeatureFlagSettingTableFilterCriteria) => {
    this.featureFlagSettingListPageSettingsService.update({
      pageSize: criteria.pageSize,
      sortColumn: criteria.sortColumn,
      sortDirection: criteria.sortDirection
    });

    let items = Object.entries(FeatureFlag).map(([key, value]) => {
      const featureFlagDto = new FeatureFlagDto();
      featureFlagDto.name = key;
      return featureFlagDto;
    });

    if (this.searchTerm.value && this.searchTerm.value.length > 0) {
      items = items.filter((x) => x.name?.indexOf(this.searchTerm.value) != -1);
    }

    if (criteria.sortColumn) {
      if (criteria.sortDirection === 'asc') {
        items = items.sort((a, b) =>
          a[criteria.sortColumn!]!.toLowerCase() < b[criteria.sortColumn!]!.toLowerCase() ? 1 : -1
        );
      } else if (criteria.sortDirection === 'desc') {
        items = items.sort((a, b) =>
          a[criteria.sortColumn!]!.toLowerCase() > b[criteria.sortColumn!]!.toLowerCase() ? 1 : -1
        );
      }
    }

    const result: TableDataSearchResult<FeatureFlagDto> = {
      items: items.slice(
        criteria.pageIndex * criteria.pageSize,
        criteria.pageIndex * criteria.pageSize + criteria.pageSize
      ),
      pageIndex: criteria.pageIndex,
      pageSize: criteria.pageSize,
      totalItems: items.length,
      totalPages: criteria.pageSize === 0 ? 0 : Math.ceil(items.length / criteria.pageSize),
      hasPreviousPage: criteria.pageIndex > 0,
      hasNextPage: criteria.pageIndex + 1 < Math.ceil(items.length / criteria.pageSize)
    };

    return of(result);
  };

  protected getByIdFunction = undefined;
}
