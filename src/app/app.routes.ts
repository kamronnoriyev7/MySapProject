import { Routes } from '@angular/router';
import { AuthGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: '',
    redirectTo: '/login',
    pathMatch: 'full'
  },
  {
    path: 'login',
    loadComponent: () => import('./features/auth/login/login.component').then(m => m.LoginComponent)
  },
  {
    path: 'dashboard',
    loadComponent: () => import('./features/dashboard/dashboard.component').then(m => m.DashboardComponent),
    canActivate: [AuthGuard]
  },
  {
    path: 'business-partners',
    loadComponent: () => import('./features/business-partners/business-partners.component').then(m => m.BusinessPartnersComponent),
    canActivate: [AuthGuard]
  },
  {
    path: 'employees',
    loadComponent: () => import('./features/employees/employees.component').then(m => m.EmployeesComponent),
    canActivate: [AuthGuard]
  },
  {
    path: 'items',
    loadComponent: () => import('./features/items/items.component').then(m => m.ItemsComponent),
    canActivate: [AuthGuard]
  },
  {
    path: 'incoming-payments',
    loadComponent: () => import('./features/incoming-payments/incoming-payments.component').then(m => m.IncomingPaymentsComponent),
    canActivate: [AuthGuard]
  },
  {
    path: 'purchase-invoices',
    loadComponent: () => import('./features/purchase-invoices/purchase-invoices.component').then(m => m.PurchaseInvoicesComponent),
    canActivate: [AuthGuard]
  },
  {
    path: '**',
    redirectTo: '/dashboard'
  }
];