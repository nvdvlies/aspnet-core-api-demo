import { ChangeDetectionStrategy, Component, OnDestroy, OnInit } from '@angular/core';
import { combineLatest, map, Observable } from 'rxjs';
import { CustomerTableDataSource } from '@customers/customer-list/customer-table-datasource';
import { CustomerTableDataContext, CustomerTableDataService } from '@customers/customer-list/customer-table-data.service';
import { TableFilterCriteria } from '@shared/directives/table-filter/table-filter-criteria';
import { SearchCustomerDto } from '@api/api.generated.clients';

interface ViewModel extends CustomerTableDataContext {
}

@Component({
  templateUrl: './customer-list.component.html',
  styleUrls: ['./customer-list.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CustomerListComponent implements OnInit, OnDestroy {
  public displayedColumns = ["code", "name"];
  public dataSource!: CustomerTableDataSource;
  public searchTerm = this.customerTableDataService.searchTerm;

  public vm$ = combineLatest([
    this.customerTableDataService.observe$,
  ])
    .pipe(
      map(([
       context
      ]) => {
        return {
          ...context
        } as ViewModel;
      })
    ) as Observable<ViewModel>;
    
  constructor(
    private readonly customerTableDataService: CustomerTableDataService
  ) { }

  ngOnInit(): void {
    this.dataSource = new CustomerTableDataSource(this.customerTableDataService);
  }

  public search(criteria: TableFilterCriteria): void {
    this.customerTableDataService.search(criteria);
  }

  public trackById(index: number, item: SearchCustomerDto): string {
    return item.id;
  }

  ngOnDestroy(): void {
    this.customerTableDataService.clearSpotlight();
  }
}
