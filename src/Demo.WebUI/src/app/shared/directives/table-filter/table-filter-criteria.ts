import { SortDirection } from "@angular/material/sort";

export class TableFilterCriteria {
  pageIndex: number;
  pageSize: number;
  sortColumn: string | undefined;
  sortDirection: SortDirection | undefined;

  constructor() {
    this.pageIndex = 0;
    this.pageSize = 10;
  }
}