import { Component, ChangeDetectionStrategy, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

export interface ConfirmModalComponentData {
  title: string | undefined;
  message: string | undefined;
}

@Component({
  selector: 'app-confirm-modal',
  templateUrl: './confirm-modal.component.html',
  styleUrls: ['./confirm-modal.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ConfirmModalComponent {
  constructor(
    public readonly dialogRef: MatDialogRef<ConfirmModalComponentData>,
    @Inject(MAT_DIALOG_DATA) public data: ConfirmModalComponentData
  ) {}

  public confirm(): void {
    this.dialogRef.close(true);
  }

  public cancel(): void {
    this.dialogRef.close(false);
  }
}
