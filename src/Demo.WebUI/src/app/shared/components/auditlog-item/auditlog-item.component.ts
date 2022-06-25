import { Component, OnInit, ChangeDetectionStrategy, Input } from '@angular/core';
import { AuditlogItemDto, AuditlogStatusEnum, AuditlogTypeEnum } from '@api/api.generated.clients';

@Component({
  selector: 'app-auditlog-item',
  templateUrl: './auditlog-item.component.html',
  styleUrls: ['./auditlog-item.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AuditlogItemComponent implements OnInit {
  @Input()
  public item: AuditlogItemDto | undefined;

  @Input()
  public entityName: string | undefined;

  @Input()
  public parentPropertyName: string | undefined;

  @Input()
  public indent: number = 0;

  public AuditlogStatusEnum = AuditlogStatusEnum;
  public AuditlogTypeEnum = AuditlogTypeEnum;

  constructor() {}

  ngOnInit(): void {}

  public getParentPropertyName(parentItem: AuditlogItemDto): string | undefined {
    return this.parentPropertyName
      ? `${this.parentPropertyName}.${parentItem.propertyName}`
      : parentItem.propertyName;
  }
}
