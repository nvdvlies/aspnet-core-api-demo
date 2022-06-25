import { Injectable } from '@angular/core';
import { forkJoin, Observable, of, Subject } from 'rxjs';
import { filter, finalize, map, switchMap, tap } from 'rxjs/operators';
import {
  UserPreferencesDto,
  ApiUserPreferencesClient,
  SaveUserPreferencesCommand
} from '@api/api.generated.clients';
import {
  UserPreferencesEventsService,
  UserPreferencesUpdatedEvent
} from '@api/signalr.generated.services';

@Injectable({
  providedIn: 'root'
})
export class UserPreferencesStoreService {
  private userPreferences: UserPreferencesDto | undefined;

  private updateLock: boolean = false;

  private readonly userPreferencesUpdatedInStore = new Subject<
    [UserPreferencesUpdatedEvent, UserPreferencesDto]
  >();

  public readonly userPreferencesUpdatedInStore$ =
    this.userPreferencesUpdatedInStore.asObservable() as Observable<
      [UserPreferencesUpdatedEvent, UserPreferencesDto]
    >;

  constructor(
    private readonly apiUserPreferencesClient: ApiUserPreferencesClient,
    private readonly userPreferencesEventsService: UserPreferencesEventsService
  ) {
    this.userPreferencesEventsService.userPreferencesUpdated$
      .pipe(
        filter(() => !this.updateLock),
        switchMap((event) => {
          return forkJoin([of(event), this.get(true)]);
        })
      )
      .subscribe(([event, userPreferences]) => {
        this.userPreferencesUpdatedInStore.next([event, userPreferences]);
      });
  }

  public get(skipCache: boolean = false): Observable<UserPreferencesDto> {
    if (skipCache || this.userPreferences == undefined) {
      return this.apiUserPreferencesClient.get().pipe(
        tap((response) => (this.userPreferences = response.userPreferences)),
        map((response) => response.userPreferences!)
      );
    } else {
      return of(this.userPreferences);
    }
  }

  public save(userPreferences: UserPreferencesDto): Observable<UserPreferencesDto> {
    const command = new SaveUserPreferencesCommand();
    command.init({ ...userPreferences });
    this.updateLock = true;
    return this.apiUserPreferencesClient.save(command).pipe(
      switchMap(() => this.get(true)),
      tap((response) => {
        const event: UserPreferencesUpdatedEvent = {
          id: response.id,
          updatedBy: response.lastModifiedBy!
        };
        this.userPreferencesUpdatedInStore.next([event, response]);
      }),
      finalize(() => (this.updateLock = false))
    );
  }
}
