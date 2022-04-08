import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { combineLatest, debounceTime, map, Observable } from 'rxjs';
import { BaseDomainEntityService } from '@domain/shared/domain-entity-base';
import {
  InvoiceDomainEntityService,
  InvoiceFormGroup,
  IInvoiceDomainEntityContext,
  InvoiceLineFormArray
} from '@domain/invoice/invoice-domain-entity.service';
import { IHasForm } from '@shared/guards/unsaved-changes.guard';
import { InvoiceListRouteState } from '@invoices/pages/invoice-list/invoice-list.component';

interface ViewModel extends IInvoiceDomainEntityContext {}

@Component({
  templateUrl: './invoice-details.component.html',
  styleUrls: ['./invoice-details.component.scss'],
  providers: [
    InvoiceDomainEntityService,
    {
      provide: BaseDomainEntityService,
      useExisting: InvoiceDomainEntityService
    }
  ],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class InvoiceDetailsComponent implements OnInit, IHasForm {
  public initFromRoute$ = this.invoiceDomainEntityService.initFromRoute();

  public vm$ = combineLatest([this.invoiceDomainEntityService.observe$]).pipe(
    debounceTime(0),
    map(([domainEntityContext]) => {
      return {
        ...domainEntityContext
      } as ViewModel;
    })
  ) as Observable<ViewModel>;

  public form: InvoiceFormGroup = this.invoiceDomainEntityService.form;
  public get invoiceLines(): InvoiceLineFormArray {
    return this.form.controls.invoiceLines as InvoiceLineFormArray;
  }

  constructor(
    private readonly router: Router,
    private readonly invoiceDomainEntityService: InvoiceDomainEntityService
  ) {}

  public ngOnInit(): void {}

  public save(): void {
    if (!this.form.valid) {
      return;
    }

    this.invoiceDomainEntityService.upsert().subscribe((invoice) => {
      this.router.navigateByUrl('/invoices', {
        state: { spotlightIdentifier: invoice.id } as InvoiceListRouteState
      });
    });
  }

  public addInvoiceLine(): void {
    this.invoiceDomainEntityService.addInvoiceLine();
  }

  public removeInvoiceLine(index: number): void {
    this.invoiceDomainEntityService.removeInvoiceLine(index);
  }

  public delete(): void {
    this.invoiceDomainEntityService.deleteWithConfirmation().subscribe(() => {
      this.router.navigateByUrl('/invoices');
    });
  }

  public resolveMergeConflictWithTakeTheirs(): void {
    this.invoiceDomainEntityService.resolveMergeConflictWithTakeTheirs();
  }
}
