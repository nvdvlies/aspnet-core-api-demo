import { BooleanInput, coerceBooleanProperty } from '@angular/cdk/coercion';
import { AfterContentInit, Directive, ElementRef, Input } from '@angular/core';

@Directive({
  selector: '[appFocus]'
})
export class FocusDirective implements AfterContentInit {
  @Input()
  get appFocus() {
    return this._appFocus;
  }
  set appFocus(value: BooleanInput) {
    this._appFocus = coerceBooleanProperty(value);
  }
  private _appFocus = false;

  public constructor(private elementRef: ElementRef) {}

  public ngAfterContentInit() {
    if (this.appFocus) {
      this.elementRef.nativeElement.focus();
    }
  }
}
