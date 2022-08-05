import { AfterViewInit, Directive, ElementRef } from '@angular/core';
import { ValidationErrors } from '@angular/forms';
import { MatFormField } from '@angular/material/form-field';
import { DomainEntityService } from '@domain/shared/domain-entity-base';
import { ExtendedAbstractControl } from '@domain/shared/form-controls-base';

@Directive({
  selector: '[appDomainEntityWarningMessage]'
})
export class DomainEntityWarningMessageDirective implements AfterViewInit {
  constructor(
    private readonly elementRef: ElementRef,
    private readonly formField: MatFormField,
    private readonly domainEntityService: DomainEntityService
  ) {}

  public ngAfterViewInit() {
    this.setWarningMessage();
    (
      this.formField._control.ngControl!.control as ExtendedAbstractControl
    )?.warningChanges?.subscribe((warnings) => this.setWarningMessage(warnings));
  }

  private setWarningMessage(warnings?: ValidationErrors | null): void {
    console.log(warnings);
    if (!warnings) {
      this.elementRef.nativeElement.innerHTML = '';
      return;
    }

    if (warnings) {
      const [warningKey, warningValue] = Object.entries(warnings)[0];
      this.elementRef.nativeElement.innerHTML = this.domainEntityService.getFormControlMessage(
        warningKey,
        warningValue
      );
    }
  }
}
