import { Directive, ElementRef, OnInit } from '@angular/core';

@Directive({
  selector: '[appCurrencyInput]'
})
export class CurrencyInputDirective implements OnInit {
  private inputElement: HTMLInputElement;

  constructor(private elementRef: ElementRef) {
    this.inputElement = this.elementRef.nativeElement;
  }

  public ngOnInit(): void {}
}
