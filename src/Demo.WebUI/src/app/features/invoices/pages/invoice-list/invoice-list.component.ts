import { ChangeDetectionStrategy, Component, OnDestroy, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { BehaviorSubject, combineLatest, map, Observable, tap } from 'rxjs';
import { InvoiceTableDataSource } from '@invoices/pages/invoice-list/invoice-table-datasource';
import {
  InvoiceTableDataContext,
  InvoiceTableDataService
} from '@invoices/pages/invoice-list/invoice-table-data.service';
import { SearchInvoiceDto } from '@api/api.generated.clients';
import { TableFilterCriteria } from '@shared/base/table-data-base';
import { Router } from '@angular/router';

interface ViewModel extends InvoiceTableDataContext {
  searchInputFocused: boolean;
}

export interface InvoiceListRouteState {
  spotlightIdentifier: string | undefined;
}

@Component({
  templateUrl: './invoice-list.component.html',
  styleUrls: ['./invoice-list.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class InvoiceListComponent implements OnInit, OnDestroy {
  public displayedColumns = ['InvoiceNumber', 'CustomerName', 'InvoiceDate'];
  public dataSource!: InvoiceTableDataSource;
  public searchTerm = this.invoiceTableDataService.searchTerm;

  public readonly searchInputFocused = new BehaviorSubject<boolean>(false);

  public searchInputFocused$ = this.searchInputFocused.asObservable();

  private vm: Readonly<ViewModel> | undefined;

  public vm$: Observable<ViewModel> = combineLatest([
    this.invoiceTableDataService.observe$,
    this.searchInputFocused$
  ]).pipe(
    map(([baseContext, searchInputFocused]) => {
      const context: ViewModel = {
        ...baseContext,
        searchInputFocused
      };
      return context;
    }),
    tap((vm) => (this.vm = vm))
  );

  constructor(
    private readonly location: Location,
    private readonly router: Router,
    private readonly invoiceTableDataService: InvoiceTableDataService
  ) {}

  public ngOnInit(): void {
    this.dataSource = new InvoiceTableDataSource(this.invoiceTableDataService);
    this.spotlight();
  }

  private spotlight(): void {
    const state = this.location.getState() as InvoiceListRouteState;
    if (state && state.spotlightIdentifier) {
      this.invoiceTableDataService.spotlight(state.spotlightIdentifier);
    }
  }

  public search(criteria: TableFilterCriteria): void {
    this.invoiceTableDataService.search(criteria);
  }

  public trackById(index: number, item: SearchInvoiceDto): string {
    return item.id;
  }

  public onSearchInputEnter(): void {
    if (!this.vm?.selectedItem) {
      return;
    }
    this.router.navigate(['/invoices', this.vm.selectedItem.id]);
  }

  public onSearchInputArrowUp(): void {
    this.invoiceTableDataService.selectedItemIndex -= 1;
  }

  public onSearchInputArrowDown(): void {
    this.invoiceTableDataService.selectedItemIndex += 1;
  }

  public ngOnDestroy(): void {
    this.invoiceTableDataService.clearSpotlight();
  }
}
