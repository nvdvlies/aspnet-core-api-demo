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
import { PermissionIdsToNamesPipe } from './pipes/permission-ids-to-names.pipe';
import { IfPermissionGrantedDirective } from './directives/if-permission-granted.directive';
import { IfPermissionDeniedDirective } from './directives/if-permission-denied.directive';
import { IfAnyPermissionGrantedDirective } from './directives/if-any-permission-granted.directive';
import { BackButtonDirective } from './directives/back-button.directive';

const components = [
  PageHeaderComponent,
  DiscardUnsavedChangesModalComponent,
  ProblemDetailsComponent,
  CustomerAutocompleteFormFieldControlComponent,
  CurrencyFormFieldControlComponent,
  ConfirmDeleteModalComponent,
  MessageComponent,
  SpinnerComponent,
  UserAutocompleteFormFieldControlComponent,
  RoleAutocompleteFormFieldControlComponent,
  AuditlogTableComponent,
  HighlightComponent,
  AuditlogItemComponent,
  MessageModalComponent,
  ConfirmModalComponent
];

const directives = [
  TableFilterContainerDirective,
  TableFilterDirective,
  DomainEntityErrorMessageDirective,
  SetFocusDirective,
  SelectOnFocusDirective,
  IfFeatureFlagEnabledDirective,
  IfFeatureFlagDisabledDirective,
  IfPermissionGrantedDirective,
  IfPermissionDeniedDirective,
  IfAnyPermissionGrantedDirective,
  BackButtonDirective
];

const pipes = [
  CustomerIdToNamePipe,
  UserIdToNamePipe,
  InvoiceIdToNumberPipe,
  UserIdsToNamesPipe,
  RoleIdToNamePipe,
  RoleIdsToNamesPipe,
  AuditlogItemValuePipe,
  InvoiceStatusEnumToNamePipe,
  PermissionIdsToNamesPipe
];

@NgModule({
  declarations: [...components, ...directives, ...pipes],
  imports: [CommonModule, ReactiveFormsModule, FormsModule, MaterialModule],
  exports: [
    ReactiveFormsModule,
    FormsModule,
    MaterialModule,
    ...components,
    ...directives,
    ...pipes
  ],
  providers: [...pipes]
})
export class SharedModule {}
