import { AbstractControl, UntypedFormControl, ValidationErrors } from '@angular/forms';
import { Subject, Observable } from 'rxjs';

export type WarningControl = {
  warnings?: ValidationErrors | null;
  warningChanges?: Observable<ValidationErrors | null>;
  setWarnings?(
    errors: ValidationErrors | null,
    opts?: {
      emitEvent?: boolean;
    }
  ): void;
};

export class ExtendedFormControl extends UntypedFormControl {
  private readonly warningChangesSubject = new Subject<ValidationErrors | null>();
  private _warnings: ValidationErrors | null = null;

  public warningChanges = this.warningChangesSubject.asObservable();

  public get warnings(): ValidationErrors | null {
    return this._warnings;
  }

  public setWarnings(
    warnings: ValidationErrors | null,
    opts?: {
      emitEvent?: boolean;
    }
  ): void {
    this._warnings = warnings;
    if (opts?.emitEvent == undefined || opts?.emitEvent === true) {
      this.warningChangesSubject?.next(this._warnings);
    }
  }
}

export type ExtendedAbstractControl = AbstractControl & WarningControl;
