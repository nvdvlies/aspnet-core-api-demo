import { Component, ChangeDetectionStrategy, Input } from '@angular/core';
import { ApiException, ProblemDetails, ValidationProblemDetails } from '@api/api.generated.clients';

@Component({
  selector: 'app-problem-details',
  templateUrl: './problem-details.component.html',
  styleUrls: ['./problem-details.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ProblemDetailsComponent {
  private _problemDetails: ValidationProblemDetails | ProblemDetails | ApiException | undefined;
  get problemDetails(): ValidationProblemDetails | ProblemDetails | ApiException | undefined {
    return this._problemDetails;
  }
  @Input() set problemDetails(
    value: ValidationProblemDetails | ProblemDetails | ApiException | undefined
  ) {
    this._problemDetails = value;
    this.setErrorMessage();
  }

  public errorMessages: string[] = [];

  constructor() {}

  private setErrorMessage(): void {
    if (!this.problemDetails) {
      this.errorMessages = [];
      return;
    }

    if (this.problemDetails instanceof ValidationProblemDetails) {
      if (this.problemDetails.errors) {
        for (const key in this.problemDetails.errors) {
          const errors = this.problemDetails.errors[key];
          for (const error of errors) {
            this.errorMessages.push(error);
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
      this.errorMessages.push('An unknown exception occured.');
    }
  }
}
