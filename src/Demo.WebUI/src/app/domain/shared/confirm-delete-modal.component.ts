import { Component, ChangeDetectionStrategy } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  template: `
    <h1 mat-dialog-title>Delete?</h1>
    <div mat-dialog-content>
      <p>Are you sure you want to delete?</p>
    </div>
    <div mat-dialog-actions>
      <button mat-button (click)="confirmDelete()" cdkFocusInitial>Proceed</button>
      <button mat-button (click)="cancelDelete()">Cancel</button>
    </div>
  `,
  styles: [],
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
