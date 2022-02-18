import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  templateUrl: './discard-unsaved-changes-modal.component.html',
  styleUrls: ['./discard-unsaved-changes-modal.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DiscardUnsavedChangesModalComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<DiscardUnsavedChangesModalComponent>
  ) {}

  ngOnInit(): void {
  }

  public confirmDiscard(): void {
    this.dialogRef.close(true);
  }

  public cancelDiscard(): void {
    this.dialogRef.close(false);
  }
}
