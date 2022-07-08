import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '@shared/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TableFilterContainerDirective } from '@shared/directives/table-filter-container.directive';
import { TableFilterDirective } from '@shared/directives/table-filter.directive';
import { PageHeaderComponent } from '@shared/components/page-header/page-header.component';
import { DiscardUnsavedChangesModalComponent } from '@shared/modals/discard-unsaved-changes-modal/discard-unsaved-changes-modal.component';
import { ProblemDetailsComponent } from '@shared/components/problem-details/problem-details.component';
import { DomainEntityErrorMessageDirective } from '@shared/directives/domain-entity-error-message.directive';
import { SetFocusDirective } from '@shared/directives/set-focus.directive';
import { CustomerIdToNamePipe } from '@shared/pipes/customer-id-to-name.pipe';
import { CurrencyFormFieldControlComponent } from '@shared/components/currency-form-field-control/currency-form-field-control.component';
import { CustomerAutocompleteFormFieldControlComponent } from '@shared/components/customer-autocomplete-form-field-control/customer-autocomplete-form-field-control.component';
import { UserIdToNamePipe } from '@shared/pipes/user-id-to-name.pipe';
import { SelectOnFocusDirective } from '@shared/directives/select-on-focus.directive';
import { ConfirmDeleteModalComponent } from '@shared/modals/confirm-delete-modal/confirm-delete-modal.component';
import { MessageComponent } from '@shared/components/message/message.component';
import { IfFeatureFlagEnabledDirective } from '@shared/directives/if-feature-flag-enabled.directive';
import { IfFeatureFlagDisabledDirective } from '@shared/directives/if-feature-flag-disabled.directive';
import { SpinnerComponent } from '@shared/components/spinner/spinner.component';
import { UserAutocompleteFormFieldControlComponent } from '@shared/components/user-autocomplete-form-field-control/user-autocomplete-form-field-control.component';
import { RoleAutocompleteFormFieldControlComponent } from '@shared/components/role-autocomplete-form-field-control/role-autocomplete-form-field-control.component';
import { AuditlogTableComponent } from '@shared/components/auditlog-table/auditlog-table.component';
import { HighlightComponent } from './components/highlight/highlight.component';
import { AuditlogItemComponent } from './components/auditlog-item/auditlog-item.component';
import { InvoiceIdToNumberPipe } from './pipes/invoice-id-to-number.pipe';
import { AuditlogItemValuePipe } from './pipes/auditlog-item-value.pipe';
import { UserIdsToNamesPipe } from './pipes/user-ids-to-names.pipe';
import { InvoiceStatusEnumToNamePipe } from './pipes/invoice-status-enum-to-name.pipe';
import { RoleIdToNamePipe } from './pipes/role-id-to-name.pipe';
import { RoleIdsToNamesPipe } from './pipes/roles-ids-to-names.pipe';
import { MessageModalComponent } from './modals/message-modal/message-modal.component';
import { ConfirmModalComponent } from './modals/confirm-modal/confirm-modal.component';

@NgModule({
  declarations: [
    TableFilterContainerDirective,
    TableFilterDirective,
    PageHeaderComponent,
    DiscardUnsavedChangesModalComponent,
    ProblemDetailsComponent,
    DomainEntityErrorMessageDirective,
    SetFocusDirective,
    CustomerAutocompleteFormFieldControlComponent,
    CustomerIdToNamePipe,
    UserIdToNamePipe,
    CurrencyFormFieldControlComponent,
    SelectOnFocusDirective,
    ConfirmDeleteModalComponent,
    MessageComponent,
    IfFeatureFlagEnabledDirective,
    IfFeatureFlagDisabledDirective,
    SpinnerComponent,
    UserAutocompleteFormFieldControlComponent,
    RoleAutocompleteFormFieldControlComponent,
    AuditlogTableComponent,
    HighlightComponent,
    AuditlogItemComponent,
    InvoiceIdToNumberPipe,
    AuditlogItemValuePipe,
    UserIdsToNamesPipe,
    InvoiceStatusEnumToNamePipe,
    RoleIdToNamePipe,
    RoleIdsToNamesPipe,
    MessageModalComponent,
    ConfirmModalComponent
  ],
  imports: [CommonModule, ReactiveFormsModule, FormsModule, MaterialModule],
  exports: [
    ReactiveFormsModule,
    FormsModule,
    MaterialModule,
    TableFilterContainerDirective,
    TableFilterDirective,
    PageHeaderComponent,
    DiscardUnsavedChangesModalComponent,
    ProblemDetailsComponent,
    DomainEntityErrorMessageDirective,
    SetFocusDirective,
    CustomerAutocompleteFormFieldControlComponent,
    CustomerIdToNamePipe,
    UserIdToNamePipe,
    CurrencyFormFieldControlComponent,
    SelectOnFocusDirective,
    ConfirmDeleteModalComponent,
    MessageComponent,
    IfFeatureFlagEnabledDirective,
    IfFeatureFlagDisabledDirective,
    SpinnerComponent,
    UserAutocompleteFormFieldControlComponent,
    RoleAutocompleteFormFieldControlComponent,
    AuditlogTableComponent,
    HighlightComponent,
    AuditlogItemComponent,
    InvoiceIdToNumberPipe,
    AuditlogItemValuePipe,
    UserIdsToNamesPipe,
    InvoiceStatusEnumToNamePipe,
    RoleIdToNamePipe,
    RoleIdsToNamesPipe,
    MessageModalComponent,
    ConfirmModalComponent
  ],
  providers: [
    CustomerIdToNamePipe,
    UserIdToNamePipe,
    InvoiceIdToNumberPipe,
    UserIdsToNamesPipe,
    RoleIdToNamePipe,
    RoleIdsToNamesPipe
  ]
})
export class SharedModule {}
