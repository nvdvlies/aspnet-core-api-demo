import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthGuard } from '@auth0/auth0-angular';
import { InvoiceDetailsComponent } from '@invoices/pages/invoice-details/invoice-details.component';
import { InvoiceListComponent } from '@invoices/pages/invoice-list/invoice-list.component';
import { UnsavedChangesGuard } from '@shared/guards/unsaved-changes.guard';
import { AppRoutes } from 'src/app/app-routing.module';
import { InvoiceAuditlogComponent } from './pages/invoice-auditlog/invoice-auditlog.component';

const routes: AppRoutes = [
  {
    path: '',
    canActivateChild: [AuthGuard],
    children: [
      {
        path: '',
        component: InvoiceListComponent
      },
      {
        path: ':id/auditlog',
        component: InvoiceAuditlogComponent
      },
      {
        path: ':id',
        component: InvoiceDetailsComponent,
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
