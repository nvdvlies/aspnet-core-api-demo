import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthGuard } from '@auth0/auth0-angular';
import { CustomerDetailsComponent } from '@customers/pages/customer-details/customer-details.component';
import { CustomerListComponent } from '@customers/pages/customer-list/customer-list.component';
import { FeatureFlag } from '@shared/enums/feature-flag.enum';
import { Role } from '@shared/enums/role.enum';
import { FeatureFlagGuard } from '@shared/guards/feature-flag.guard';
import { RoleGuard } from '@shared/guards/role.guard';
import { UnsavedChangesGuard } from '@shared/guards/unsaved-changes.guard';
import { AppRoutes, RouteData } from 'src/app/app-routing.module';

const routes: AppRoutes = [
  {
    path: '',
    canActivateChild: [AuthGuard],
    children: [
      {
        path: '',
        component: CustomerListComponent
      },
      {
        path: ':id',
        component: CustomerDetailsComponent,
        // canActivate: [FeatureFlagGuard, RoleGuard],
        canDeactivate: [UnsavedChangesGuard]
        // data: {
        //   featureFlag: FeatureFlag.FeatureFlagX,
        //   roleNames: [Role.Administrator]
        // } as RouteData
      }
    ] as AppRoutes
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CustomersRoutingModule {}
