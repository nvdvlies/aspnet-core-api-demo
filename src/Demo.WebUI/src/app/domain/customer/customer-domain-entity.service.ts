import { Injectable, OnDestroy } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { combineLatest, Observable, of } from 'rxjs';
import { debounceTime, map } from 'rxjs/operators';
import { CustomerDto, ICustomerDto } from '@api/api.generated.clients';
import { CustomerStoreService } from '@domain/customer/customer-store.service';
import {
  DomainEntityBase,
  DomainEntityContext,
  IDomainEntityContext,
  InitFromRouteOptions
} from '@domain/shared/domain-entity-base';
import { Permission } from '@shared/enums/permission.enum';
import { UserPermissionService } from '@shared/services/user-permission.service';

export interface ICustomerDomainEntityContext extends IDomainEntityContext<CustomerDto> {}

export class CustomerDomainEntityContext
  extends DomainEntityContext<CustomerDto>
  implements ICustomerDomainEntityContext
{
  constructor() {
    super();
  }
}

type CustomerControls = { [key in keyof ICustomerDto]-?: AbstractControl };
export type CustomerFormGroup = FormGroup & { controls: CustomerControls };

@Injectable()
export class CustomerDomainEntityService
  extends DomainEntityBase<CustomerDto>
  implements OnDestroy
{
  protected createFunction = (customer: CustomerDto) => this.customerStoreService.create(customer);
  protected readFunction = (id?: string) => this.customerStoreService.getById(id!);
  protected updateFunction = (customer: CustomerDto) => this.customerStoreService.update(customer);
  protected deleteFunction = (id?: string) => this.customerStoreService.delete(id!);
  protected entityUpdatedEvent$ = this.customerStoreService.customerUpdatedInStore$;
  protected readPermission?: keyof typeof Permission = 'CustomersRead';
  protected writePermission?: keyof typeof Permission = 'CustomersWrite';

  public observe$: Observable<CustomerDomainEntityContext> = combineLatest([
    this.observeInternal$
  ]).pipe(
    debounceTime(0),
    map(([baseContext]) => {
      const context: CustomerDomainEntityContext = {
        ...baseContext
      };
      return context;
    })
  );

  constructor(
    route: ActivatedRoute,
    userPermissionService: UserPermissionService,
    private readonly customerStoreService: CustomerStoreService
  ) {
    super(route, userPermissionService);
    super.init();
  }

  public override get form(): CustomerFormGroup {
    return super.form as CustomerFormGroup;
  }
  public override set form(value: CustomerFormGroup) {
    super.form = value;
  }

  protected instantiateForm(): void {
    this.form = this.buildFormGroup();
  }

  private buildFormGroup(): CustomerFormGroup {
    const controls: CustomerControls = {
      id: new FormControl(super.readonlyFormState),
      code: new FormControl(super.readonlyFormState),
      name: new FormControl(
        null,
        [Validators.required, Validators.minLength(2), Validators.maxLength(200)],
        []
      ),
      deleted: new FormControl(super.readonlyFormState),
      deletedBy: new FormControl(super.readonlyFormState),
      deletedOn: new FormControl(super.readonlyFormState),
      createdBy: new FormControl(super.readonlyFormState),
      createdOn: new FormControl(super.readonlyFormState),
      lastModifiedBy: new FormControl(super.readonlyFormState),
      lastModifiedOn: new FormControl(super.readonlyFormState),
      xmin: new FormControl(super.readonlyFormState)
    };
    return new FormGroup(controls) as CustomerFormGroup;
  }

  protected instantiateNewEntity(): Observable<CustomerDto> {
    return of(new CustomerDto());
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

  public override create(): Observable<CustomerDto> {
    return super.create();
  }

  public override update(): Observable<CustomerDto> {
    return super.update();
  }

  public override upsert(): Observable<CustomerDto> {
    return super.upsert();
  }

  public override delete(): Observable<void> {
    return super.delete();
  }

  public override reset(): void {
    super.reset();
  }
}
