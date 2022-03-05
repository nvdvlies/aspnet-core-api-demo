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
  CustomerDomainEntityService,
  CustomerFormGroup,
  ICustomerDomainEntityContext
} from '@domain/customer/customer-domain-entity.service';
import { IHasForm } from '@shared/guards/unsaved-changes.guard';
import { CustomerDto } from '@api/api.generated.clients';
import { CustomerListRouteState } from '@customers/customer-list/customer-list.component';

interface ViewModel extends ICustomerDomainEntityContext {
  createdByFullname: string | undefined;
  lastModifiedByFullname: string | undefined;
}

@Component({
  templateUrl: './customer-details.component.html',
  styleUrls: ['./customer-details.component.scss'],
  providers: [
    CustomerDomainEntityService,
    {
      provide: BaseDomainEntityService,
      useExisting: CustomerDomainEntityService
    }
  ],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CustomerDetailsComponent implements OnInit, IHasForm {
  public initFromRoute$ = this.customerDomainEntityService.initFromRoute();

  private readonly createdByFullname = new BehaviorSubject<string | undefined>(undefined);
  private readonly lastModifiedByFullname = new BehaviorSubject<string | undefined>(undefined);

  private createdByFullname$ = this.createdByFullname.asObservable();
  private lastModifiedByFullname$ = this.lastModifiedByFullname.asObservable();

  public vm$ = combineLatest([
    this.customerDomainEntityService.observe$,
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

  public form: CustomerFormGroup = this.customerDomainEntityService.form;

  // public get name(): FormControl { return this.form.controls.name as FormControl; }

  constructor(
    private readonly router: Router,
    private readonly customerDomainEntityService: CustomerDomainEntityService
  ) {}

  public ngOnInit(): void {
    this.customerDomainEntityService.afterNewHook = this.initNewCustomer.bind(this);
    this.customerDomainEntityService.afterGetByIdHook = this.loadAdditionalData.bind(this);
  }

  private initNewCustomer(customer: CustomerDto): Observable<null> {
    // this.form.controls.name?.patchValue('!');
    // this.form.controls.name?.markAsDirty();
    return of(null);
  }

  private loadAdditionalData(customer: CustomerDto): Observable<null> {
    // this.form.controls.name?.patchValue(customer.name += "!");
    // this.form.controls.name?.markAsDirty();

    return forkJoin([
      //of(null), // get invoices
      //of(null), // get contacts
      of([
        { id: customer.createdBy, fullName: 'John Doe' },
        { id: customer.lastModifiedBy, fullName: 'Jane Doe' }
      ]) // this.usersQuickSearch.getByIds([customer.createdBy, customer.lastModifiedBy])
    ]).pipe(
      tap(([users]) => {
        this.createdByFullname.next(
          users.find((x) => x.id?.toLowerCase() === customer.createdBy?.toLowerCase())?.fullName ??
            undefined
        );
        this.lastModifiedByFullname.next(
          users.find((x) => x.id?.toLowerCase() === customer.lastModifiedBy?.toLowerCase())
            ?.fullName ?? undefined
        );
      }),
      switchMap(() => of(null))
    );
  }

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
    this.customerDomainEntityService.deleteWithConfirmation().subscribe(() => {
      this.router.navigateByUrl('/customers');
    });
  }

  public resolveMergeConflictWithTakeTheirs(): void {
    this.customerDomainEntityService.resolveMergeConflictWithTakeTheirs();
  }
}
