import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from './material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';
import { TableFilterContainerDirective } from './directives/table-filter/table-filter-container.directive';
import { TableFilterDirective } from './directives/table-filter/table-filter.directive';
import { PageHeaderComponent } from './components/page-header/page-header.component';
import { DiscardUnsavedChangesModalComponent } from './modals/discard-unsaved-changes-modal/discard-unsaved-changes-modal.component';
import { ProblemDetailsComponent } from './components/problem-details/problem-details.component';
import { DomainEntityErrorMessageDirective } from './directives/domain-entity-error-message/domain-entity-error-message.directive';
import { FocusDirective } from './directives/focus/focus.directive';
import { CustomerAutocompleteComponent } from './components/autocomplete/customer-autocomplete/customer-autocomplete.component';
import { CustomerIdToNamePipe } from './pipes/customer-id-to-name.pipe';
import { CurrencyInputDirective } from './directives/currency-input/currency-input.directive';

@NgModule({
  declarations: [
    TableFilterContainerDirective,
    TableFilterDirective,
    PageHeaderComponent,
    DiscardUnsavedChangesModalComponent,
    ProblemDetailsComponent,
    DomainEntityErrorMessageDirective,
    FocusDirective,
    CustomerAutocompleteComponent,
    CustomerIdToNamePipe,
    CurrencyInputDirective
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
    CustomerAutocompleteComponent,
    CustomerIdToNamePipe,
    CurrencyInputDirective
  ]
})
export class SharedModule {}
