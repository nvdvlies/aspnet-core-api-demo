import { FocusMonitor } from '@angular/cdk/a11y';
import {
  Component,
  ChangeDetectionStrategy,
  Optional,
  ElementRef,
  Host,
  SkipSelf,
  Self
} from '@angular/core';
import { ControlContainer, NgControl } from '@angular/forms';
import { MatFormFieldControl } from '@angular/material/form-field';
import { ApiCustomersClient, CustomerLookupOrderByEnum } from '@api/api.generated.clients';
import {
  AutocompleteMatFormFieldControlBase,
  AutocompleteOption
} from '@shared/base/autocomplete-mat-form-field-control-base';
import { CustomerLookupService } from '@shared/services/customer-lookup.service';
import { map } from 'rxjs';

@Component({
  selector: 'app-customer-autocomplete-form-field-control',
  templateUrl: '../../../base/autocomplete-mat-form-field-control-base.html',
  styleUrls: [
    '../../../base/autocomplete-mat-form-field-control-base.scss',
    './customer-autocomplete.component.scss'
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
    {
      provide: MatFormFieldControl,
      useExisting: CustomerAutocompleteComponent
    }
  ]
})
export class CustomerAutocompleteComponent extends AutocompleteMatFormFieldControlBase {
  constructor(
    @Optional() elementRef: ElementRef<HTMLElement>,
    @Optional() focusMonitor: FocusMonitor,
    @Host() @SkipSelf() controlContainer: ControlContainer,
    @Optional() @Self() ngControl: NgControl,
    private readonly apiCustomersClient: ApiCustomersClient,
    private readonly customerLookupService: CustomerLookupService
  ) {
    super(elementRef, focusMonitor, controlContainer, ngControl);
  }

  protected searchFunction = (searchTerm: string | undefined) => {
    return this.apiCustomersClient
      .lookup(CustomerLookupOrderByEnum.Name, false, 0, 25, searchTerm, undefined)
      .pipe(
        map(
          (response) =>
            response.customers?.map(
              (customer) => new AutocompleteOption(customer.id!, customer.name!)
            ) ?? []
        )
      );
  };

  protected lookupFunction = (id: string) => {
    return this.customerLookupService
      .getById(id)
      .pipe(
        map((customer) =>
          customer ? new AutocompleteOption(customer.id!, customer.name!) : undefined
        )
      );
  };
}
