import { animate, state, style, transition, trigger } from '@angular/animations';
import {
  Component,
  OnInit,
  ChangeDetectionStrategy,
  Input,
  Output,
  EventEmitter
} from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { AuditlogDto } from '@api/api.generated.clients';

export interface AuditlogTableData {
  items: AuditlogDto[];
  isLoading: boolean;
  pageIndex: number | undefined;
  totalItems: number | undefined;
  totalPages: number | undefined;
  hasPreviousPage: boolean | undefined;
  hasNextPage: boolean | undefined;
}

@Component({
  selector: 'app-auditlog-table',
  templateUrl: './auditlog-table.component.html',
  styleUrls: ['./auditlog-table.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({ height: '0px', minHeight: '0' })),
      state('expanded', style({ height: '*' })),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)'))
    ])
  ]
})
export class AuditlogTableComponent implements OnInit {
  @Input()
  public data: AuditlogTableData | undefined;

  @Output()
  public pageIndexChanged = new EventEmitter<number>();

  public displayedColumns = ['expand', 'modifiedOn', 'modifiedBy'];
  public expandedRow: AuditlogDto | null = null;

  constructor() {}

  ngOnInit(): void {}

  public pageChange(event: PageEvent): void {
    if (event.pageIndex !== event.previousPageIndex) {
    }
    this.pageIndexChanged.emit(event.pageIndex);
  }

  public expandOrCollapseRow(item: AuditlogDto): void {
    if (this.expandedRow && this.expandedRow.id === item.id) {
      this.expandedRow = null;
    } else {
      this.expandedRow = item;
    }
  }

  public trackById(index: number, item: AuditlogDto): string {
    return item.id;
  }
}
