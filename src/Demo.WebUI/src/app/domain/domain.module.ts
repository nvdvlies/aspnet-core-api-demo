import { ModuleWithProviders, NgModule, Optional, SkipSelf } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ConfirmDeleteModalComponent } from '@domain/shared/confirm-delete-modal.component';
import { SharedModule } from '@shared/shared.module';

@NgModule({
  declarations: [ConfirmDeleteModalComponent],
  imports: [CommonModule, SharedModule]
})
export class DomainModule {
  static forRoot(): ModuleWithProviders<DomainModule> {
    return {
      ngModule: DomainModule,
      providers: []
    };
  }

  constructor(@Optional() @SkipSelf() parentModule: DomainModule) {
    if (parentModule) {
      throw new Error('DomainModule is already loaded. Import in AppModule only');
    }
  }
}
