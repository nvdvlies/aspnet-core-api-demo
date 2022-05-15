import { Injectable } from '@angular/core';
import { ApiLogClient, LogLevel, LogMessage } from '@api/api.generated.clients';
import { bufferTime, catchError, EMPTY, filter, Subject, switchMap } from 'rxjs';
import { LoggerService } from './logger.service';

@Injectable({
  providedIn: 'root'
})
export class ApiLoggerService implements LoggerService {
  private logSubject = new Subject<LogMessage>();

  constructor(private readonly apiLogClient: ApiLogClient) {
    this.logSubject
      .pipe(
        bufferTime(5000, null, 5),
        filter((buffer) => buffer.length > 0),
        switchMap((logMessages) => this.apiLogClient.post(logMessages)),
        catchError((error) => {
          console.error(error);
          return EMPTY;
        })
      )
      .subscribe();
  }

  public logInfo(message: string, eventId?: number): void {
    this.logSubject.next({
      logLevel: LogLevel.Information,
      eventId: eventId,
      message: message
    } as LogMessage);
  }

  public logWarning(message: string, eventId?: number): void {
    this.logSubject.next({
      logLevel: LogLevel.Warning,
      eventId: eventId,
      message: message
    } as LogMessage);
  }

  public logError(message: string, eventId?: number, error?: Error): void {
    this.logSubject.next({
      logLevel: LogLevel.Error,
      eventId: eventId,
      message: message,
      exceptionMessage: error?.toString()
    } as LogMessage);
  }

  public logCritical(message: string, eventId?: number, error?: Error): void {
    this.logSubject.next({
      logLevel: LogLevel.Critical,
      eventId: eventId,
      message: message,
      exceptionMessage: error?.toString()
    } as LogMessage);
  }
}
