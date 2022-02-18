import { coerceBooleanProperty } from '@angular/cdk/coercion';
import { Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRouteSnapshot, CanDeactivate, RouterStateSnapshot } from '@angular/router';
import { DiscardUnsavedChangesModalComponent } from '@shared/modals/discard-unsaved-changes-modal/discard-unsaved-changes-modal.component';
import { map, Observable } from 'rxjs';

export interface IHasForm {
  form: FormGroup;
}

@Injectable({
  providedIn: 'root'
})
export class UnsavedChangesGuard implements CanDeactivate<IHasForm> {

  constructor(
    private readonly matDialog: MatDialog
  ){
  }

  canDeactivate(
    component: IHasForm,
    currentRoute: ActivatedRouteSnapshot,
    currentState: RouterStateSnapshot,
    nextState?: RouterStateSnapshot): Observable<boolean> | boolean {
      if (component.form.dirty) {
        return this.confirmDialog();  
      }
      return true;
  }

  private confirmDialog(): Observable<boolean> {
    const dialogRef = this.matDialog.open(DiscardUnsavedChangesModalComponent);
    return dialogRef.afterClosed()
      .pipe(
        map(canDeactivate => coerceBooleanProperty(canDeactivate))
      );
  }
}
