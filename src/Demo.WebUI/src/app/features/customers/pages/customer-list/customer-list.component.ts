import {
  ChangeDetectionStrategy,
  Component,
  ElementRef,
  HostListener,
  OnDestroy,
  OnInit,
  QueryList,
  ViewChild,
  ViewChildren,
  ViewContainerRef
} from '@angular/core';
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
import { MatPaginator } from '@angular/material/paginator';

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
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator | undefined;
  @ViewChildren('tableRows', { read: ElementRef }) tableRows: QueryList<ElementRef> | undefined;

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

  public navigateToDetailsById(id: string): void {
    this.router.navigate(['/customers', id]);
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
    this.customerTableDataService.selectedItemIndex -= 1;
    this.tableRows
      ?.get(this.customerTableDataService.selectedItemIndex)
      ?.nativeElement.scrollIntoView(false, { behavior: 'auto', block: 'end' });
    event.preventDefault();
  }

  @HostListener('document:keydown.shift.alt.ArrowDown', ['$event'])
  public navigateTableDown(event: Event): void {
    this.customerTableDataService.selectedItemIndex += 1;
    this.tableRows
      ?.get(this.customerTableDataService.selectedItemIndex)
      ?.nativeElement.scrollIntoView(false, { behavior: 'auto', block: 'end' });
    event.preventDefault();
  }

  @HostListener('document:keydown.shift.alt.n', ['$event'])
  public newCustomerShortcut(event: KeyboardEvent) {
    this.router.navigateByUrl('/customers/new');
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
    this.customerTableDataService.clearSpotlight();
  }
}
