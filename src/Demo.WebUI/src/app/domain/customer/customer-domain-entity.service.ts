import { Injectable, OnDestroy } from '@angular/core';
import { AbstractControl, UntypedFormControl, UntypedFormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { combineLatest, Observable, of } from 'rxjs';
import { debounceTime, map } from 'rxjs/operators';
import { CustomerDto, ICustomerDto, ILocationDto, LocationDto } from '@api/api.generated.clients';
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
export type CustomerFormGroup = UntypedFormGroup & { controls: CustomerControls };

type LocationControls = {
  [key in keyof ILocationDto]-?: AbstractControl;
};
export type LocationFormGroup = UntypedFormGroup & {
  controls: LocationControls;
};

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
      id: new UntypedFormControl(super.readonlyFormState),
      code: new UntypedFormControl(super.readonlyFormState),
      name: new UntypedFormControl(
        null,
        [Validators.required, Validators.minLength(2), Validators.maxLength(200)],
        []
      ),
      addressId: new UntypedFormControl(null),
      address: new UntypedFormGroup({
        displayName: new UntypedFormControl(super.readonlyFormState),
        streetName: new UntypedFormControl(super.readonlyFormState),
        houseNumber: new UntypedFormControl(super.readonlyFormState),
        postalCode: new UntypedFormControl(super.readonlyFormState),
        city: new UntypedFormControl(super.readonlyFormState),
        countryCode: new UntypedFormControl(super.readonlyFormState),
        latitude: new UntypedFormControl({ value: 0, disabled: true }),
        longitude: new UntypedFormControl({ value: 0, disabled: true })
      } as LocationControls) as LocationFormGroup,
      deleted: new UntypedFormControl(super.readonlyFormState),
      deletedBy: new UntypedFormControl(super.readonlyFormState),
      deletedOn: new UntypedFormControl(super.readonlyFormState),
      createdBy: new UntypedFormControl(super.readonlyFormState),
      createdOn: new UntypedFormControl(super.readonlyFormState),
      lastModifiedBy: new UntypedFormControl(super.readonlyFormState),
      lastModifiedOn: new UntypedFormControl(super.readonlyFormState),
      xmin: new UntypedFormControl(super.readonlyFormState)
    };
    return new UntypedFormGroup(controls) as CustomerFormGroup;
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

  public setAddress(location: ILocationDto): void {
    this.form.controls.address.patchValue({
      ...location
    });
  }

  public removeAddress(): void {
    if (this.form.controls.addressId.value) {
      this.form.controls.addressId.setValue(null);
    }
    this.form.controls.address.reset({
      latitude: 0,
      longitude: 0
    } as ILocationDto);
  }
}
