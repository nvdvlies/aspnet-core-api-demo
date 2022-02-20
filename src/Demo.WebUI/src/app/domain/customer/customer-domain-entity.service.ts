import { Injectable, OnDestroy } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { combineLatest, Observable } from 'rxjs';
import { debounceTime, map } from 'rxjs/operators';
import { CustomerDto, ICustomerDto } from '@api/api.generated.clients';
import { CustomerStoreService } from '@domain/customer/customer-store.service';
import { DomainEntityBase, DomainEntityContext, IDomainEntityContext, InitFromRouteOptions } from '@domain/shared/domain-entity-base';
import { MatDialog } from '@angular/material/dialog';

export interface ICustomerDomainEntityContext extends IDomainEntityContext<CustomerDto> {

}

export class CustomerDomainEntityContext extends DomainEntityContext<CustomerDto> implements ICustomerDomainEntityContext {
  constructor() {
    super();
  }
}

type CustomerControls = { [key in keyof ICustomerDto]: AbstractControl };
export type CustomerFormGroup = FormGroup & { controls: CustomerControls };  

@Injectable()
export class CustomerDomainEntityService extends DomainEntityBase<CustomerDto> implements OnDestroy {
  protected getByIdFunction = (id: string) => this.customerStoreService.getById(id);
  protected createFunction = (customer: CustomerDto) => this.customerStoreService.create(customer);
  protected updateFunction = (customer: CustomerDto) => this.customerStoreService.update(customer);
  protected deleteFunction = (id: string) => this.customerStoreService.delete(id);
  protected entityUpdatedEvent$ = this.customerStoreService.customerUpdatedInStore$;

  public observe$ = combineLatest([
    this.observeInternal$
  ])
    .pipe(
      debounceTime(0),
      map(([
        context,
      ]) => {
        return {
          ...context
        } as CustomerDomainEntityContext;
      })
    ) as Observable<CustomerDomainEntityContext>;

  constructor(
    route: ActivatedRoute,
    matDialog: MatDialog,
    private readonly customerStoreService: CustomerStoreService
  ) {
    super(route, matDialog);
    super.init();
  }

  public override get form(): CustomerFormGroup {
    return super.form as CustomerFormGroup;
  }
  public override set form(value: CustomerFormGroup) {
    super.form = value;
  }

  protected instantiateForm(): void {
    this.form = this.buildInvoiceFormGroup();
  }

  private buildInvoiceFormGroup(): CustomerFormGroup {
    return new FormGroup({
      id: new FormControl(super.readonlyFormState),
      code: new FormControl(super.readonlyFormState),
      name: new FormControl(null, [Validators.required, Validators.minLength(2), Validators.maxLength(200)], []),
      deleted: new FormControl(super.readonlyFormState),
      deletedBy: new FormControl(super.readonlyFormState),
      deletedOn: new FormControl(super.readonlyFormState),
      createdBy: new FormControl(super.readonlyFormState),
      createdOn: new FormControl(super.readonlyFormState),
      lastModifiedBy: new FormControl(super.readonlyFormState),
      lastModifiedOn: new FormControl(super.readonlyFormState),
      timestamp: new FormControl(super.readonlyFormState)
    } as CustomerControls) as CustomerFormGroup;
  }

  protected instantiateNewEntity(): CustomerDto {
    return new CustomerDto();
  }

  public override new(): Observable<null> {
    return super.new();
  }

  public override getById(id: string): Observable<null> {
    return super.getById(id);
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

  public override deleteWithConfirmation(): Observable<void> {
    return super.deleteWithConfirmation();
  }

  public override delete(): Observable<void> {
    return super.delete();
  }

  public override getErrorMessage(errorKey: string, errorValue: any): string | undefined {
    switch (errorKey) {
      case 'some_custom_error_key': return 'Some custom error description';
      default:
        return super.getErrorMessage(errorKey, errorValue);
    }
  }

  public override reset(): void {
    super.reset();
  }
}
