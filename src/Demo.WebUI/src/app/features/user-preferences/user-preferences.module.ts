import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '@shared/shared.module';
import { UserPreferencesRoutingModule } from '@user-preferences/user-preferences-routing.module';
import { UserPreferencesDetailsComponent } from '@user-preferences/pages/user-preferences-details/user-preferences-details.component';

@NgModule({
  declarations: [UserPreferencesDetailsComponent],
  imports: [CommonModule, SharedModule, UserPreferencesRoutingModule]
})
export class UserPreferencesModule {}
