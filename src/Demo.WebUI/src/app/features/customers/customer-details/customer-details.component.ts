import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { combineLatest, map, Observable } from 'rxjs';
import { CustomerDto, ProblemDetails, ValidationProblemDetails } from '@api/api.generated.clients';
import { CustomerDomainEntityService, CustomerFormGroup, ICustomerDomainEntityContext } from '@domain/customer/customer-domain-entity.service';
import { AbstractControl } from '@angular/forms';

interface ViewModel extends ICustomerDomainEntityContext {
  id: string | null;
  entity: Readonly<CustomerDto> | undefined;
  pristine: Readonly<CustomerDto> | undefined;
  isLoading: boolean;
  isSaving: boolean;
  isDeleting: boolean;
  hasNewerVersionWithMergeConflict: boolean;
  problemDetails: ValidationProblemDetails | ProblemDetails | undefined;
}

@Component({
  templateUrl: './customer-details.component.html',
  styleUrls: ['./customer-details.component.scss'],
  providers: [CustomerDomainEntityService],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CustomerDetailsComponent implements OnInit {
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
    private readonly customerDomainEntityService: CustomerDomainEntityService
  ) { 
  }

  ngOnInit(): void {
  }

  public form: CustomerFormGroup = this.customerDomainEntityService.form;

  public get name(): AbstractControl { return this.form.controls.name!; }

  public save(): void {
    this.customerDomainEntityService.upsert()
      .subscribe(() => {
        this.router.navigateByUrl('/customers');
      });
  }

  public delete(): void {
    this.customerDomainEntityService.delete()
      .subscribe(() => {
        this.router.navigateByUrl('/customers');
      });
  }

  public resolveMergeConflictWithTakeTheirs(): void {
    this.customerDomainEntityService.resolveMergeConflictWithTakeTheirs();
  }
}
