import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '@shared/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';
import { TableFilterContainerDirective } from '@shared/directives/table-filter-container.directive';
import { TableFilterDirective } from '@shared/directives/table-filter.directive';
import { PageHeaderComponent } from '@shared/components/page-header/page-header.component';
import { DiscardUnsavedChangesModalComponent } from '@shared/modals/discard-unsaved-changes-modal/discard-unsaved-changes-modal.component';
import { ProblemDetailsComponent } from '@shared/components/problem-details/problem-details.component';
import { DomainEntityErrorMessageDirective } from '@shared/directives/domain-entity-error-message.directive';
import { FocusDirective } from '@shared/directives/focus.directive';
import { CustomerIdToNamePipe } from '@shared/pipes/customer-id-to-name.pipe';
import { CurrencyFormFieldControlComponent } from '@shared/components/currency-form-field-control/currency-form-field-control.component';
import { CustomerAutocompleteFormFieldControlComponent } from '@shared/components/customer-autocomplete-form-field-control/customer-autocomplete-form-field-control.component';
import { UserIdToNamePipe } from '@shared/pipes/user-id-to-name.pipe';
import { SelectOnFocusDirective } from '@shared/directives/select-on-focus.directive';

@NgModule({
  declarations: [
    TableFilterContainerDirective,
    TableFilterDirective,
    PageHeaderComponent,
    DiscardUnsavedChangesModalComponent,
    ProblemDetailsComponent,
    DomainEntityErrorMessageDirective,
    FocusDirective,
    CustomerAutocompleteFormFieldControlComponent,
    CustomerIdToNamePipe,
    UserIdToNamePipe,
    CurrencyFormFieldControlComponent,
    SelectOnFocusDirective
  ],
  imports: [CommonModule, ReactiveFormsModule, FormsModule, MaterialModule, FlexLayoutModule],
  exports: [
    ReactiveFormsModule,
    FormsModule,
    MaterialModule,
    FlexLayoutModule,
    TableFilterContainerDirective,
    TableFilterDirective,
    PageHeaderComponent,
    DiscardUnsavedChangesModalComponent,
    ProblemDetailsComponent,
    DomainEntityErrorMessageDirective,
    FocusDirective,
    CustomerAutocompleteFormFieldControlComponent,
    CustomerIdToNamePipe,
    UserIdToNamePipe,
    CurrencyFormFieldControlComponent,
    SelectOnFocusDirective
  ]
})
export class SharedModule {}
