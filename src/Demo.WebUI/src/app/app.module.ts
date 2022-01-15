import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CoreModule } from '@core/core.module';
import { DomainModule } from '@domain/domain.module';
import { SharedModule } from '@shared/shared.module';
import { ApiModule } from '@api/api.module';
import { LayoutModule } from './layout/layout.module';
import { AuthModule } from '@auth0/auth0-angular';
import { environment } from '@env/environment';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    ApiModule.forRoot(),
    CoreModule.forRoot(),
    DomainModule.forRoot(),
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
        ],
      },
    }),
    SharedModule,
    LayoutModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
