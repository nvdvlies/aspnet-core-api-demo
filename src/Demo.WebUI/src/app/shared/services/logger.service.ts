import { Injectable } from '@angular/core';

@Injectable()
export abstract class LoggerService {
  public abstract logInfo(message: string, eventId?: number): void;
  public abstract logWarning(message: string, eventId?: number): void;
  public abstract logError(message: string, eventId?: number, error?: Error): void;
  public abstract logCritical(message: string, eventId?: number, error?: Error): void;
}
