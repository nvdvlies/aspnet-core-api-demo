import {
  ChangeDetectionStrategy,
  Component,
  ElementRef,
  HostListener,
  OnDestroy,
  OnInit,
  QueryList,
  ViewChild,
  ViewChildren
} from '@angular/core';
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
import { MatPaginator } from '@angular/material/paginator';

interface ViewModel extends FeatureFlagSettingTableDataContext {}

export interface FeatureFlagSettingListRouteState {
  spotlightIdentifier: string | undefined;
}

@Component({
  templateUrl: './feature-flag-setting-list.component.html',
  styleUrls: ['./feature-flag-setting-list.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class FeatureFlagSettingListComponent implements OnInit, OnDestroy {
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator | undefined;
  @ViewChildren('tableRows', { read: ElementRef }) tableRows: QueryList<ElementRef> | undefined;

  public displayedColumns = ['name', 'description', 'enabledForAll', 'enabledForUsers'];
  public dataSource!: FeatureFlagSettingTableDataSource;
  public searchTerm = this.featureFlagSettingTableDataService.searchTerm;

  private vm: Readonly<ViewModel> | undefined;

  public vm$: Observable<ViewModel> = combineLatest([
    this.featureFlagSettingTableDataService.observe$
  ]).pipe(
    map(([context]) => {
      const vm: ViewModel = {
        ...context
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

  public navigateToDetailsByName(name: string): void {
    this.router.navigate(['/feature-flag-settings', name]);
  }

  @HostListener('document:keydown.shift.alt.enter', ['$event'])
  public navigateToDetails(event: Event): void {
    if (!this.vm?.selectedItem) {
      return;
    }
    this.navigateToDetailsByName(this.vm.selectedItem.name!);
    event.preventDefault();
  }

  @HostListener('document:keydown.shift.alt.ArrowUp', ['$event'])
  public navigateTableUp(event: Event): void {
    this.featureFlagSettingTableDataService.selectedItemIndex -= 1;
    this.tableRows
      ?.get(this.featureFlagSettingTableDataService.selectedItemIndex)
      ?.nativeElement.scrollIntoView(false, { behavior: 'auto', block: 'end' });
    event.preventDefault();
  }

  @HostListener('document:keydown.shift.alt.ArrowDown', ['$event'])
  public navigateTableDown(event: Event): void {
    this.featureFlagSettingTableDataService.selectedItemIndex += 1;
    this.tableRows
      ?.get(this.featureFlagSettingTableDataService.selectedItemIndex)
      ?.nativeElement.scrollIntoView(false, { behavior: 'auto', block: 'end' });
    event.preventDefault();
  }

  @HostListener('document:keydown.shift.alt.ArrowRight', ['$event'])
  public navigateToNextPage(event: KeyboardEvent) {
    if (this.paginator?.hasNextPage) {
      this.paginator?.nextPage();
    }
    event.preventDefault();
  }

  @HostListener('document:keydown.shift.alt.ArrowLeft', ['$event'])
  public navigateToPreviousPage(event: KeyboardEvent) {
    if (this.paginator?.hasPreviousPage) {
      this.paginator?.previousPage();
    }
    event.preventDefault();
  }

  public ngOnDestroy(): void {
    this.featureFlagSettingTableDataService.clearSpotlight();
  }
}
