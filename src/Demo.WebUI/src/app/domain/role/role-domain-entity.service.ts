import { Injectable, OnDestroy } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { combineLatest, Observable, of } from 'rxjs';
import { debounceTime, map } from 'rxjs/operators';
import { RoleDto, IRoleDto } from '@api/api.generated.clients';
import { RoleStoreService } from '@domain/role/role-store.service';
import {
  DomainEntityBase,
  DomainEntityContext,
  IDomainEntityContext,
  InitFromRouteOptions
} from '@domain/shared/domain-entity-base';

export interface IRoleDomainEntityContext extends IDomainEntityContext<RoleDto> {}

export class RoleDomainEntityContext
  extends DomainEntityContext<RoleDto>
  implements IRoleDomainEntityContext
{
  constructor() {
    super();
  }
}

type RoleControls = { [key in keyof IRoleDto]: AbstractControl };
export type RoleFormGroup = FormGroup & { controls: RoleControls };

@Injectable()
export class RoleDomainEntityService extends DomainEntityBase<RoleDto> implements OnDestroy {
  protected createFunction = (role: RoleDto) => this.roleStoreService.create(role);
  protected readFunction = (id?: string) => this.roleStoreService.getById(id!);
  protected updateFunction = (role: RoleDto) => this.roleStoreService.update(role);
  protected deleteFunction = (id?: string) => this.roleStoreService.delete(id!);
  protected entityUpdatedEvent$ = this.roleStoreService.roleUpdatedInStore$;

  public observe$ = combineLatest([this.observeInternal$]).pipe(
    debounceTime(0),
    map(([context]) => {
      return {
        ...context
      } as RoleDomainEntityContext;
    })
  ) as Observable<RoleDomainEntityContext>;

  constructor(route: ActivatedRoute, private readonly roleStoreService: RoleStoreService) {
    super(route);
    super.init();
  }

  public override get form(): RoleFormGroup {
    return super.form as RoleFormGroup;
  }
  public override set form(value: RoleFormGroup) {
    super.form = value;
  }

  protected instantiateForm(): void {
    this.form = this.buildFormGroup();
  }

  private buildFormGroup(): RoleFormGroup {
    return new FormGroup({
      id: new FormControl(super.readonlyFormState),
      name: new FormControl(null, [Validators.required, Validators.maxLength(50)], []),
      deleted: new FormControl(super.readonlyFormState),
      deletedBy: new FormControl(super.readonlyFormState),
      deletedOn: new FormControl(super.readonlyFormState),
      createdBy: new FormControl(super.readonlyFormState),
      createdOn: new FormControl(super.readonlyFormState),
      lastModifiedBy: new FormControl(super.readonlyFormState),
      lastModifiedOn: new FormControl(super.readonlyFormState),
      timestamp: new FormControl(super.readonlyFormState)
    } as RoleControls) as RoleFormGroup;
  }

  protected instantiateNewEntity(): Observable<RoleDto> {
    return of(new RoleDto());
  }

  public override new(): Observable<null> {
    return super.new();
  }

  public override read(id: string): Observable<null> {
    return super.read(id);
  }

  public override initFromRoute(options?: InitFromRouteOptions | undefined): Observable<null> {
    return super.initFromRoute(options);
  }

  public override create(): Observable<RoleDto> {
    return super.create();
  }

  public override update(): Observable<RoleDto> {
    return super.update();
  }

  public override upsert(): Observable<RoleDto> {
    return super.upsert();
  }

  public override delete(): Observable<void> {
    return super.delete();
  }

  public override reset(): void {
    super.reset();
  }
}
