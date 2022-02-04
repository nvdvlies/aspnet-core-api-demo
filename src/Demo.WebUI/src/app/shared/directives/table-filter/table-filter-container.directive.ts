import { AfterContentInit, ContentChild, ContentChildren, Directive, EventEmitter, Input, Output, QueryList } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { TableFilterCriteria } from '@shared/directives/table-filter/table-filter-criteria';
import { TableFilterDirective } from '@shared/directives/table-filter/table-filter.directive';
import { Subscription } from 'rxjs';

@Directive({
  selector: '[tableFilterContainer]'
})
export class TableFilterContainerDirective implements AfterContentInit {
  @ContentChildren(TableFilterDirective, { descendants: true }) filters!: QueryList<TableFilterDirective>;
  @ContentChild(MatPaginator) paginator: MatPaginator | undefined;
  @ContentChild(MatSort) sort: MatSort | undefined;
  
  @Input() filterCriteria: TableFilterCriteria | undefined;
  @Output() filterChange = new EventEmitter<TableFilterCriteria>();

  private subscriptions: Subscription[] = [];

  constructor() { }

  public ngAfterContentInit(): void {
    this.subscribeToPaginator();
    this.subscribeToSort();
    this.subscribeToFilterControls();
  }

  private subscribeToPaginator(): void {
    const subscription = this.paginator?.page?.subscribe((pageEvent: PageEvent) => {
      this.filterCriteria ??= new TableFilterCriteria();
      this.filterCriteria.pageIndex = pageEvent.pageIndex;
      this.filterCriteria.pageSize = pageEvent.pageSize;
      this.filterChange.emit(this.filterCriteria);
    });
    if (subscription) {
      this.subscriptions.push(subscription);
    }
  }

  private subscribeToSort(): void {
    const subscription = this.sort?.sortChange.subscribe((sort) => {
      this.filterCriteria ??= new TableFilterCriteria();
      this.filterCriteria.pageIndex = 0;
      this.filterCriteria.sortColumn = sort.active;
      this.filterCriteria.sortDirection = sort.direction;
      this.filterChange.emit(this.filterCriteria);
    });
    if (subscription) {
      this.subscriptions.push(subscription);
    }
  }

  private subscribeToFilterControls(): void {
    this.filters.forEach(filter => {
      const subscription = filter.filterChange
        .subscribe(value => {
          this.filterCriteria ??= new TableFilterCriteria();
          this.filterCriteria.filters.set(filter.tableFilter!, value);
          this.filterChange.emit(this.filterCriteria);
        });
      if (subscription) {
        this.subscriptions.push(subscription);
      }
    });
  }

  public ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }
}
