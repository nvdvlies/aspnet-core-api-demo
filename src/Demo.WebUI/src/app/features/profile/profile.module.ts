import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '@shared/shared.module';
import { ProfileRoutingModule } from './profile-routing.module';
import { ProfileDetailsComponent } from './pages/profile-details/profile-details.component';

@NgModule({
  declarations: [
    ProfileDetailsComponent
  ],
  imports: [CommonModule, SharedModule, ProfileRoutingModule]
})
export class ProfileModule {}
