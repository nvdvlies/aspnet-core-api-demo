import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '@auth0/auth0-angular';
import { InvoiceDetailsComponent } from '@invoices/pages/invoice-details/invoice-details.component';
import { InvoiceListComponent } from '@invoices/pages/invoice-list/invoice-list.component';
import { UnsavedChangesGuard } from '@shared/guards/unsaved-changes.guard';

const routes: Routes = [
  {
    path: '',
    canActivateChild: [AuthGuard],
    children: [
      {
        path: '',
        component: InvoiceListComponent
      },
      {
        path: ':id',
        component: InvoiceDetailsComponent,
        canDeactivate: [UnsavedChangesGuard]
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InvoicesRoutingModule {}
