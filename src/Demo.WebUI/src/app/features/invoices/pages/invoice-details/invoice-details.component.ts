import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import {
  BehaviorSubject,
  combineLatest,
  debounceTime,
  forkJoin,
  map,
  Observable,
  of,
  switchMap,
  tap
} from 'rxjs';
import { BaseDomainEntityService } from '@domain/shared/domain-entity-base';
import {
  InvoiceDomainEntityService,
  InvoiceFormGroup,
  IInvoiceDomainEntityContext,
  InvoiceLineFormArray
} from '@domain/invoice/invoice-domain-entity.service';
import { IHasForm } from '@shared/guards/unsaved-changes.guard';
import { InvoiceDto } from '@api/api.generated.clients';
import { InvoiceListRouteState } from '@invoices/pages/invoice-list/invoice-list.component';

interface ViewModel extends IInvoiceDomainEntityContext {
  createdByFullname: string | undefined;
  lastModifiedByFullname: string | undefined;
}

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

  private readonly createdByFullname = new BehaviorSubject<string | undefined>(undefined);
  private readonly lastModifiedByFullname = new BehaviorSubject<string | undefined>(undefined);

  private createdByFullname$ = this.createdByFullname.asObservable();
  private lastModifiedByFullname$ = this.lastModifiedByFullname.asObservable();

  public vm$ = combineLatest([
    this.invoiceDomainEntityService.observe$,
    this.createdByFullname$,
    this.lastModifiedByFullname$
  ]).pipe(
    debounceTime(0),
    map(([domainEntityContext, createdByFullname, lastModifiedByFullname]) => {
      return {
        ...domainEntityContext,
        createdByFullname,
        lastModifiedByFullname
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

  public ngOnInit(): void {
    this.invoiceDomainEntityService.afterGetByIdHook = this.loadAdditionalData.bind(this);
  }

  private loadAdditionalData(invoice: InvoiceDto): Observable<null> {
    return forkJoin([
      of([
        { id: invoice.createdBy, fullName: 'John Doe' },
        { id: invoice.lastModifiedBy, fullName: 'Jane Doe' }
      ]) // this.usersQuickSearch.getByIds([invoice.createdBy, invoice.lastModifiedBy])
    ]).pipe(
      tap(([users]) => {
        this.createdByFullname.next(
          users.find((x) => x.id?.toLowerCase() === invoice.createdBy?.toLowerCase())?.fullName ??
            undefined
        );
        this.lastModifiedByFullname.next(
          users.find((x) => x.id?.toLowerCase() === invoice.lastModifiedBy?.toLowerCase())
            ?.fullName ?? undefined
        );
      }),
      switchMap(() => of(null))
    );
  }

  public save(): void {
    if (!this.form.valid) {
      //this.form.markAllAsTouched();
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
