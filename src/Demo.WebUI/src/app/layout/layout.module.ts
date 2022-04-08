import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { SharedModule } from '@shared/shared.module';
import { DefaultTemplateComponent } from '@layout/default-template/default-template.component';

@NgModule({
  declarations: [DefaultTemplateComponent],
  imports: [CommonModule, RouterModule, SharedModule],
  exports: [DefaultTemplateComponent]
})
export class LayoutModule {}
