import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  { 
    path: 'customers', 
    loadChildren: () => import('./features/customers/customers.module').then(m => m.CustomersModule) 
  }, 
  { 
    path: 'invoices', 
    loadChildren: () => import('./features/invoices/invoices.module').then(m => m.InvoicesModule) 
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
