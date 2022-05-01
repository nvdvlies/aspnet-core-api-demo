import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import {
  UserDto,
  ApiUsersClient,
  CreateUserCommand,
  UpdateUserCommand,
  DeleteUserCommand
} from '@api/api.generated.clients';
import {
  UserUpdatedEvent,
  UserDeletedEvent,
  UserEventsService
} from '@api/signalr.generated.services';
import { StoreBase } from '@domain/shared/store-base';

@Injectable({
  providedIn: 'root'
})
export class UserStoreService extends StoreBase<UserDto> {
  public readonly users$ = this.cache.asObservable();
  public readonly userUpdatedInStore$ = this.entityUpdatedInStore.asObservable() as Observable<
    [UserUpdatedEvent, UserDto]
  >;
  public readonly userDeletedFromStore$ =
    this.entityDeletedFromStore.asObservable() as Observable<UserDeletedEvent>;

  protected entityUpdatedEvent$ = this.userEventsService.userUpdated$;
  protected entityDeletedEvent$ = this.userEventsService.userDeleted$;

  constructor(
    private readonly apiUsersClient: ApiUsersClient,
    private readonly userEventsService: UserEventsService
  ) {
    super();
    super.init();
  }

  protected getByIdFunction = (id: string) => {
    return this.apiUsersClient.getUserById(id).pipe(map((x) => x.user!));
  };

  public override getById(id: string, skipCache: boolean = false): Observable<UserDto> {
    return super.getById(id, skipCache);
  }

  protected createFunction = (user: UserDto) => {
    const command = new CreateUserCommand();
    command.init({ ...user });
    return this.apiUsersClient.create(command).pipe(map((response) => response.id));
  };

  public override create(user: UserDto): Observable<UserDto> {
    return super.create(user);
  }

  protected updateFunction = (user: UserDto) => {
    const command = new UpdateUserCommand();
    command.init({ ...user });
    return this.apiUsersClient.update(user.id, command);
  };

  public override update(user: UserDto): Observable<UserDto> {
    return super.update(user);
  }

  protected deleteFunction = (id: string) => {
    const command = new DeleteUserCommand();
    return this.apiUsersClient.delete(id, command);
  };

  public override delete(id: string): Observable<void> {
    return super.delete(id);
  }
}
