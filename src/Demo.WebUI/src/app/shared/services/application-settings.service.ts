import { Injectable } from '@angular/core';
import { ApiApplicationSettingsClient, ApplicationSettingsDto } from '@api/api.generated.clients';
import { CurrentUserService } from '@core/services/current-user.service';
import { BehaviorSubject, Observable, of, switchMap, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApplicationSettingsService {
  private applicationSettings: ApplicationSettingsDto | undefined;

  public isInitialized$ = new BehaviorSubject<boolean>(false);

  constructor(
    private readonly apiApplicationSettingsClient: ApiApplicationSettingsClient,
    private readonly currentUserService: CurrentUserService
  ) {
    this.currentUserService.isAuthenticated$.subscribe((isAuthenticated) => {
      if (!isAuthenticated) {
        this.applicationSettings = undefined;
      }
    });
  }

  public init(skipCache: boolean = false): Observable<boolean> {
    if (skipCache || this.applicationSettings == undefined) {
      return this.apiApplicationSettingsClient.get().pipe(
        tap((response) => (this.applicationSettings = response.applicationSettings)),
        tap(() => this.isInitialized$.next(true)),
        switchMap(() => of(true))
      );
    } else {
      return of(true);
    }
  }

  public get setting1(): boolean {
    return this.applicationSettings?.settings?.setting1 ?? false;
  }
}
