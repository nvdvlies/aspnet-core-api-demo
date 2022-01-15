import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DefaultTemplateComponent } from './default-template/default-template.component';
import { RouterModule } from '@angular/router';
import { SharedModule } from '@shared/shared.module';



@NgModule({
  declarations: [
    DefaultTemplateComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    SharedModule
  ],
  exports: [
    DefaultTemplateComponent
  ]
})
export class LayoutModule { }
