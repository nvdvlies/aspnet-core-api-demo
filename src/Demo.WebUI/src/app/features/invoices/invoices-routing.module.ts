import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthGuard } from '@auth0/auth0-angular';
import { InvoiceDetailsComponent } from '@invoices/pages/invoice-details/invoice-details.component';
import { InvoiceListComponent } from '@invoices/pages/invoice-list/invoice-list.component';
import { PermissionGuard } from '@shared/guards/permission.guard';
import { UnsavedChangesGuard } from '@shared/guards/unsaved-changes.guard';
import { AppRoutes, RouteData } from 'src/app/app-routing.module';
import { InvoiceAuditlogComponent } from './pages/invoice-auditlog/invoice-auditlog.component';
import { InvoicelistPageSettingsComponent } from './pages/invoice-list-page-settings/invoice-list-page-settings.component';

const routes: AppRoutes = [
  {
    path: '',
    canActivateChild: [AuthGuard],
    children: [
      {
        path: '',
        component: InvoiceListComponent,
        canActivate: [PermissionGuard],
        data: {
          permission: 'InvoicesRead'
        } as RouteData
      },
      {
        path: 'page-settings',
        component: InvoicelistPageSettingsComponent,
        canActivate: [PermissionGuard],
        data: {
          permission: 'InvoicesRead'
        } as RouteData
      },
      {
        path: ':id/auditlog',
        component: InvoiceAuditlogComponent,
        canActivate: [PermissionGuard],
        data: {
          permission: 'InvoicesRead'
        } as RouteData
      },
      {
        path: ':id',
        component: InvoiceDetailsComponent,
        canActivate: [PermissionGuard],
        data: {
          permission: 'InvoicesRead'
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
export class InvoicesRoutingModule {}
