import { Component, ChangeDetectionStrategy, Input, OnInit, Optional } from '@angular/core';
import { ControlContainer, FormControl, FormGroup } from '@angular/forms';
import { ApiException, ProblemDetails, ValidationProblemDetails } from '@api/api.generated.clients';
import { LoggerService } from '@shared/services/logger.service';

@Component({
  selector: 'app-problem-details',
  templateUrl: './problem-details.component.html',
  styleUrls: ['./problem-details.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ProblemDetailsComponent implements OnInit {
  private _problemDetails: ValidationProblemDetails | ProblemDetails | ApiException | undefined;
  public get problemDetails():
    | ValidationProblemDetails
    | ProblemDetails
    | ApiException
    | undefined {
    return this._problemDetails;
  }
  @Input() public set problemDetails(
    value: ValidationProblemDetails | ProblemDetails | ApiException | undefined
  ) {
    this._problemDetails = value;
    this.setErrorMessage();
  }

  public errorMessages: string[] = [];
  private form: FormGroup | undefined;

  constructor(
    @Optional() private readonly controlContainer: ControlContainer,
    private readonly loggerService: LoggerService
  ) {}

  ngOnInit(): void {
    this.form = this.controlContainer?.control as FormGroup;
  }

  private setErrorMessage(): void {
    if (!this.problemDetails) {
      this.errorMessages = [];
      return;
    }

    if (this.problemDetails instanceof ValidationProblemDetails) {
      if (this.problemDetails.errors) {
        for (const key in this.problemDetails.errors) {
          const path = this.getFormControlPath(key);
          const formControl = this.form?.get(path);
          if (formControl && formControl instanceof FormControl && formControl.enabled) {
            formControl.setErrors({
              serverError: this.problemDetails.errors[key][0]
            });
          } else {
            this.errorMessages.push(this.problemDetails.errors[key][0]);
          }
        }
      } else {
        this.errorMessages.push(this.problemDetails.title!);
      }
    } else if (this.problemDetails instanceof ProblemDetails) {
      this.errorMessages.push(this.problemDetails.title!);
    } else if (this.problemDetails instanceof ApiException) {
      this.errorMessages.push(this.problemDetails.message);
    } else {
      this.loggerService.logError('Unhandled ProblemDetails', undefined, this.problemDetails);
      this.errorMessages.push('An unknown exception occured.');
    }
  }

  /*
    - InvoiceNumber -> ['invoiceNumber']
    - Address.PostalCode -> ['address', 'postalCode']
    - InvoiceLines[0].SellingPrice -> ['invoiceLines', 0, 'sellingPrice']
  */
  private getFormControlPath(propertyName: string): Array<string | number> {
    const result: Array<string | number> = [];
    if (!propertyName) {
      return result;
    }
    const parts = propertyName.split('.');
    for (let part of parts) {
      part = part.charAt(0).toLowerCase() + part.slice(1);
      const index = /\[(?<index>\d+)\]$/.exec(part)?.groups?.['index'];
      if (index != null) {
        result.push(part.substring(0, part.indexOf('[')));
        result.push(index);
      } else {
        result.push(part);
      }
    }
    return result;
  }
}
