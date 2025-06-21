import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { RouterModule } from '@angular/router';
import { forkJoin } from 'rxjs';

import { BusinessPartnerService } from '../../core/services/business-partner.service';
import { EmployeeService } from '../../core/services/employee.service';
import { ItemService } from '../../core/services/item.service';
import { IncomingPaymentService } from '../../core/services/incoming-payment.service';
import { PurchaseInvoiceService } from '../../core/services/purchase-invoice.service';
import { LoadingComponent } from '../../shared/components/loading/loading.component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatIconModule,
    RouterModule,
    LoadingComponent
  ],
  template: `
    <div class="container">
      <h1>Dashboard</h1>
      <p class="subtitle">Welcome to SAP Management System</p>

      <div *ngIf="isLoading">
        <app-loading></app-loading>
      </div>

      <div *ngIf="!isLoading" class="dashboard-grid">
        <mat-card class="dashboard-card" routerLink="/business-partners">
          <mat-card-content>
            <div class="card-icon">
              <mat-icon>business</mat-icon>
            </div>
            <h3>Business Partners</h3>
            <p class="count">{{ stats.businessPartners }}</p>
            <p class="description">Manage your business relationships</p>
          </mat-card-content>
        </mat-card>

        <mat-card class="dashboard-card" routerLink="/employees">
          <mat-card-content>
            <div class="card-icon">
              <mat-icon>people</mat-icon>
            </div>
            <h3>Employees</h3>
            <p class="count">{{ stats.employees }}</p>
            <p class="description">Employee management system</p>
          </mat-card-content>
        </mat-card>

        <mat-card class="dashboard-card" routerLink="/items">
          <mat-card-content>
            <div class="card-icon">
              <mat-icon>inventory</mat-icon>
            </div>
            <h3>Items</h3>
            <p class="count">{{ stats.items }}</p>
            <p class="description">Product and inventory management</p>
          </mat-card-content>
        </mat-card>

        <mat-card class="dashboard-card" routerLink="/incoming-payments">
          <mat-card-content>
            <div class="card-icon">
              <mat-icon>payment</mat-icon>
            </div>
            <h3>Incoming Payments</h3>
            <p class="count">{{ stats.incomingPayments }}</p>
            <p class="description">Track incoming payments</p>
          </mat-card-content>
        </mat-card>

        <mat-card class="dashboard-card" routerLink="/purchase-invoices">
          <mat-card-content>
            <div class="card-icon">
              <mat-icon>receipt</mat-icon>
            </div>
            <h3>Purchase Invoices</h3>
            <p class="count">{{ stats.purchaseInvoices }}</p>
            <p class="description">Manage purchase invoices</p>
          </mat-card-content>
        </mat-card>
      </div>
    </div>
  `,
  styles: [`
    .container {
      padding: 20px;
    }

    h1 {
      font-size: 2.5rem;
      font-weight: 300;
      color: #333;
      margin-bottom: 8px;
    }

    .subtitle {
      font-size: 1.1rem;
      color: #666;
      margin-bottom: 40px;
    }

    .dashboard-grid {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
      gap: 24px;
      margin-top: 20px;
    }

    .dashboard-card {
      cursor: pointer;
      transition: all 0.3s ease;
      background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
      color: white;
      border-radius: 16px;
      overflow: hidden;
    }

    .dashboard-card:hover {
      transform: translateY(-8px);
      box-shadow: 0 12px 24px rgba(0,0,0,0.15);
    }

    .dashboard-card mat-card-content {
      text-align: center;
      padding: 32px 24px;
    }

    .card-icon {
      margin-bottom: 16px;
    }

    .card-icon mat-icon {
      font-size: 48px;
      width: 48px;
      height: 48px;
      opacity: 0.9;
    }

    .dashboard-card h3 {
      font-size: 1.4rem;
      font-weight: 500;
      margin-bottom: 8px;
      opacity: 0.95;
    }

    .count {
      font-size: 2.5rem;
      font-weight: 700;
      margin: 16px 0;
      line-height: 1;
    }

    .description {
      font-size: 0.95rem;
      opacity: 0.8;
      margin: 0;
    }

    @media (max-width: 768px) {
      .dashboard-grid {
        grid-template-columns: 1fr;
      }
      
      h1 {
        font-size: 2rem;
      }
    }
  `]
})
export class DashboardComponent implements OnInit {
  isLoading = true;
  stats = {
    businessPartners: 0,
    employees: 0,
    items: 0,
    incomingPayments: 0,
    purchaseInvoices: 0
  };

  constructor(
    private businessPartnerService: BusinessPartnerService,
    private employeeService: EmployeeService,
    private itemService: ItemService,
    private incomingPaymentService: IncomingPaymentService,
    private purchaseInvoiceService: PurchaseInvoiceService
  ) {}

  ngOnInit(): void {
    this.loadDashboardStats();
  }

  private loadDashboardStats(): void {
    forkJoin({
      businessPartners: this.businessPartnerService.getAll(),
      employees: this.employeeService.getAll(),
      items: this.itemService.getAll(),
      incomingPayments: this.incomingPaymentService.getAll(),
      purchaseInvoices: this.purchaseInvoiceService.getAll()
    }).subscribe({
      next: (data) => {
        this.stats = {
          businessPartners: data.businessPartners.length,
          employees: data.employees.length,
          items: data.items.length,
          incomingPayments: data.incomingPayments.length,
          purchaseInvoices: data.purchaseInvoices.length
        };
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading dashboard stats:', error);
        this.isLoading = false;
      }
    });
  }
}