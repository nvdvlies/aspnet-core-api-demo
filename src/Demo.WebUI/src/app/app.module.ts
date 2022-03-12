import { DEFAULT_CURRENCY_CODE, LOCALE_ID, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AuthModule } from '@auth0/auth0-angular';
import { CoreModule } from '@core/core.module';
import { DomainModule } from '@domain/domain.module';
import { SharedModule } from '@shared/shared.module';
import { ApiModule } from '@api/api.module';
import { environment } from '@env/environment';
import { LayoutModule } from '@layout/layout.module';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import locale from '@angular/common/locales/nl';
import { registerLocaleData } from '@angular/common';

registerLocaleData(locale, 'nl');

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    ReactiveFormsModule,
    AppRoutingModule,
    AuthModule.forRoot({
      ...environment.auth,
      scope: 'user',
      httpInterceptor: {
        allowedList: [
          {
            uri: `${environment.apiBaseUrl}/*`,
            tokenOptions: {
              audience: environment.auth.audience,
              scope: 'user'
            }
          }
        ]
      }
    }),
    ApiModule.forRoot(),
    CoreModule.forRoot(),
    DomainModule.forRoot(),
    LayoutModule,
    SharedModule
  ],
  providers: [
    {
      provide: LOCALE_ID,
      useValue: 'nl'
    },
    {
      provide: DEFAULT_CURRENCY_CODE,
      useValue: 'EUR'
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}
