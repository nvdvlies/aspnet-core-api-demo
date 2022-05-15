import { HttpErrorResponse } from '@angular/common/http';
import { ErrorHandler, Injectable } from '@angular/core';
import { ValidationProblemDetails } from '@api/api.generated.clients';
import { ProblemDetailsError } from '@domain/shared/domain-entity-base';
import { LoggerService } from '@shared/services/logger.service';

@Injectable()
export class GlobalErrorHandlerService implements ErrorHandler {
  constructor(private readonly loggerService: LoggerService) {}

  public handleError(error: Error | HttpErrorResponse) {
    if (
      error instanceof ProblemDetailsError &&
      error.problemDetails instanceof ValidationProblemDetails
    ) {
      return;
    }
    this.loggerService.logError('An unhandled exception occured.', undefined, error);
    console.error(error);
  }
}
