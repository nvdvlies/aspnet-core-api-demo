import { Injectable, OnDestroy } from '@angular/core';
import {
  AbstractControl,
  FormArray,
  FormControl,
  FormGroup,
  ValidatorFn,
  Validators
} from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { combineLatest, Observable, of, throwError } from 'rxjs';
import { debounceTime, map } from 'rxjs/operators';
import {
  InvoiceDto,
  IInvoiceDto,
  IInvoiceLineDto,
  InvoiceLineDto,
  ApiInvoicesClient,
  CopyInvoiceCommand
} from '@api/api.generated.clients';
import { InvoiceStoreService } from '@domain/invoice/invoice-store.service';
import {
  DomainEntityBase,
  DomainEntityContext,
  IDomainEntityContext,
  InitFromRouteOptions
} from '@domain/shared/domain-entity-base';

export interface IInvoiceDomainEntityContext extends IDomainEntityContext<InvoiceDto> {}

export class InvoiceDomainEntityContext
  extends DomainEntityContext<InvoiceDto>
  implements IInvoiceDomainEntityContext
{
  constructor() {
    super();
  }
}

type InvoiceControls = { [key in keyof IInvoiceDto]: AbstractControl };
export type InvoiceFormGroup = FormGroup & { controls: InvoiceControls };

type InvoiceLineControls = { [key in keyof IInvoiceLineDto]: AbstractControl };
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

  public observe$ = combineLatest([this.observeInternal$]).pipe(
    debounceTime(0),
    map(([context]) => {
      return {
        ...context
      } as InvoiceDomainEntityContext;
    })
  ) as Observable<InvoiceDomainEntityContext>;

  constructor(
    route: ActivatedRoute,
    private readonly invoiceStoreService: InvoiceStoreService,
    private readonly apiInvoicesClient: ApiInvoicesClient
  ) {
    super(route);
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
    return new FormGroup({
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
      timestamp: new FormControl(super.readonlyFormState)
    } as InvoiceControls) as InvoiceFormGroup;
  }

  private buildInvoiceLineFormGroup(): InvoiceLineFormGroup {
    return new FormGroup({
      id: new FormControl(super.readonlyFormState),
      lineNumber: new FormControl(super.readonlyFormState),
      quantity: new FormControl(null, [Validators.required], []),
      sellingPrice: new FormControl(null, [Validators.required], []),
      description: new FormControl(null, [Validators.required], []),
      timestamp: new FormControl(super.readonlyFormState)
    } as InvoiceLineControls) as InvoiceLineFormGroup;
  }

  public addInvoiceLine(): void {
    const newInvoiceLineFormGroup = this.buildInvoiceLineFormGroup();
    this.invoiceLineFormArray.push(newInvoiceLineFormGroup);
  }

  public removeInvoiceLine(index: number): void {
    if (this.invoiceLineFormArray.length > 1) {
      this.invoiceLineFormArray.removeAt(index);
    }
  }

  protected instantiateNewEntity(): Observable<InvoiceDto> {
    const invoice = new InvoiceDto();
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
    return super.update(this.invoiceStoreService.markAsSent);
  }

  public markAsPaid(): Observable<InvoiceDto> {
    return super.update(this.invoiceStoreService.markAsPaid);
  }

  public markAsCancelled(): Observable<InvoiceDto> {
    return super.update(this.invoiceStoreService.markAsCancelled);
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
  }

  private patchInvoiceLinesToForm(invoice: InvoiceDto): void {
    this.invoiceLineFormArray.clear();
    invoice.invoiceLines?.forEach((invoiceLine) => {
      const invoiceLineFormGroup = this.buildInvoiceLineFormGroup();
      invoiceLineFormGroup.patchValue({ ...invoiceLine });
      this.invoiceLineFormArray.push(invoiceLineFormGroup);
    });
  }

  public override reset(): void {
    super.reset();
  }
}
