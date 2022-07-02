import { coerceBooleanProperty } from '@angular/cdk/coercion';
import { ComponentType } from '@angular/cdk/portal';
import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import {
  MessageModalComponent,
  MessageModalComponentData
} from '@shared/modals/message-modal/message-modal.component';
import { map, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ModalService {
  constructor(protected readonly matDialog: MatDialog) {}

  public confirm<T>(component: ComponentType<T>): Observable<boolean> {
    const dialogRef = this.matDialog.open(component);
    return dialogRef
      .afterClosed()
      .pipe(map((confirmDelete) => coerceBooleanProperty(confirmDelete)));
  }

  public showMessage(message: string, title?: string): void {
    const data: MessageModalComponentData = { message, title };
    this.matDialog.open(MessageModalComponent, { data, maxWidth: 500 });
  }
}
