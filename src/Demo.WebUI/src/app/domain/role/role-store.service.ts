import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import {
  RoleDto,
  ApiRolesClient,
  CreateRoleCommand,
  UpdateRoleCommand,
  DeleteRoleCommand
} from '@api/api.generated.clients';
import {
  RoleUpdatedEvent,
  RoleDeletedEvent,
  RoleEventsService
} from '@api/signalr.generated.services';
import { StoreBase } from '@domain/shared/store-base';

@Injectable({
  providedIn: 'root'
})
export class RoleStoreService extends StoreBase<RoleDto> {
  public readonly roles$ = this.cache.asObservable();
  public readonly roleUpdatedInStore$ = this.entityUpdatedInStore.asObservable() as Observable<
    [RoleUpdatedEvent, RoleDto]
  >;
  public readonly roleDeletedFromStore$ =
    this.entityDeletedFromStore.asObservable() as Observable<RoleDeletedEvent>;

  protected entityUpdatedEvent$ = this.roleEventsService.roleUpdated$;
  protected entityDeletedEvent$ = this.roleEventsService.roleDeleted$;

  constructor(
    private readonly apiRolesClient: ApiRolesClient,
    private readonly roleEventsService: RoleEventsService
  ) {
    super();
    super.init();
  }

  protected getByIdFunction = (id: string) => {
    return this.apiRolesClient.getRoleById(id).pipe(map((x) => x.role!));
  };

  public override getById(id: string, skipCache: boolean = false): Observable<RoleDto> {
    return super.getById(id, skipCache);
  }

  protected createFunction = (role: RoleDto) => {
    const command = new CreateRoleCommand();
    command.init({ ...role });
    return this.apiRolesClient.create(command).pipe(map((response) => response.id));
  };

  public override create(role: RoleDto): Observable<RoleDto> {
    return super.create(role);
  }

  protected updateFunction = (role: RoleDto) => {
    const command = new UpdateRoleCommand();
    command.init({ ...role });
    return this.apiRolesClient.update(role.id, command);
  };

  public override update(role: RoleDto): Observable<RoleDto> {
    return super.update(role);
  }

  protected deleteFunction = (id: string) => {
    const command = new DeleteRoleCommand();
    return this.apiRolesClient.delete(id, command);
  };

  public override delete(id: string): Observable<void> {
    return super.delete(id);
  }
}
