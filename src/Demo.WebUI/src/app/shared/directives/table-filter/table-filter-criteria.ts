import { SortDirection } from "@angular/material/sort";

export class TableFilterCriteria {
  pageIndex: number;
  pageSize: number;
  sortColumn: string | undefined;
  sortDirection: SortDirection | undefined;
  filters: Map<string, any>;

  constructor() {
    this.pageIndex = 0;
    this.pageSize = 10;
    this.filters = new Map<string, any>();
  }
}