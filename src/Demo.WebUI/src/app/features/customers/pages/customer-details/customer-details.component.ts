import { ChangeDetectionStrategy, Component, HostListener, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { combineLatest, debounceTime, EMPTY, map, Observable, switchMap, tap } from 'rxjs';
import { DomainEntityService } from '@domain/shared/domain-entity-base';
import {
  CustomerDomainEntityService,
  CustomerFormGroup,
  ICustomerDomainEntityContext
} from '@domain/customer/customer-domain-entity.service';
import { IHasForm } from '@shared/guards/unsaved-changes.guard';
import { CustomerListRouteState } from '@customers/pages/customer-list/customer-list.component';
import { ConfirmDeleteModalComponent } from '@shared/modals/confirm-delete-modal/confirm-delete-modal.component';
import { ModalService } from '@shared/services/modal.service';

interface ViewModel extends ICustomerDomainEntityContext {}

@Component({
  templateUrl: './customer-details.component.html',
  styleUrls: ['./customer-details.component.scss'],
  providers: [
    CustomerDomainEntityService,
    {
      provide: DomainEntityService,
      useExisting: CustomerDomainEntityService
    }
  ],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CustomerDetailsComponent implements OnInit, IHasForm {
  public initFromRoute$ = this.customerDomainEntityService.initFromRoute();

  private vm: Readonly<ViewModel> | undefined;

  public vm$ = combineLatest([this.customerDomainEntityService.observe$]).pipe(
    debounceTime(0),
    map(([domainEntityContext]) => {
      const vm: ViewModel = {
        ...domainEntityContext
      };
      return vm;
    }),
    tap((vm) => (this.vm = vm))
  ) as Observable<ViewModel>;

  public form: CustomerFormGroup = this.customerDomainEntityService.form;

  constructor(
    private readonly router: Router,
    private readonly customerDomainEntityService: CustomerDomainEntityService,
    private readonly modalService: ModalService
  ) {}

  public ngOnInit(): void {}

  public save(): void {
    if (!this.form.valid) {
      return;
    }

    this.customerDomainEntityService.upsert().subscribe((customer) => {
      this.router.navigateByUrl('/customers', {
        state: { spotlightIdentifier: customer.id } as CustomerListRouteState
      });
    });
  }

  public delete(): void {
    this.modalService
      .confirm(ConfirmDeleteModalComponent)
      .pipe(
        switchMap((confirmed) => (confirmed ? this.customerDomainEntityService.delete() : EMPTY))
      )
      .subscribe(() => {
        this.router.navigateByUrl('/customers');
      });
  }

  public resolveMergeConflictWithTakeTheirs(): void {
    this.customerDomainEntityService.resolveMergeConflictWithTakeTheirs();
  }

  @HostListener('document:keydown.shift.alt.s', ['$event'])
  public saveShortcut(event: KeyboardEvent) {
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
    this.router.navigateByUrl('/customers');
    event.preventDefault();
  }
}
