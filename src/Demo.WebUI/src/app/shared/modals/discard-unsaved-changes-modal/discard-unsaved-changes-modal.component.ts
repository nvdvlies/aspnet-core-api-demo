import { Component, ChangeDetectionStrategy } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  templateUrl: './discard-unsaved-changes-modal.component.html',
  styleUrls: ['./discard-unsaved-changes-modal.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DiscardUnsavedChangesModalComponent {
  constructor(public dialogRef: MatDialogRef<DiscardUnsavedChangesModalComponent>) {}

  public confirmDiscard(): void {
    this.dialogRef.close(true);
  }

  public cancelDiscard(): void {
    this.dialogRef.close(false);
  }
}
