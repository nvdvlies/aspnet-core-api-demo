import { CommonModule } from '@angular/common';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule, Optional, SkipSelf, ModuleWithProviders } from '@angular/core';

import { AuthHttpInterceptor } from '@auth0/auth0-angular';

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
          {
            provide: HTTP_INTERCEPTORS,
            useClass: AuthHttpInterceptor,
            multi: true,
          },
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
