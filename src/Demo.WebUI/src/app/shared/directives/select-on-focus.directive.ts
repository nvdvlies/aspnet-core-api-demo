import { Directive, HostListener, Inject, ElementRef } from '@angular/core';

@Directive({
  selector:
    'input[type=text]:not(.disable-select-on-focus), input[type=number]:not(.disable-select-on-focus), input[type=email]:not(.disable-select-on-focus)'
})
export class SelectOnFocusDirective {
  constructor(@Inject(ElementRef) private elementRef: ElementRef) {}

  @HostListener('focus')
  public onFocus(): void {
    setTimeout(() => {
      const nativeElement = this.elementRef.nativeElement;
      nativeElement.select();
    }, 10);
  }
}
