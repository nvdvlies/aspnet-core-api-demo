import { ModuleWithProviders, NgModule, Optional, SkipSelf } from '@angular/core';
import { CommonModule } from '@angular/common';

@NgModule({
  declarations: [],
  imports: [CommonModule]
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
