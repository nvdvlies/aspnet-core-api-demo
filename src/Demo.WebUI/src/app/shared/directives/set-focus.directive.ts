import { BooleanInput, coerceBooleanProperty } from '@angular/cdk/coercion';
import { AfterContentInit, Directive, ElementRef, Input } from '@angular/core';

@Directive({
  selector: '[appSetFocus]'
})
export class SetFocusDirective implements AfterContentInit {
  @Input()
  get appSetFocus() {
    return this._appSetFocus;
  }
  set appSetFocus(value: BooleanInput) {
    this._appSetFocus = coerceBooleanProperty(value);
  }
  private _appSetFocus = false;

  public constructor(private elementRef: ElementRef) {}

  public ngAfterContentInit() {
    if (this.appSetFocus) {
      setTimeout(() => {
        if (this.elementRef.nativeElement.tagName === 'INPUT') {
          this.elementRef.nativeElement.focus();
        } else {
          (
            this.elementRef.nativeElement.querySelector(
              'input:not([type=hidden]):not([hidden])'
            ) as HTMLElement
          )?.focus();
        }
      }, 10);
    }
  }
}
