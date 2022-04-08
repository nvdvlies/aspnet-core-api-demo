import { Component, ChangeDetectionStrategy, Input } from '@angular/core';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MessageComponent {
  @Input()
  type!: 'info' | 'success' | 'warning' | 'error';
}
