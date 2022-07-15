import { Injectable, OnDestroy } from '@angular/core';
import { AbstractControl, FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
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

type InvoiceControls = { [key in keyof IInvoiceDto]-?: AbstractControl };
export type InvoiceFormGroup = FormGroup & { controls: InvoiceControls };

type InvoiceLineControls = { [key in keyof IInvoiceLineDto]-?: AbstractControl };
export type InvoiceLineFormGroup = FormGroup & {
  controls: InvoiceLineControls;
};

export type InvoiceLineFormArray = FormArray & {
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
      id: new FormControl(super.readonlyFormState),
      invoiceNumber: new FormControl(super.readonlyFormState),
      customerId: new FormControl(null, [Validators.required], []),
      invoiceDate: new FormControl(null, [Validators.required], []),
      paymentTerm: new FormControl(null, [Validators.required], []),
      orderReference: new FormControl(null, [Validators.required], []),
      status: new FormControl(super.readonlyFormState),
      pdfIsSynced: new FormControl(super.readonlyFormState),
      pdfDataChecksum: new FormControl(super.readonlyFormState),
      invoiceLines: new FormArray(
        [] as InvoiceLineFormGroup[],
        [Validators.required],
        []
      ) as InvoiceLineFormArray,
      deleted: new FormControl(super.readonlyFormState),
      deletedBy: new FormControl(super.readonlyFormState),
      deletedOn: new FormControl(super.readonlyFormState),
      createdBy: new FormControl(super.readonlyFormState),
      createdOn: new FormControl(super.readonlyFormState),
      lastModifiedBy: new FormControl(super.readonlyFormState),
      lastModifiedOn: new FormControl(super.readonlyFormState),
      xmin: new FormControl(super.readonlyFormState)
    };
    return new FormGroup(controls) as InvoiceFormGroup;
  }

  private buildInvoiceLineFormGroup(): InvoiceLineFormGroup {
    const controls: InvoiceLineControls = {
      id: new FormControl(super.readonlyFormState),
      lineNumber: new FormControl(super.readonlyFormState),
      quantity: new FormControl(null, [Validators.required], []),
      sellingPrice: new FormControl(null, [Validators.required], []),
      description: new FormControl(null, [Validators.required], []),
      xmin: new FormControl(super.readonlyFormState)
    };
    return new FormGroup(controls) as InvoiceLineFormGroup;
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

  private get invoiceLineFormArray(): FormArray {
    return this.form.controls.invoiceLines as FormArray;
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
