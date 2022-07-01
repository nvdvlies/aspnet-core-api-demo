import { Injectable } from '@angular/core';
import { forkJoin, Observable, of, Subject } from 'rxjs';
import { filter, finalize, map, switchMap, tap } from 'rxjs/operators';
import {
  CurrentUserDto,
  ApiCurrentUserClient,
  UpdateCurrentUserCommand
} from '@api/api.generated.clients';
import { CurrentUserEventsService, CurrentUserUpdatedEvent } from '@api/signalr.generated.services';

@Injectable({
  providedIn: 'root'
})
export class CurrentUserStoreService {
  private currentUser: CurrentUserDto | undefined;

  private updateLock: boolean = false;

  private readonly currentUserUpdatedInStore = new Subject<
    [CurrentUserUpdatedEvent, CurrentUserDto]
  >();

  public readonly currentUserUpdatedInStore$ = this.currentUserUpdatedInStore.asObservable();

  constructor(
    private readonly apiCurrentUserClient: ApiCurrentUserClient,
    private readonly currentUserEventsService: CurrentUserEventsService
  ) {
    this.currentUserEventsService.currentUserUpdated$
      .pipe(
        filter(() => !this.updateLock),
        switchMap((event) => {
          return forkJoin([of(event), this.get(true)]);
        })
      )
      .subscribe(([event, currentUser]) => {
        this.currentUserUpdatedInStore.next([event, currentUser]);
      });
  }

  public get(skipCache: boolean = false): Observable<CurrentUserDto> {
    if (skipCache || this.currentUser == undefined) {
      return this.apiCurrentUserClient.getCurrentUser().pipe(
        tap((response) => (this.currentUser = response.currentUser)),
        map((response) => response.currentUser!)
      );
    } else {
      return of(this.currentUser);
    }
  }

  public save(currentUser: CurrentUserDto): Observable<CurrentUserDto> {
    const command = new UpdateCurrentUserCommand();
    command.init({ ...currentUser });
    this.updateLock = true;
    return this.apiCurrentUserClient.update(command).pipe(
      switchMap(() => this.get(true)),
      tap((response) => {
        const event: CurrentUserUpdatedEvent = {
          updatedBy: response.lastModifiedBy!
        };
        this.currentUserUpdatedInStore.next([event, response]);
      }),
      finalize(() => (this.updateLock = false))
    );
  }
}
