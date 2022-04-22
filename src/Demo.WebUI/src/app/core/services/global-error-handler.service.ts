import { HttpErrorResponse } from '@angular/common/http';
import { ErrorHandler, Injectable } from '@angular/core';

@Injectable()
export class GlobalErrorHandlerService implements ErrorHandler {
  constructor() {}

  public handleError(error: Error | HttpErrorResponse) {
    console.error(error); // TODO
  }
}
