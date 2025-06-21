import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

import { AuthService } from '../../../core/services/auth.service';
import { LoginRequest } from '../../../core/models/auth.models';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    MatSnackBarModule
  ],
  template: `
    <div class="login-container">
      <mat-card class="login-card">
        <mat-card-header>
          <mat-card-title>SAP System Login</mat-card-title>
          <mat-card-subtitle>Enter your credentials to access the system</mat-card-subtitle>
        </mat-card-header>
        
        <mat-card-content>
          <form [formGroup]="loginForm" (ngSubmit)="onSubmit()">
            <mat-form-field class="form-field">
              <mat-label>Company Database</mat-label>
              <input matInput formControlName="companyDB" placeholder="Enter company database">
              <mat-error *ngIf="loginForm.get('companyDB')?.hasError('required')">
                Company database is required
              </mat-error>
            </mat-form-field>

            <mat-form-field class="form-field">
              <mat-label>Username</mat-label>
              <input matInput formControlName="userName" placeholder="Enter username">
              <mat-error *ngIf="loginForm.get('userName')?.hasError('required')">
                Username is required
              </mat-error>
            </mat-form-field>

            <mat-form-field class="form-field">
              <mat-label>Password</mat-label>
              <input matInput type="password" formControlName="password" placeholder="Enter password">
              <mat-error *ngIf="loginForm.get('password')?.hasError('required')">
                Password is required
              </mat-error>
            </mat-form-field>

            <div class="login-actions">
              <button 
                mat-raised-button 
                color="primary" 
                type="submit" 
                [disabled]="loginForm.invalid || isLoading"
                class="login-button">
                <mat-spinner diameter="20" *ngIf="isLoading"></mat-spinner>
                <span *ngIf="!isLoading">Login</span>
                <span *ngIf="isLoading">Logging in...</span>
              </button>
            </div>
          </form>
        </mat-card-content>
      </mat-card>
    </div>
  `,
  styles: [`
    .login-container {
      display: flex;
      justify-content: center;
      align-items: center;
      min-height: 100vh;
      background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
      padding: 20px;
    }

    .login-card {
      width: 100%;
      max-width: 400px;
      padding: 20px;
      box-shadow: 0 8px 32px rgba(0,0,0,0.1);
      border-radius: 16px;
    }

    .form-field {
      width: 100%;
      margin-bottom: 16px;
    }

    .login-actions {
      display: flex;
      justify-content: center;
      margin-top: 24px;
    }

    .login-button {
      width: 100%;
      height: 48px;
      font-size: 16px;
    }

    mat-card-header {
      text-align: center;
      margin-bottom: 24px;
    }

    mat-card-title {
      font-size: 24px;
      font-weight: 600;
      color: #333;
    }

    mat-card-subtitle {
      color: #666;
      margin-top: 8px;
    }
  `]
})
export class LoginComponent {
  loginForm: FormGroup;
  isLoading = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {
    this.loginForm = this.fb.group({
      companyDB: ['', Validators.required],
      userName: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  onSubmit(): void {
    if (this.loginForm.valid) {
      this.isLoading = true;
      const loginRequest: LoginRequest = this.loginForm.value;

      this.authService.login(loginRequest).subscribe({
        next: (response) => {
          this.isLoading = false;
          this.snackBar.open('Login successful!', 'Close', {
            duration: 3000,
            panelClass: ['success-snackbar']
          });
          this.router.navigate(['/dashboard']);
        },
        error: (error) => {
          this.isLoading = false;
          this.snackBar.open(
            error.error?.message || 'Login failed. Please check your credentials.',
            'Close',
            {
              duration: 5000,
              panelClass: ['error-snackbar']
            }
          );
        }
      });
    }
  }
}