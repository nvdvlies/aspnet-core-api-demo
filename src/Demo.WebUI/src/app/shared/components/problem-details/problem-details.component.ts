import { Component, OnInit, ChangeDetectionStrategy, Input } from '@angular/core';
import { ApiException, ProblemDetails, ValidationProblemDetails } from '@api/api.generated.clients';

@Component({
  selector: 'app-problem-details',
  templateUrl: './problem-details.component.html',
  styleUrls: ['./problem-details.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ProblemDetailsComponent implements OnInit {
  private _problemDetails: ValidationProblemDetails | ProblemDetails | ApiException | undefined;
  get problemDetails(): ValidationProblemDetails | ProblemDetails | ApiException | undefined {
      return this._problemDetails;
  }
  @Input() set problemDetails(value: ValidationProblemDetails | ProblemDetails | ApiException | undefined) {
      this._problemDetails = value;
      this.setErrorMessage();
  }

  public errorMessage: string | undefined;

  constructor() { }

  ngOnInit(): void {
  }

  private setErrorMessage(): void {
    if (!this.problemDetails) {
      this.errorMessage = undefined;
      return;
    }

    if (this.problemDetails instanceof ValidationProblemDetails) {
      this.errorMessage = this.problemDetails.title;
      if (this.problemDetails.errors) {
        this.errorMessage += '<ul>';
        for (const key in this.problemDetails.errors) {
          const errors = this.problemDetails.errors[key];
          for (const error of errors) {
            this.errorMessage += `<li>${key}: ${error}</li>`;
          }
        }
        this.errorMessage += '</ul>';
      }
    } else if (this.problemDetails instanceof ProblemDetails) {
      this.errorMessage = this.problemDetails.title;
    } else if (this.problemDetails instanceof ApiException) {
      this.errorMessage = this.problemDetails.message;
    } else {
      this.errorMessage = 'An unknown exception occured.'
    }
  }
}
