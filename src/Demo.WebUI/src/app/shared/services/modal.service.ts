import { coerceBooleanProperty } from '@angular/cdk/coercion';
import { ComponentType } from '@angular/cdk/portal';
import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
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
}
