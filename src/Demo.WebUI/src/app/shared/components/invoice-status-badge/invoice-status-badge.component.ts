import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { InvoiceStatusEnum } from '@api/api.generated.clients';

@Component({
  selector: 'app-invoice-status-badge',
  templateUrl: './invoice-status-badge.component.html',
  styleUrls: ['./invoice-status-badge.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class InvoiceStatusBadgeComponent {
  @Input()
  public status: InvoiceStatusEnum | undefined;

  public InvoiceStatusEnum = InvoiceStatusEnum;

  constructor() {}
}
