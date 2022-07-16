import {
  Component,
  Optional,
  ElementRef,
  Host,
  SkipSelf,
  Self,
  ChangeDetectionStrategy,
  ViewChild,
  OnInit
} from '@angular/core';
import { ControlContainer, UntypedFormControl, NgControl } from '@angular/forms';
import { FocusMonitor } from '@angular/cdk/a11y';
import { MatFormFieldControl } from '@angular/material/form-field';
import { MatFormFieldControlBase } from '@shared/base/mat-form-field-control-base';
import { CurrencyUtils } from '@shared/utils/currency.utils';
import { KeyboardUtils } from '@shared/utils/keyboard.util';

export interface ViewModel {}

@Component({
  selector: 'app-currency-form-field-control',
  templateUrl: './currency-form-field-control.component.html',
  styleUrls: ['./currency-form-field-control.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
    {
      provide: MatFormFieldControl,
      useExisting: CurrencyFormFieldControlComponent
    }
  ]
})
export class CurrencyFormFieldControlComponent
  extends MatFormFieldControlBase<string>
  implements OnInit
{
  @ViewChild('input', { static: true })
  public inputElementRef!: ElementRef;

  public inputFormControl = new UntypedFormControl(null);

  constructor(
    @Optional() elementRef: ElementRef<HTMLElement>,
    @Optional() focusMonitor: FocusMonitor,
    @Host() @SkipSelf() controlContainer: ControlContainer,
    @Optional() @Self() ngControl: NgControl
  ) {
    super(elementRef, focusMonitor, controlContainer, ngControl);
  }

  public override ngOnInit(): void {
    super.ngOnInit();

    this.formControl.status === 'DISABLED'
      ? this.inputFormControl.disable()
      : this.inputFormControl.enable();
    this.formControl.statusChanges.subscribe((status) => {
      status === 'DISABLED' ? this.inputFormControl.disable() : this.inputFormControl.enable();
    });
  }

  public onFocus(): void {
    this.inputFormControl.setValue(CurrencyUtils.parse(this.value)?.toString());
    this.inputElementRef.nativeElement.select();
  }

  public onBlur(event: Event): void {
    const value = (<HTMLInputElement>event.target).value;
    this.value = CurrencyUtils.parse(value);
    this.inputFormControl.setValue(CurrencyUtils.format(this.value));
  }

  public onKeydown(event: KeyboardEvent): void {
    const isNumericOrNonPrintableKey =
      /[0-9\+\-\.\,]/.test(event.key) || KeyboardUtils.isNonPrintableKey(event);
    if (!isNumericOrNonPrintableKey) {
      event.preventDefault();
    }
  }

  public override writeValue(value: any): void {
    super.writeValue(value);
    this.inputFormControl.setValue(CurrencyUtils.format(CurrencyUtils.parse(value)) ?? '');
  }
}
