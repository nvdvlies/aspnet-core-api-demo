import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthGuard } from '@auth0/auth0-angular';
import { CustomerDetailsComponent } from '@customers/pages/customer-details/customer-details.component';
import { CustomerListComponent } from '@customers/pages/customer-list/customer-list.component';
import { PermissionGuard } from '@shared/guards/permission.guard';
import { UnsavedChangesGuard } from '@shared/guards/unsaved-changes.guard';
import { AppRoutes, RouteData } from 'src/app/app-routing.module';
import { CustomerAuditlogComponent } from './pages/customer-auditlog/customer-auditlog.component';

const routes: AppRoutes = [
  {
    path: '',
    canActivateChild: [AuthGuard],
    children: [
      {
        path: '',
        component: CustomerListComponent,
        canActivate: [PermissionGuard],
        data: {
          permission: 'CustomersRead'
        } as RouteData
      },
      {
        path: ':id/auditlog',
        component: CustomerAuditlogComponent,
        canActivate: [PermissionGuard],
        data: {
          permission: 'CustomersRead'
        } as RouteData
      },
      {
        path: ':id',
        component: CustomerDetailsComponent,
        canActivate: [PermissionGuard],
        data: {
          permission: 'CustomersRead'
        } as RouteData,
        canDeactivate: [UnsavedChangesGuard]
      }
    ] as AppRoutes
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CustomersRoutingModule {}
