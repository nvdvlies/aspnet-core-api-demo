import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from './material.module';
import { ReactiveFormsModule } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';
import { TableFilterContainerDirective } from './directives/table-filter/table-filter-container.directive';
import { TableFilterDirective } from './directives/table-filter/table-filter.directive';
import { PageHeaderComponent } from './components/page-header/page-header.component';
import { DiscardUnsavedChangesModalComponent } from './modals/discard-unsaved-changes-modal/discard-unsaved-changes-modal.component';

@NgModule({
  declarations: [
    TableFilterContainerDirective,
    TableFilterDirective,
    PageHeaderComponent,
    DiscardUnsavedChangesModalComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MaterialModule,
    FlexLayoutModule
  ],
  exports: [
    ReactiveFormsModule,
    MaterialModule,
    FlexLayoutModule,
    TableFilterContainerDirective,
    TableFilterDirective,
    PageHeaderComponent,
    DiscardUnsavedChangesModalComponent
  ]
})
export class SharedModule { }
