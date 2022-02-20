import { AfterViewInit, Component } from '@angular/core';
import { MatFormField } from '@angular/material/form-field';
import { BaseDomainEntityService } from '@domain/shared/domain-entity-base';

@Component({
  selector: '[appDomainEntityErrorMessage]',
  template: '{{ errorMessage }}'
})
export class DomainEntityErrorMessageDirective implements AfterViewInit {
  public errorMessage: string | undefined;

  constructor(
    private readonly formField: MatFormField,
    private readonly domainEntityService: BaseDomainEntityService
  ) { 
  }

  public ngAfterViewInit() {
    this.setErrorMessage();
    this.formField._control.ngControl?.statusChanges?.subscribe((state: 'VALID' | 'INVALID') => this.setErrorMessage(state));
  }

  private setErrorMessage(state?: 'VALID' | 'INVALID'): void {
    if (state === 'VALID') { 
      this.errorMessage = undefined;
      return;
    }

    const errors = this.formField._control.ngControl?.errors;

    if (errors) {
      const [errorKey, errorValue] = Object.entries(errors)[0];
      this.errorMessage = this.domainEntityService.getErrorMessage(errorKey, errorValue);
    }    
  }
}
