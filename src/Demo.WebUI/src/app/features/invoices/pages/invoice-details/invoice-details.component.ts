import { ChangeDetectionStrategy, Component, HostListener } from '@angular/core';
import { Router } from '@angular/router';
import {
  BehaviorSubject,
  combineLatest,
  debounceTime,
  EMPTY,
  finalize,
  map,
  Observable,
  switchMap,
  tap
} from 'rxjs';
import { DomainEntityService } from '@domain/shared/domain-entity-base';
import {
  InvoiceDomainEntityService,
  InvoiceFormGroup,
  IInvoiceDomainEntityContext,
  InvoiceLineFormArray
} from '@domain/invoice/invoice-domain-entity.service';
import { IHasForm } from '@shared/guards/unsaved-changes.guard';
import { InvoiceListRouteState } from '@invoices/pages/invoice-list/invoice-list.component';
import { ModalService } from '@shared/services/modal.service';
import { ConfirmDeleteModalComponent } from '@shared/modals/confirm-delete-modal/confirm-delete-modal.component';
import { InvoiceStatusEnum } from '@api/api.generated.clients';

interface ViewModel extends IInvoiceDomainEntityContext {
  isMarkingAsSent: boolean;
  isMarkingAsCancelled: boolean;
  isMarkingAsPaid: boolean;
}

@Component({
  templateUrl: './invoice-details.component.html',
  styleUrls: ['./invoice-details.component.scss'],
  providers: [
    InvoiceDomainEntityService,
    {
      provide: DomainEntityService,
      useExisting: InvoiceDomainEntityService
    }
  ],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class InvoiceDetailsComponent implements IHasForm {
  public initFromRoute$ = this.invoiceDomainEntityService.initFromRoute();

  public readonly isMarkingAsSent = new BehaviorSubject<boolean>(false);
  public readonly isMarkingAsCancelled = new BehaviorSubject<boolean>(false);
  public readonly isMarkingAsPaid = new BehaviorSubject<boolean>(false);

  public isMarkingAsSent$ = this.isMarkingAsSent.asObservable();
  public isMarkingAsCancelled$ = this.isMarkingAsCancelled.asObservable();
  public isMarkingAsPaid$ = this.isMarkingAsPaid.asObservable();

  public InvoiceStatusEnum = InvoiceStatusEnum;

  private vm: Readonly<ViewModel> | undefined;

  public vm$: Observable<ViewModel> = combineLatest([
    this.invoiceDomainEntityService.observe$,
    this.isMarkingAsSent$,
    this.isMarkingAsCancelled$,
    this.isMarkingAsPaid$
  ]).pipe(
    debounceTime(0),
    map(([domainEntityContext, isMarkingAsSent, isMarkingAsCancelled, isMarkingAsPaid]) => {
      const context: ViewModel = {
        ...domainEntityContext,
        isMarkingAsSent,
        isMarkingAsCancelled,
        isMarkingAsPaid
      };
      return context;
    }),
    tap((vm) => (this.vm = vm))
  );

  public form: InvoiceFormGroup = this.invoiceDomainEntityService.form;
  public get invoiceLines(): InvoiceLineFormArray {
    return this.form.controls.invoiceLines as InvoiceLineFormArray;
  }

  constructor(
    private readonly router: Router,
    private readonly invoiceDomainEntityService: InvoiceDomainEntityService,
    private readonly modalService: ModalService
  ) {}

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
    this.modalService
      .confirmWithModal(ConfirmDeleteModalComponent)
      .pipe(
        switchMap((confirmed) => (confirmed ? this.invoiceDomainEntityService.delete() : EMPTY))
      )
      .subscribe(() => {
        this.router.navigateByUrl('/invoices');
      });
  }

  public markAsSent(): void {
    this.modalService
      .confirmWithMessage('Are you sure you want to mark this invoice as sent?', 'Mark as sent')
      .pipe(
        switchMap((confirmed) => {
          if (confirmed) {
            this.isMarkingAsSent.next(true);
            return this.invoiceDomainEntityService.markAsSent();
          } else {
            return EMPTY;
          }
        }),
        finalize(() => this.isMarkingAsSent.next(false))
      )
      .subscribe();
  }

  public markAsCancelled(): void {
    this.modalService
      .confirmWithMessage(
        'Are you sure you want to mark this invoice as cancelled?',
        'Mark as cancelled'
      )
      .pipe(
        switchMap((confirmed) => {
          if (confirmed) {
            this.isMarkingAsCancelled.next(true);
            return this.invoiceDomainEntityService.markAsCancelled();
          } else {
            return EMPTY;
          }
        }),
        finalize(() => this.isMarkingAsCancelled.next(false))
      )
      .subscribe();
  }

  public markAsPaid(): void {
    this.modalService
      .confirmWithMessage('Are you sure you want to mark this invoice as paid?', 'Mark as paid')
      .pipe(
        switchMap((confirmed) => {
          if (confirmed) {
            this.isMarkingAsPaid.next(true);
            return this.invoiceDomainEntityService.markAsPaid();
          } else {
            return EMPTY;
          }
        }),
        finalize(() => this.isMarkingAsPaid.next(false))
      )
      .subscribe();
  }

  public resolveMergeConflictWithTakeTheirs(): void {
    this.invoiceDomainEntityService.resolveMergeConflictWithTakeTheirs();
  }

  @HostListener('document:keydown.shift.alt.s', ['$event'])
  public saveShortcut(event: KeyboardEvent) {
    this.form.markAllAsTouched();
    this.save();
    event.preventDefault();
  }

  @HostListener('document:keydown.shift.alt.d', ['$event'])
  public deleteShortcut(event: KeyboardEvent) {
    if (!this.vm?.id) {
      return;
    }
    this.delete();
    event.preventDefault();
  }

  @HostListener('document:keydown.shift.alt.c', ['$event'])
  public closeShortcut(event: KeyboardEvent) {
    this.router.navigateByUrl('/invoices');
    event.preventDefault();
  }
}
