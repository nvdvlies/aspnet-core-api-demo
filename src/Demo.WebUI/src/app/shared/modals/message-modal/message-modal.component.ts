import { Component, ChangeDetectionStrategy, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

export interface MessageModalComponentData {
  title: string | undefined;
  message: string | undefined;
}

@Component({
  selector: 'app-message-modal',
  templateUrl: './message-modal.component.html',
  styleUrls: ['./message-modal.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MessageModalComponent {
  constructor(@Inject(MAT_DIALOG_DATA) public data: MessageModalComponentData) {}
}
