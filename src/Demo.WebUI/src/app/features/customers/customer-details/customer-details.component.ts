import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { combineLatest, map, Observable } from 'rxjs';
import { BaseDomainEntityService } from '@domain/shared/domain-entity-base';
import { CustomerDomainEntityService, CustomerFormGroup, ICustomerDomainEntityContext } from '@domain/customer/customer-domain-entity.service';
import { IHasForm } from '@shared/guards/unsaved-changes.guard';
import { CustomerTableDataService } from '@customers/customer-list/customer-table-data.service';

interface ViewModel extends ICustomerDomainEntityContext {
}

@Component({
  templateUrl: './customer-details.component.html',
  styleUrls: ['./customer-details.component.scss'],
  providers: [
    CustomerDomainEntityService,
    { provide: BaseDomainEntityService, useExisting: CustomerDomainEntityService }
  ],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CustomerDetailsComponent implements OnInit, IHasForm {
  public initFromRoute$ = this.customerDomainEntityService.initFromRoute();

  public vm$ = combineLatest([
    this.customerDomainEntityService.observe$
  ])
    .pipe(
      map(([
        domainEntityContext,
      ]) => {
        return {
          ...domainEntityContext
        } as ViewModel;
      })
    ) as Observable<ViewModel>;
    
  constructor(
    private readonly router: Router,
    private readonly customerDomainEntityService: CustomerDomainEntityService,
    private readonly customerTableDataService: CustomerTableDataService
  ) { 
  }

  ngOnInit(): void {
  }

  public form: CustomerFormGroup = this.customerDomainEntityService.form;

  // public get name(): FormControl { return this.form.controls.name as FormControl; }

  public save(): void {
    this.customerDomainEntityService.upsert()
      .subscribe(customer => {
        this.customerTableDataService.spotlight(customer.id);
        this.router.navigateByUrl('/customers');
      });
  }

  public delete(): void {
    this.customerDomainEntityService.deleteWithConfirmation()
      .subscribe(() => {
        this.router.navigateByUrl('/customers');
      });
  }

  public resolveMergeConflictWithTakeTheirs(): void {
    this.customerDomainEntityService.resolveMergeConflictWithTakeTheirs();
  }
}
