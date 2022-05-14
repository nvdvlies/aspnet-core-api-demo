import { Injectable } from '@angular/core';
import { forkJoin, Observable, of, Subject } from 'rxjs';
import { filter, finalize, map, switchMap, tap } from 'rxjs/operators';
import {
  ApplicationSettingsDto,
  ApiApplicationSettingsClient,
  SaveApplicationSettingsCommand
} from '@api/api.generated.clients';
import {
  ApplicationSettingsEventsService,
  ApplicationSettingsUpdatedEvent
} from '@api/signalr.generated.services';

@Injectable({
  providedIn: 'root'
})
export class ApplicationSettingsStoreService {
  private applicationSettings: ApplicationSettingsDto | undefined;

  private updateLock: boolean = false;

  private readonly applicationSettingsUpdatedInStore = new Subject<
    [ApplicationSettingsUpdatedEvent, ApplicationSettingsDto]
  >();

  public readonly applicationSettingsUpdatedInStore$ =
    this.applicationSettingsUpdatedInStore.asObservable() as Observable<
      [ApplicationSettingsUpdatedEvent, ApplicationSettingsDto]
    >;

  constructor(
    private readonly apiApplicationSettingsClient: ApiApplicationSettingsClient,
    private readonly applicationSettingsEventsService: ApplicationSettingsEventsService
  ) {
    this.applicationSettingsEventsService.applicationSettingsUpdated$
      .pipe(
        filter(() => !this.updateLock),
        switchMap((event) => {
          return forkJoin([of(event), this.get(true)]);
        })
      )
      .subscribe(([event, applicationSettings]) => {
        this.applicationSettingsUpdatedInStore.next([event, applicationSettings]);
      });
  }

  public get(skipCache: boolean = false): Observable<ApplicationSettingsDto> {
    if (skipCache || this.applicationSettings == undefined) {
      return this.apiApplicationSettingsClient.get().pipe(
        tap((response) => (this.applicationSettings = response.applicationSettings)),
        map((response) => response.applicationSettings!)
      );
    } else {
      return of(this.applicationSettings);
    }
  }

  public save(applicationSettings: ApplicationSettingsDto): Observable<ApplicationSettingsDto> {
    const command = new SaveApplicationSettingsCommand();
    command.init({ ...applicationSettings });
    this.updateLock = true;
    return this.apiApplicationSettingsClient.save(command).pipe(
      switchMap(() => this.get(true)),
      finalize(() => (this.updateLock = false))
    );
  }
}
