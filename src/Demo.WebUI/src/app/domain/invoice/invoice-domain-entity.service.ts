import { Injectable, OnDestroy } from '@angular/core';
import {
  AbstractControl,
  UntypedFormArray,
  UntypedFormControl,
  UntypedFormGroup,
  Validators
} from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { BehaviorSubject, combineLatest, Observable, of, throwError } from 'rxjs';
import { debounceTime, map } from 'rxjs/operators';
import {
  InvoiceDto,
  IInvoiceDto,
  IInvoiceLineDto,
  InvoiceLineDto,
  ApiInvoicesClient,
  CopyInvoiceCommand,
  InvoiceStatusEnum
} from '@api/api.generated.clients';
import { InvoiceStoreService } from '@domain/invoice/invoice-store.service';
import {
  DomainEntityBase,
  DomainEntityContext,
  IDomainEntityContext,
  InitFromRouteOptions
} from '@domain/shared/domain-entity-base';
import { Permission } from '@shared/enums/permission.enum';
import { UserPermissionService } from '@shared/services/user-permission.service';
import { ExtendedAbstractControl } from '@domain/shared/form-controls-base';

export interface IInvoiceDomainEntityContext extends IDomainEntityContext<InvoiceDto> {
  canEditInvoiceContent: boolean;
}

export class InvoiceDomainEntityContext
  extends DomainEntityContext<InvoiceDto>
  implements IInvoiceDomainEntityContext
{
  public canEditInvoiceContent: boolean = true;

  constructor() {
    super();
  }
}

type InvoiceControls = { [key in keyof IInvoiceDto]-?: ExtendedAbstractControl };
export type InvoiceFormGroup = UntypedFormGroup & { controls: InvoiceControls };

type InvoiceLineControls = { [key in keyof IInvoiceLineDto]-?: ExtendedAbstractControl };
export type InvoiceLineFormGroup = UntypedFormGroup & {
  controls: InvoiceLineControls;
};

export type InvoiceLineFormArray = UntypedFormArray & {
  controls: InvoiceLineFormGroup[];
};

@Injectable()
export class InvoiceDomainEntityService extends DomainEntityBase<InvoiceDto> implements OnDestroy {
  protected createFunction = (invoice: InvoiceDto) => this.invoiceStoreService.create(invoice);
  protected readFunction = (id?: string) => this.invoiceStoreService.getById(id!);
  protected updateFunction = (invoice: InvoiceDto) => this.invoiceStoreService.update(invoice);
  protected deleteFunction = (id?: string) => this.invoiceStoreService.delete(id!);
  protected entityUpdatedEvent$ = this.invoiceStoreService.invoiceUpdatedInStore$;
  protected readPermission?: keyof typeof Permission = 'InvoicesRead';
  protected writePermission?: keyof typeof Permission = 'InvoicesWrite';

  protected readonly canEditInvoiceContent = new BehaviorSubject<boolean>(true);

  protected canEditInvoiceContent$ = this.canEditInvoiceContent.asObservable();

  public observe$: Observable<InvoiceDomainEntityContext> = combineLatest([
    this.observeInternal$,
    this.canEditInvoiceContent$
  ]).pipe(
    debounceTime(0),
    map(([baseContext, canEditInvoiceContent]) => {
      const context: InvoiceDomainEntityContext = {
        ...baseContext,
        canEditInvoiceContent
      };
      return context;
    })
  );

  constructor(
    route: ActivatedRoute,
    userPermissionService: UserPermissionService,
    private readonly invoiceStoreService: InvoiceStoreService,
    private readonly apiInvoicesClient: ApiInvoicesClient
  ) {
    super(route, userPermissionService);
    super.init();
  }

  public override get form(): InvoiceFormGroup {
    return super.form as InvoiceFormGroup;
  }
  public override set form(value: InvoiceFormGroup) {
    super.form = value;
  }

  protected instantiateForm(): void {
    this.form = this.buildInvoiceFormGroup();
  }

  private buildInvoiceFormGroup(): InvoiceFormGroup {
    const controls: InvoiceControls = {
      id: new UntypedFormControl(super.readonlyFormState),
      invoiceNumber: new UntypedFormControl(super.readonlyFormState),
      customerId: new UntypedFormControl(null, [Validators.required], []),
      invoiceDate: new UntypedFormControl(null, [Validators.required], []),
      paymentTerm: new UntypedFormControl(null, [Validators.required], []),
      orderReference: new UntypedFormControl(null, [Validators.required], []),
      status: new UntypedFormControl(super.readonlyFormState),
      pdfIsSynced: new UntypedFormControl(super.readonlyFormState),
      pdfDataChecksum: new UntypedFormControl(super.readonlyFormState),
      invoiceLines: new UntypedFormArray(
        [] as InvoiceLineFormGroup[],
        [Validators.required],
        []
      ) as InvoiceLineFormArray,
      deleted: new UntypedFormControl(super.readonlyFormState),
      deletedBy: new UntypedFormControl(super.readonlyFormState),
      deletedOn: new UntypedFormControl(super.readonlyFormState),
      createdBy: new UntypedFormControl(super.readonlyFormState),
      createdOn: new UntypedFormControl(super.readonlyFormState),
      lastModifiedBy: new UntypedFormControl(super.readonlyFormState),
      lastModifiedOn: new UntypedFormControl(super.readonlyFormState),
      xmin: new UntypedFormControl(super.readonlyFormState)
    };
    return new UntypedFormGroup(controls) as InvoiceFormGroup;
  }

  private buildInvoiceLineFormGroup(): InvoiceLineFormGroup {
    const controls: InvoiceLineControls = {
      id: new UntypedFormControl(super.readonlyFormState),
      lineNumber: new UntypedFormControl(super.readonlyFormState),
      quantity: new UntypedFormControl(null, [Validators.required], []),
      sellingPrice: new UntypedFormControl(null, [Validators.required], []),
      description: new UntypedFormControl(null, [Validators.required], []),
      xmin: new UntypedFormControl(super.readonlyFormState)
    };
    return new UntypedFormGroup(controls) as InvoiceLineFormGroup;
  }

  public addInvoiceLine(): void {
    const newInvoiceLineFormGroup = this.buildInvoiceLineFormGroup();
    this.invoiceLineFormArray.push(newInvoiceLineFormGroup);
  }

  public removeInvoiceLine(index: number): void {
    if (this.invoiceLineFormArray.length > 1) {
      this.invoiceLineFormArray.removeAt(index);
      if (!this.form.dirty) {
        this.form.markAsDirty();
      }
    }
  }

  protected instantiateNewEntity(): Observable<InvoiceDto> {
    const invoice = new InvoiceDto();
    invoice.status = InvoiceStatusEnum.Draft;
    invoice.invoiceDate = new Date();
    invoice.paymentTerm = 30;
    const invoiceLine = new InvoiceLineDto();
    invoice.invoiceLines = [invoiceLine];
    return of(invoice);
  }

  public get invoiceLines(): InvoiceLineFormGroup[] {
    return this.invoiceLineFormArray.controls as InvoiceLineFormGroup[];
  }

  private get invoiceLineFormArray(): UntypedFormArray {
    return this.form.controls.invoiceLines as UntypedFormArray;
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

  public override create(): Observable<InvoiceDto> {
    return super.create();
  }

  public override update(): Observable<InvoiceDto> {
    return super.update();
  }

  public override upsert(): Observable<InvoiceDto> {
    return super.upsert();
  }

  public markAsSent(): Observable<InvoiceDto> {
    return super.update((invoice: InvoiceDto) => this.invoiceStoreService.markAsSent(invoice));
  }

  public markAsPaid(): Observable<InvoiceDto> {
    return super.update((invoice: InvoiceDto) => this.invoiceStoreService.markAsPaid(invoice));
  }

  public markAsCancelled(): Observable<InvoiceDto> {
    return super.update((invoice: InvoiceDto) => this.invoiceStoreService.markAsCancelled(invoice));
  }

  public copy(): Observable<string> {
    if (!this.id.value) {
      return throwError(() => new Error('Id is not set.'));
    }
    const command = new CopyInvoiceCommand();
    return this.apiInvoicesClient.copy(this.id.value, command).pipe(map((x) => x.id));
  }

  public override delete(): Observable<void> {
    return super.delete();
  }

  protected override afterPatchEntityToFormHook(invoice: InvoiceDto): void {
    // form.patchValue doesnt modify FormArray structure, so we need to do this manually afterwards.
    this.patchInvoiceLinesToForm(invoice);

    const canEditInvoiceContent = invoice.status === InvoiceStatusEnum.Draft;
    if (!canEditInvoiceContent) {
      this.disableInvoiceContentControls();
    }
    this.canEditInvoiceContent.next(canEditInvoiceContent);
  }

  private patchInvoiceLinesToForm(invoice: InvoiceDto): void {
    this.invoiceLineFormArray.clear();
    invoice.invoiceLines?.forEach((invoiceLine) => {
      const invoiceLineFormGroup = this.buildInvoiceLineFormGroup();
      invoiceLineFormGroup.patchValue({ ...invoiceLine });
      this.invoiceLineFormArray.push(invoiceLineFormGroup);
    });
  }

  private disableInvoiceContentControls(): void {
    this.form.controls.invoiceDate?.disable();
    this.form.controls.customerId?.disable();
    this.form.controls.orderReference?.disable();
    this.invoiceLineFormArray.disable();
    this.invoiceLines.forEach((invoiceLine) => {
      invoiceLine.controls.quantity.disable();
      invoiceLine.controls.description?.disable();
      invoiceLine.controls.sellingPrice.disable();
    });
  }

  public override reset(): void {
    super.reset();
  }
}
