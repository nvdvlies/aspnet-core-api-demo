import { HttpErrorResponse } from '@angular/common/http';
import { ErrorHandler, Injectable } from '@angular/core';
import { ValidationProblemDetails } from '@api/api.generated.clients';
import { ProblemDetailsError } from '@domain/shared/domain-entity-base';

@Injectable()
export class GlobalErrorHandlerService implements ErrorHandler {
  constructor() {}

  public handleError(error: Error | HttpErrorResponse) {
    if (
      error instanceof ProblemDetailsError &&
      error.problemDetails instanceof ValidationProblemDetails
    ) {
      return;
    }
    // TODO: log
    console.error(error);
  }
}
