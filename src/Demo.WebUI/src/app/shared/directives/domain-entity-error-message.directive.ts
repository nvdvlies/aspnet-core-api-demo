import { AfterViewInit, Directive, ElementRef } from '@angular/core';
import { MatFormField } from '@angular/material/form-field';
import { DomainEntityService } from '@domain/shared/domain-entity-base';

@Directive({
  selector: '[appDomainEntityErrorMessage]'
})
export class DomainEntityErrorMessageDirective implements AfterViewInit {
  constructor(
    private readonly elementRef: ElementRef,
    private readonly formField: MatFormField,
    private readonly domainEntityService: DomainEntityService
  ) {}

  public ngAfterViewInit() {
    this.setErrorMessage();
    this.formField._control.ngControl?.statusChanges?.subscribe((state: 'VALID' | 'INVALID') =>
      this.setErrorMessage(state)
    );
  }

  private setErrorMessage(state?: 'VALID' | 'INVALID'): void {
    if (state === 'VALID') {
      this.elementRef.nativeElement.innerHTML = '';
      return;
    }

    const errors = this.formField._control.ngControl?.errors;

    if (errors) {
      const [errorKey, errorValue] = Object.entries(errors)[0];
      this.elementRef.nativeElement.innerHTML = this.domainEntityService.getErrorMessage(
        errorKey,
        errorValue
      );
    }
  }
}
