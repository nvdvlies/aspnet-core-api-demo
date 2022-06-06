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
import { InvoiceTableDataSource } from '@invoices/pages/invoice-list/invoice-table-datasource';
import {
  InvoiceTableDataContext,
  InvoiceTableDataService
} from '@invoices/pages/invoice-list/invoice-table-data.service';
import { InvoiceStatusEnum, SearchInvoiceDto } from '@api/api.generated.clients';
import { TableFilterCriteria } from '@shared/base/table-data-base';
import { Router } from '@angular/router';
import { MatPaginator } from '@angular/material/paginator';

interface ViewModel extends InvoiceTableDataContext {}

class InvoiceStatusSelectOption {
  public value: number | undefined;
  public label: string;

  constructor(value: number | undefined, label: string) {
    this.value = value;
    this.label = label;
  }
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
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator | undefined;
  @ViewChildren('tableRows', { read: ElementRef }) tableRows: QueryList<ElementRef> | undefined;

  public displayedColumns = ['InvoiceNumber', 'CustomerName', 'InvoiceDate'];
  public dataSource!: InvoiceTableDataSource;
  public searchTerm = this.invoiceTableDataService.searchTerm;
  public invoiceStatus = this.invoiceTableDataService.invoiceStatus;

  private vm: Readonly<ViewModel> | undefined;

  public invoiceStatuses: InvoiceStatusSelectOption[] = [
    new InvoiceStatusSelectOption(undefined, 'All'),
    new InvoiceStatusSelectOption(InvoiceStatusEnum.Draft, 'Draft'),
    new InvoiceStatusSelectOption(InvoiceStatusEnum.Sent, 'Sent'),
    new InvoiceStatusSelectOption(InvoiceStatusEnum.Paid, 'Paid'),
    new InvoiceStatusSelectOption(InvoiceStatusEnum.Cancelled, 'Cancelled')
  ];

  public vm$: Observable<ViewModel> = combineLatest([this.invoiceTableDataService.observe$]).pipe(
    map(([baseContext]) => {
      const context: ViewModel = {
        ...baseContext
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

  public navigateToDetailsById(id: string): void {
    this.router.navigate(['/invoices', id]);
  }

  @HostListener('document:keydown.shift.alt.enter', ['$event'])
  public navigateToDetails(event: Event): void {
    if (!this.vm?.selectedItem) {
      return;
    }
    this.navigateToDetailsById(this.vm.selectedItem.id);
    event.preventDefault();
  }

  @HostListener('document:keydown.shift.alt.ArrowUp', ['$event'])
  public navigateTableUp(event: Event): void {
    this.invoiceTableDataService.selectedItemIndex -= 1;
    this.tableRows
      ?.get(this.invoiceTableDataService.selectedItemIndex)
      ?.nativeElement.scrollIntoView(false, { behavior: 'auto', block: 'end' });
    event.preventDefault();
  }

  @HostListener('document:keydown.shift.alt.ArrowDown', ['$event'])
  public navigateTableDown(event: Event): void {
    this.invoiceTableDataService.selectedItemIndex += 1;
    this.tableRows
      ?.get(this.invoiceTableDataService.selectedItemIndex)
      ?.nativeElement.scrollIntoView(false, { behavior: 'auto', block: 'end' });
    event.preventDefault();
  }

  @HostListener('document:keydown.shift.alt.n', ['$event'])
  public newInvoiceShortcut(event: KeyboardEvent) {
    this.router.navigateByUrl('/invoices/new');
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
    this.invoiceTableDataService.clearSpotlight();
  }
}
