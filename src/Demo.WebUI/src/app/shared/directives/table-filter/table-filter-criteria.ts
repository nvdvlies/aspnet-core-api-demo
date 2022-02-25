import { SortDirection } from '@angular/material/sort';

export interface ITableFilterCriteria {
  pageIndex: number;
  pageSize: number;
  sortColumn: any | undefined;
  sortDirection: SortDirection | undefined;
}

export class TableFilterCriteria implements ITableFilterCriteria {
  pageIndex: number;
  pageSize: number;
  sortColumn: string | undefined;
  sortDirection: SortDirection | undefined;

  constructor() {
    this.pageIndex = 0;
    this.pageSize = 10;
  }
}
