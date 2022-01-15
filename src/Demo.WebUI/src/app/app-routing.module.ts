import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '@auth0/auth0-angular';
import { DefaultTemplateComponent } from '@layout/default-template/default-template.component';

const routes: Routes = [
  { 
    path: '', 
    component: DefaultTemplateComponent,
    canActivate: [AuthGuard],
    children: [
      { 
        path: '', 
        redirectTo: 'customers', 
        pathMatch: 'full' 
      },
      { 
        path: 'customers', 
        loadChildren: () => import('./features/customers/customers.module').then(m => m.CustomersModule), 
        canActivate: [AuthGuard] 
      }, 
      { 
        path: 'invoices', 
        loadChildren: () => import('./features/invoices/invoices.module').then(m => m.InvoicesModule), 
        canActivate: [AuthGuard] 
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
