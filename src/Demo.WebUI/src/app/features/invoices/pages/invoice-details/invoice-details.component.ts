import { ChangeDetectionStrategy, Component, HostListener } from '@angular/core';
import { Router } from '@angular/router';
import {
  BehaviorSubject,
  catchError,
  combineLatest,
  debounceTime,
  EMPTY,
  finalize,
  map,
  Observable,
  switchMap,
  tap,
  throwError
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
import {
  ApiException,
  ApiInvoicesClient,
  CopyInvoiceCommand,
  CopyInvoiceResponse,
  CreditInvoiceCommand,
  CreditInvoiceResponse,
  InvoiceStatusEnum,
  ProblemDetails,
  ValidationProblemDetails
} from '@api/api.generated.clients';
import { PermissionDeniedError } from '@shared/errors/permission-denied.error';

interface ViewModel extends IInvoiceDomainEntityContext {
  isMarkingAsSent: boolean;
  isMarkingAsCancelled: boolean;
  isMarkingAsPaid: boolean;
  isCopying: boolean;
  isCreatingCredit: boolean;
  copyProblemDetails:
    | ValidationProblemDetails
    | ProblemDetails
    | ApiException
    | PermissionDeniedError
    | undefined;
  creditProblemDetails:
    | ValidationProblemDetails
    | ProblemDetails
    | ApiException
    | PermissionDeniedError
    | undefined;
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
  public readonly isCopying = new BehaviorSubject<boolean>(false);
  public readonly isCreatingCredit = new BehaviorSubject<boolean>(false);
  public readonly copyProblemDetails = new BehaviorSubject<
    ValidationProblemDetails | ProblemDetails | ApiException | PermissionDeniedError | undefined
  >(undefined);
  public readonly creditProblemDetails = new BehaviorSubject<
    ValidationProblemDetails | ProblemDetails | ApiException | PermissionDeniedError | undefined
  >(undefined);

  public isMarkingAsSent$ = this.isMarkingAsSent.asObservable();
  public isMarkingAsCancelled$ = this.isMarkingAsCancelled.asObservable();
  public isMarkingAsPaid$ = this.isMarkingAsPaid.asObservable();
  public isCopying$ = this.isCopying.asObservable();
  public isCreatingCredit$ = this.isCreatingCredit.asObservable();
  protected copyProblemDetails$ = this.copyProblemDetails.asObservable();
  protected creditProblemDetails$ = this.creditProblemDetails.asObservable();

  public InvoiceStatusEnum = InvoiceStatusEnum;

  private vm: Readonly<ViewModel> | undefined;

  public vm$: Observable<ViewModel> = combineLatest([
    this.invoiceDomainEntityService.observe$,
    this.isMarkingAsSent$,
    this.isMarkingAsCancelled$,
    this.isMarkingAsPaid$,
    this.isCopying$,
    this.isCreatingCredit$,
    this.copyProblemDetails$,
    this.creditProblemDetails$
  ]).pipe(
    debounceTime(0),
    map(
      ([
        domainEntityContext,
        isMarkingAsSent,
        isMarkingAsCancelled,
        isMarkingAsPaid,
        isCopying,
        isCreatingCredit,
        copyProblemDetails,
        creditProblemDetails
      ]) => {
        const context: ViewModel = {
          ...domainEntityContext,
          isMarkingAsSent,
          isMarkingAsCancelled,
          isMarkingAsPaid,
          isCopying,
          isCreatingCredit,
          copyProblemDetails,
          creditProblemDetails
        };
        return context;
      }
    ),
    tap((vm) => (this.vm = vm))
  );

  public form: InvoiceFormGroup = this.invoiceDomainEntityService.form;
  public get invoiceLines(): InvoiceLineFormArray {
    return this.form.controls.invoiceLines as InvoiceLineFormArray;
  }

  constructor(
    private readonly router: Router,
    private readonly invoiceDomainEntityService: InvoiceDomainEntityService,
    private readonly modalService: ModalService,
    private readonly apiInvoicesClient: ApiInvoicesClient
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

  public copy(): void {
    this.modalService
      .confirmWithMessage('Are you sure you want to copy this invoice?', 'Copy')
      .pipe(
        switchMap((confirmed) => {
          if (confirmed) {
            this.isCopying.next(true);
            const command = new CopyInvoiceCommand();
            return this.apiInvoicesClient.copy(this.vm!.id!, command);
          } else {
            return EMPTY;
          }
        }),
        catchError(
          (
            error: ValidationProblemDetails | ProblemDetails | ApiException | PermissionDeniedError
          ) => {
            this.copyProblemDetails.next(error);
            return throwError(() => error);
          }
        ),
        finalize(() => this.isCopying.next(false))
      )
      .subscribe((response: CopyInvoiceResponse) => {
        this.router
          .navigateByUrl('/', { skipLocationChange: true })
          .then(() => this.router.navigate(['/invoices', response.id]));
      });
  }

  public credit(): void {
    this.modalService
      .confirmWithMessage('Are you sure you want to credit this invoice?', 'Credit')
      .pipe(
        switchMap((confirmed) => {
          if (confirmed) {
            this.isCreatingCredit.next(true);
            const command = new CreditInvoiceCommand();
            return this.apiInvoicesClient.credit(this.vm!.id!, command);
          } else {
            return EMPTY;
          }
        }),
        catchError(
          (
            error: ValidationProblemDetails | ProblemDetails | ApiException | PermissionDeniedError
          ) => {
            this.creditProblemDetails.next(error);
            return throwError(() => error);
          }
        ),
        finalize(() => this.isCreatingCredit.next(false))
      )
      .subscribe((response: CreditInvoiceResponse) => {
        this.router
          .navigateByUrl('/', { skipLocationChange: true })
          .then(() => this.router.navigate(['/invoices', response.id]));
      });
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
