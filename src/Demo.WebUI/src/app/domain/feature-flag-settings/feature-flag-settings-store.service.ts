import { Injectable } from '@angular/core';
import { forkJoin, Observable, of, Subject } from 'rxjs';
import { filter, finalize, map, switchMap, tap } from 'rxjs/operators';
import {
  FeatureFlagSettingsDto,
  ApiFeatureFlagSettingsClient,
  SaveFeatureFlagSettingsCommand
} from '@api/api.generated.clients';
import {
  FeatureFlagSettingsEventsService,
  FeatureFlagSettingsUpdatedEvent
} from '@api/signalr.generated.services';

@Injectable({
  providedIn: 'root'
})
export class FeatureFlagSettingsStoreService {
  private featureFlagSettings: FeatureFlagSettingsDto | undefined;

  private updateLock: boolean = false;

  private readonly featureFlagSettingsUpdatedInStore = new Subject<
    [FeatureFlagSettingsUpdatedEvent, FeatureFlagSettingsDto]
  >();

  public readonly featureFlagSettingsUpdatedInStore$ =
    this.featureFlagSettingsUpdatedInStore.asObservable() as Observable<
      [FeatureFlagSettingsUpdatedEvent, FeatureFlagSettingsDto]
    >;

  constructor(
    private readonly apiFeatureFlagSettingsClient: ApiFeatureFlagSettingsClient,
    private readonly featureFlagSettingsEventsService: FeatureFlagSettingsEventsService
  ) {
    this.featureFlagSettingsEventsService.featureFlagSettingsUpdated$
      .pipe(
        filter(() => !this.updateLock),
        switchMap((event) => {
          return forkJoin([of(event), this.get(true)]);
        })
      )
      .subscribe(([event, featureFlagSettings]) => {
        this.featureFlagSettingsUpdatedInStore.next([event, featureFlagSettings]);
      });
  }

  public get(skipCache: boolean = false): Observable<FeatureFlagSettingsDto> {
    if (skipCache || this.featureFlagSettings == undefined) {
      return this.apiFeatureFlagSettingsClient.get().pipe(
        tap((response) => (this.featureFlagSettings = response.featureFlagSettings)),
        map((response) => response.featureFlagSettings!)
      );
    } else {
      return of(this.featureFlagSettings);
    }
  }

  public save(featureFlagSettings: FeatureFlagSettingsDto): Observable<FeatureFlagSettingsDto> {
    const command = new SaveFeatureFlagSettingsCommand();
    command.init({ ...featureFlagSettings });
    this.updateLock = true;
    return this.apiFeatureFlagSettingsClient.save(command).pipe(
      switchMap(() => this.get(true)),
      tap((response) => {
        const event: FeatureFlagSettingsUpdatedEvent = {
          id: response.id,
          updatedBy: response.lastModifiedBy!
        };
        this.featureFlagSettingsUpdatedInStore.next([event, response]);
      }),
      finalize(() => (this.updateLock = false))
    );
  }
}
