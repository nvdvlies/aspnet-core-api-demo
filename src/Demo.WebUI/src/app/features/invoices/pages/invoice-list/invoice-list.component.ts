import { ChangeDetectionStrategy, Component, OnDestroy, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { combineLatest, map, Observable } from 'rxjs';
import { InvoiceTableDataSource } from '@invoices/pages/invoice-list/invoice-table-datasource';
import {
  InvoiceTableDataContext,
  InvoiceTableDataService
} from '@invoices/pages/invoice-list/invoice-table-data.service';
import { TableFilterCriteria } from '@shared/directives/table-filter/table-filter-criteria';
import { SearchInvoiceDto } from '@api/api.generated.clients';

interface ViewModel extends InvoiceTableDataContext {}

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

  public vm$ = combineLatest([this.invoiceTableDataService.observe$]).pipe(
    map(([context]) => {
      return {
        ...context
      } as ViewModel;
    })
  ) as Observable<ViewModel>;

  constructor(
    private readonly location: Location,
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

  public ngOnDestroy(): void {
    this.invoiceTableDataService.clearSpotlight();
  }
}
