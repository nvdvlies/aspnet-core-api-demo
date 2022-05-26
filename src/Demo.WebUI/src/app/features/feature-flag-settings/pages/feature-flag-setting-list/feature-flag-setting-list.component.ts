import { ChangeDetectionStrategy, Component, OnDestroy, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { BehaviorSubject, combineLatest, map, Observable, tap } from 'rxjs';
import { FeatureFlagSettingTableDataSource } from '@feature-flag-settings/pages/feature-flag-setting-list/feature-flag-setting-table-datasource';
import {
  FeatureFlagSettingTableDataContext,
  FeatureFlagSettingTableDataService
} from '@feature-flag-settings/pages/feature-flag-setting-list/feature-flag-setting-table-data.service';
import { TableFilterCriteria } from '@shared/base/table-data-base';
import { Router } from '@angular/router';
import { FeatureFlagDto } from '@api/api.generated.clients';

interface ViewModel extends FeatureFlagSettingTableDataContext {
  searchInputFocused: boolean;
}

export interface FeatureFlagSettingListRouteState {
  spotlightIdentifier: string | undefined;
}

@Component({
  templateUrl: './feature-flag-setting-list.component.html',
  styleUrls: ['./feature-flag-setting-list.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class FeatureFlagSettingListComponent implements OnInit, OnDestroy {
  public displayedColumns = ['name', 'description', 'enabledForAll', 'enabledForUsers'];
  public dataSource!: FeatureFlagSettingTableDataSource;
  public searchTerm = this.featureFlagSettingTableDataService.searchTerm;

  public readonly searchInputFocused = new BehaviorSubject<boolean>(false);

  public searchInputFocused$ = this.searchInputFocused.asObservable();

  private vm: Readonly<ViewModel> | undefined;

  public vm$: Observable<ViewModel> = combineLatest([
    this.featureFlagSettingTableDataService.observe$,
    this.searchInputFocused$
  ]).pipe(
    map(([context, searchInputFocused]) => {
      const vm: ViewModel = {
        ...context,
        searchInputFocused
      };
      return vm;
    }),
    tap((vm) => (this.vm = vm))
  );

  constructor(
    private readonly location: Location,
    private readonly router: Router,
    private readonly featureFlagSettingTableDataService: FeatureFlagSettingTableDataService
  ) {}

  public ngOnInit(): void {
    this.dataSource = new FeatureFlagSettingTableDataSource(
      this.featureFlagSettingTableDataService
    );

    const state = this.location.getState() as FeatureFlagSettingListRouteState;
    if (state && state.spotlightIdentifier) {
      this.featureFlagSettingTableDataService.spotlight(state.spotlightIdentifier);
    }

    this.featureFlagSettingTableDataService.search();
  }

  public search(criteria: TableFilterCriteria): void {
    this.featureFlagSettingTableDataService.search(criteria);
  }

  public trackById(index: number, item: FeatureFlagDto): string {
    return item.name!;
  }

  public onSearchInputEnter(): void {
    if (!this.vm?.selectedItem) {
      return;
    }
    this.router.navigate(['/feature-flag-settings', this.vm.selectedItem.name]);
  }

  public onSearchInputArrowUp(): void {
    this.featureFlagSettingTableDataService.selectedItemIndex -= 1;
  }

  public onSearchInputArrowDown(): void {
    this.featureFlagSettingTableDataService.selectedItemIndex += 1;
  }

  public ngOnDestroy(): void {
    this.featureFlagSettingTableDataService.clearSpotlight();
  }
}
