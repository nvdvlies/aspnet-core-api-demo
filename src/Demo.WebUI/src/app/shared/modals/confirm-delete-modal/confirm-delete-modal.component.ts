import { Component, ChangeDetectionStrategy } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  templateUrl: './confirm-delete-modal.component.html',
  styleUrls: ['./confirm-delete-modal.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ConfirmDeleteModalComponent {
  constructor(public dialogRef: MatDialogRef<ConfirmDeleteModalComponent>) {}

  public confirmDelete(): void {
    this.dialogRef.close(true);
  }

  public cancelDelete(): void {
    this.dialogRef.close(false);
  }
}
