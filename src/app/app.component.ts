import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet, Router, NavigationEnd } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { filter } from 'rxjs/operators';

import { AuthService } from './core/services/auth.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule,
    RouterOutlet,
    MatToolbarModule,
    MatSidenavModule,
    MatListModule,
    MatIconModule,
    MatButtonModule
  ],
  template: `
    <div class="sidenav-container" *ngIf="showNavigation">
      <mat-sidenav-container class="sidenav-container">
        <mat-sidenav #drawer class="sidenav" mode="side" opened="true">
          <mat-toolbar color="primary">
            <span>SAP System</span>
          </mat-toolbar>
          <mat-nav-list class="navigation-menu">
            <a mat-list-item routerLink="/dashboard" routerLinkActive="active">
              <mat-icon>dashboard</mat-icon>
              <span>Dashboard</span>
            </a>
            <a mat-list-item routerLink="/business-partners" routerLinkActive="active">
              <mat-icon>business</mat-icon>
              <span>Business Partners</span>
            </a>
            <a mat-list-item routerLink="/employees" routerLinkActive="active">
              <mat-icon>people</mat-icon>
              <span>Employees</span>
            </a>
            <a mat-list-item routerLink="/items" routerLinkActive="active">
              <mat-icon>inventory</mat-icon>
              <span>Items</span>
            </a>
            <a mat-list-item routerLink="/incoming-payments" routerLinkActive="active">
              <mat-icon>payment</mat-icon>
              <span>Incoming Payments</span>
            </a>
            <a mat-list-item routerLink="/purchase-invoices" routerLinkActive="active">
              <mat-icon>receipt</mat-icon>
              <span>Purchase Invoices</span>
            </a>
            <a mat-list-item (click)="logout()">
              <mat-icon>logout</mat-icon>
              <span>Logout</span>
            </a>
          </mat-nav-list>
        </mat-sidenav>
        
        <mat-sidenav-content>
          <mat-toolbar color="primary">
            <span>SAP Management System</span>
            <span class="toolbar-spacer"></span>
            <button mat-icon-button (click)="logout()" *ngIf="showNavigation">
              <mat-icon>logout</mat-icon>
            </button>
          </mat-toolbar>
          
          <main class="main-content">
            <router-outlet></router-outlet>
          </main>
        </mat-sidenav-content>
      </mat-sidenav-container>
    </div>
    
    <div *ngIf="!showNavigation">
      <router-outlet></router-outlet>
    </div>
  `,
  styles: [`
    .active {
      background-color: rgba(255, 255, 255, 0.1) !important;
    }
    
    .mat-list-item {
      display: flex !important;
      align-items: center !important;
      gap: 16px !important;
    }
  `]
})
export class AppComponent implements OnInit {
  showNavigation = false;

  constructor(
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit() {
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe((event: NavigationEnd) => {
        this.showNavigation = !event.url.includes('/login');
      });
  }

  logout() {
    this.authService.logout().subscribe({
      next: () => {
        this.router.navigate(['/login']);
      },
      error: (error) => {
        console.error('Logout error:', error);
        this.router.navigate(['/login']);
      }
    });
  }
}