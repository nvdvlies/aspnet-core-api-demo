import { CommonModule } from '@angular/common';
import { NgModule, Optional, SkipSelf, ModuleWithProviders } from '@angular/core';
import { API_BASE_URL } from '@api/api.generated.clients';
import { SIGNALR_BASE_URL } from '@api/signalr.generated.services';
import { environment } from '@env/environment';

@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ]
})
export class ApiModule {
  static forRoot(): ModuleWithProviders<ApiModule> {
    return {
        ngModule: ApiModule,
        providers: [
          { provide: API_BASE_URL, useValue: environment.apiBaseUrl },
          { provide: SIGNALR_BASE_URL, useValue: environment.apiBaseUrl }
        ]
    };
  }

  constructor(@Optional() @SkipSelf() parentModule: ApiModule) {
      if (parentModule) {
          throw new Error("ApiModule is already loaded. Import in AppModule only");
      }
  }
}
