import { ChangeDetectionStrategy, Component, HostListener, OnDestroy, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { BehaviorSubject, combineLatest, map, Observable, tap } from 'rxjs';
import { CustomerTableDataSource } from '@customers/pages/customer-list/customer-table-datasource';
import {
  CustomerTableDataContext,
  CustomerTableDataService
} from '@customers/pages/customer-list/customer-table-data.service';
import { SearchCustomerDto } from '@api/api.generated.clients';
import { TableFilterCriteria } from '@shared/base/table-data-base';
import { Router } from '@angular/router';

interface ViewModel extends CustomerTableDataContext {
  searchInputFocused: boolean;
}

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

  public readonly searchInputFocused = new BehaviorSubject<boolean>(false);

  public searchInputFocused$ = this.searchInputFocused.asObservable();

  private vm: Readonly<ViewModel> | undefined;

  public vm$: Observable<ViewModel> = combineLatest([
    this.customerTableDataService.observe$,
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

  public onSearchInputEnter(): void {
    if (!this.vm?.selectedItem) {
      return;
    }
    this.router.navigate(['/customers', this.vm.selectedItem.id]);
  }

  public onSearchInputArrowUp(): void {
    this.customerTableDataService.selectedItemIndex -= 1;
  }

  public onSearchInputArrowDown(): void {
    this.customerTableDataService.selectedItemIndex += 1;
  }

  @HostListener('document:keydown.shift.alt.n', ['$event'])
  public newCustomerShortcut(event: KeyboardEvent) {
    this.router.navigateByUrl('/customers/new');
    event.preventDefault();
  }

  public ngOnDestroy(): void {
    this.customerTableDataService.clearSpotlight();
  }
}
