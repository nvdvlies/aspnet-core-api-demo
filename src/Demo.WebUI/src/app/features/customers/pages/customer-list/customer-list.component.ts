import { ChangeDetectionStrategy, Component, OnDestroy, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { combineLatest, map, Observable, tap } from 'rxjs';
import { CustomerTableDataSource } from '@customers/pages/customer-list/customer-table-datasource';
import {
  CustomerTableDataContext,
  CustomerTableDataService
} from '@customers/pages/customer-list/customer-table-data.service';
import { SearchCustomerDto } from '@api/api.generated.clients';
import { TableFilterCriteria } from '@shared/base/table-data-base';

interface ViewModel extends CustomerTableDataContext {}

export interface CustomerListRouteState {
  spotlightIdentifier: string | undefined;
}

@Component({
  templateUrl: './customer-list.component.html',
  styleUrls: ['./customer-list.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CustomerListComponent implements OnInit, OnDestroy {
  public displayedColumns = ['code', 'name'];
  public dataSource!: CustomerTableDataSource;
  public searchTerm = this.customerTableDataService.searchTerm;

  private vm: Readonly<ViewModel> | undefined;

  public vm$: Observable<ViewModel> = combineLatest([this.customerTableDataService.observe$]).pipe(
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
    private readonly customerTableDataService: CustomerTableDataService
  ) {}

  public ngOnInit(): void {
    this.dataSource = new CustomerTableDataSource(this.customerTableDataService);

    const state = this.location.getState() as CustomerListRouteState;
    if (state && state.spotlightIdentifier) {
      this.customerTableDataService.spotlight(state.spotlightIdentifier);
    }
  }

  public search(criteria: TableFilterCriteria): void {
    this.customerTableDataService.search(criteria);
  }

  public trackById(index: number, item: SearchCustomerDto): string {
    return item.id;
  }

  public ngOnDestroy(): void {
    this.customerTableDataService.clearSpotlight();
  }
}
