import { Directive, HostListener, Inject, ElementRef } from '@angular/core';

@Directive({
  selector:
    'input[type=text]:not(.disable-select-on-focus), input[type=email]:not(.disable-select-on-focus)'
})
/* eslint-disable-next-line @angular-eslint/directive-selector */
export class SelectOnFocusDirective {
  constructor(@Inject(ElementRef) private elementRef: ElementRef) {}

  @HostListener('focus')
  public onFocus(): void {
    setTimeout(() => {
      const nativeElement = this.elementRef.nativeElement;
      nativeElement.setSelectionRange(0, nativeElement.value?.length ?? 0);
    }, 10);
  }
}
