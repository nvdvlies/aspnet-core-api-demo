import { Injectable, OnDestroy } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

import { combineLatest, Observable } from 'rxjs';
import { debounceTime, map } from 'rxjs/operators';

import { InvoiceDto, IInvoiceDto, IInvoiceLineDto } from '@api/api.generated.clients';
import { InvoiceStoreService } from '@domain/invoice/invoice-store.service';
import { DomainEntityBase, DomainEntityContext, IDomainEntityContext, InitFromRouteOptions } from '@domain/shared/domain-entity-base';

export interface IInvoiceDomainEntityContext extends IDomainEntityContext<InvoiceDto> {

}

export class InvoiceDomainEntityContext extends DomainEntityContext<InvoiceDto> implements IInvoiceDomainEntityContext {
  constructor() {
    super();
  }
}

type InvoiceControls = { [key in keyof IInvoiceDto]: AbstractControl };
export type InvoiceFormGroup = FormGroup & { controls: InvoiceControls };  

type InvoiceLineControls = { [key in keyof IInvoiceLineDto]: AbstractControl };
export type InvoiceLineFormGroup = FormGroup & { controls: InvoiceLineControls };  

export type InvoiceLineFormArray = FormArray & { controls: InvoiceLineFormGroup[] }; 

@Injectable()
export class InvoiceDomainEntityService extends DomainEntityBase<InvoiceDto> implements OnDestroy {
  protected getByIdFunction = (id: string) => this.invoiceStoreService.getById(id);
  protected createFunction = (invoice: InvoiceDto) => this.invoiceStoreService.create(invoice);
  protected updateFunction = (invoice: InvoiceDto) => this.invoiceStoreService.update(invoice);
  protected deleteFunction = (id: string) => this.invoiceStoreService.delete(id);
  protected entityUpdatedEvent$ = this.invoiceStoreService.invoiceUpdatedInStore$;

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
        } as InvoiceDomainEntityContext;
      })
    ) as Observable<InvoiceDomainEntityContext>;

  constructor(
    route: ActivatedRoute,
    protected readonly formBuilder: FormBuilder,
    protected readonly invoiceStoreService: InvoiceStoreService
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

  public get invoiceLines(): InvoiceLineFormGroup[] {
    return (this.form.controls.invoiceLines as InvoiceLineFormArray).controls as InvoiceLineFormGroup[];
  }

  private get invoiceLineFormArray(): FormArray {
    return this.form.controls.invoiceLines as FormArray;
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

  public override create(): Observable<InvoiceDto> {
    return super.create();
  }

  public override update(): Observable<InvoiceDto> {
    return super.update();
  }

  public override upsert(): Observable<InvoiceDto> {
    return super.upsert();
  }

  public override delete(): Observable<void> {
    return super.delete();
  }

  protected instantiateForm(): void {
    this.form = this.buildInvoiceFormGroup();
  }

  private buildInvoiceFormGroup(): InvoiceFormGroup {
    return new FormGroup({
      id: new FormControl(),
      invoiceNumber: new FormControl(),
      customerId: new FormControl(),
      invoiceDate: new FormControl(),
      paymentTerm: new FormControl(),
      orderReference: new FormControl(),
      status: new FormControl(),
      pdfIsSynced: new FormControl(),
      pdfDataChecksum: new FormControl(),
      invoiceLines: new FormArray([] as InvoiceLineFormGroup[]) as InvoiceLineFormArray,
      deleted: new FormControl(),
      deletedBy: new FormControl(),
      deletedOn: new FormControl(),
      createdBy: new FormControl(),
      createdOn: new FormControl(),
      lastModifiedBy: new FormControl(),
      lastModifiedOn: new FormControl(),
      timestamp: new FormControl()
    } as InvoiceControls) as InvoiceFormGroup;
  }

  private buildInvoiceLineFormGroup(): InvoiceLineFormGroup {
    return new FormGroup({
      id: new FormControl(),
      lineNumber: new FormControl({ value: null, disabled: true }),
      quantity: new FormControl(null, [Validators.required], []),
      sellingPrice: new FormControl(),
      description: new FormControl(null, [Validators.required], []),
      timestamp: new FormControl()
    } as InvoiceLineControls) as InvoiceLineFormGroup;
  }

  public addInvoiceLine(): void {
    const newInvoiceLineFormGroup = this.buildInvoiceLineFormGroup();
    this.invoiceLineFormArray.push(newInvoiceLineFormGroup);
  }

  public removeInvoiceLine(index: number): void {
    this.invoiceLineFormArray.removeAt(index);
  }

  protected instantiateNewEntity(): InvoiceDto {
    const invoice = new InvoiceDto();
    invoice.invoiceLines = [];
    return invoice;
  }

  protected override afterPatchEntityToFormHook(invoice: InvoiceDto): void {
    // form.patchValue doesnt modify FormArray structure, so we need to do this manually afterwards.
    this.invoiceLineFormArray.clear();
    invoice.invoiceLines?.forEach(invoiceLine => {
      const invoiceLineFormGroup = this.buildInvoiceLineFormGroup();
      invoiceLineFormGroup.patchValue({...invoiceLine});
      this.invoiceLineFormArray.push(invoiceLineFormGroup);
    })
  };

  public markAsPaid(): Observable<InvoiceDto> {
    return super.update(this.invoiceStoreService.markAsPaid);
  }

  public override  reset(): void {
    super.reset()
  }
}
