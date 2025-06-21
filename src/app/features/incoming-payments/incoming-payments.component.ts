import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatCardModule } from '@angular/material/card';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

import { IncomingPaymentService } from '../../core/services/incoming-payment.service';
import { IncomingPaymentDto } from '../../core/models/incoming-payment.models';
import { LoadingComponent } from '../../shared/components/loading/loading.component';
import { ConfirmationDialogComponent } from '../../shared/components/confirmation-dialog/confirmation-dialog.component';

@Component({
  selector: 'app-incoming-payments',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatCardModule,
    MatDialogModule,
    MatSnackBarModule,
    LoadingComponent
  ],
  template: `
    <div class="container">
      <div class="header">
        <h1>Incoming Payments</h1>
        <button mat-raised-button color="primary" (click)="openCreateForm()">
          <mat-icon>add</mat-icon>
          Add Payment
        </button>
      </div>

      <!-- Filter Section -->
      <mat-card class="filter-container">
        <form [formGroup]="filterForm" (ngSubmit)="applyFilter()">
          <div class="filter-row">
            <mat-form-field class="filter-field">
              <mat-label>Search</mat-label>
              <input matInput formControlName="search" placeholder="Search by card name or code">
            </mat-form-field>
            <button mat-raised-button type="submit" color="accent">
              <mat-icon>search</mat-icon>
              Filter
            </button>
            <button mat-button type="button" (click)="clearFilter()">
              <mat-icon>clear</mat-icon>
              Clear
            </button>
          </div>
        </form>
      </mat-card>

      <!-- Create/Edit Form -->
      <mat-card *ngIf="showForm" class="form-container">
        <mat-card-header>
          <mat-card-title>{{ editingPayment ? 'Edit' : 'Create' }} Incoming Payment</mat-card-title>
        </mat-card-header>
        <mat-card-content>
          <form [formGroup]="paymentForm" (ngSubmit)="onSubmit()">
            <div class="form-row">
              <mat-form-field class="form-field">
                <mat-label>Card Code</mat-label>
                <input matInput formControlName="cardCode" placeholder="Enter card code">
                <mat-error *ngIf="paymentForm.get('cardCode')?.hasError('required')">
                  Card code is required
                </mat-error>
              </mat-form-field>
              <mat-form-field class="form-field">
                <mat-label>Card Name</mat-label>
                <input matInput formControlName="cardName" placeholder="Enter card name">
              </mat-form-field>
            </div>
            <div class="form-row">
              <mat-form-field class="form-field">
                <mat-label>Document Date</mat-label>
                <input matInput [matDatepicker]="picker" formControlName="docDate">
                <mat-datepicker-toggle matSuffix [for]="picker"></mat-datepicker-toggle>
                <mat-datepicker #picker></mat-datepicker>
                <mat-error *ngIf="paymentForm.get('docDate')?.hasError('required')">
                  Document date is required
                </mat-error>
              </mat-form-field>
              <mat-form-field class="form-field">
                <mat-label>Cash Sum</mat-label>
                <input matInput type="number" formControlName="cashSum" placeholder="Enter amount">
                <mat-error *ngIf="paymentForm.get('cashSum')?.hasError('required')">
                  Cash sum is required
                </mat-error>
              </mat-form-field>
            </div>
            <div class="form-row">
              <mat-form-field class="form-field">
                <mat-label>Document Currency</mat-label>
                <input matInput formControlName="docCurrency" placeholder="e.g., USD, EUR">
              </mat-form-field>
            </div>
            <mat-form-field class="form-field">
              <mat-label>Remarks</mat-label>
              <textarea matInput formControlName="remarks" placeholder="Additional remarks" rows="3"></textarea>
            </mat-form-field>
            <div class="action-buttons">
              <button mat-button type="button" (click)="cancelForm()">Cancel</button>
              <button mat-raised-button color="primary" type="submit" [disabled]="paymentForm.invalid || isSubmitting">
                {{ editingPayment ? 'Update' : 'Create' }}
              </button>
            </div>
          </form>
        </mat-card-content>
      </mat-card>

      <!-- Loading -->
      <app-loading *ngIf="isLoading"></app-loading>

      <!-- Data Table -->
      <mat-card *ngIf="!isLoading" class="table-container">
        <table mat-table [dataSource]="payments" class="data-table">
          <ng-container matColumnDef="docEntry">
            <th mat-header-cell *matHeaderCellDef>Doc Entry</th>
            <td mat-cell *matCellDef="let payment">{{ payment.docEntry }}</td>
          </ng-container>

          <ng-container matColumnDef="docDate">
            <th mat-header-cell *matHeaderCellDef>Date</th>
            <td mat-cell *matCellDef="let payment">{{ payment.docDate | date:'short' }}</td>
          </ng-container>

          <ng-container matColumnDef="cardName">
            <th mat-header-cell *matHeaderCellDef>Card Name</th>
            <td mat-cell *matCellDef="let payment">{{ payment.cardName }}</td>
          </ng-container>

          <ng-container matColumnDef="cardCode">
            <th mat-header-cell *matHeaderCellDef>Card Code</th>
            <td mat-cell *matCellDef="let payment">{{ payment.cardCode }}</td>
          </ng-container>

          <ng-container matColumnDef="cashSum">
            <th mat-header-cell *matHeaderCellDef>Amount</th>
            <td mat-cell *matCellDef="let payment">{{ payment.cashSum | currency:payment.docCurrency }}</td>
          </ng-container>

          <ng-container matColumnDef="actions">
            <th mat-header-cell *matHeaderCellDef>Actions</th>
            <td mat-cell *matCellDef="let payment">
              <button mat-icon-button color="primary" (click)="editPayment(payment)">
                <mat-icon>edit</mat-icon>
              </button>
              <button mat-icon-button color="warn" (click)="cancelPayment(payment.docEntry)">
                <mat-icon>cancel</mat-icon>
              </button>
            </td>
          </ng-container>

          <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
          <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>
        </table>
      </mat-card>
    </div>
  `,
  styles: [`
    .header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 20px;
    }

    .form-row {
      display: flex;
      gap: 20px;
    }

    .form-row .form-field {
      flex: 1;
    }

    .table-container {
      margin-top: 20px;
    }

    @media (max-width: 768px) {
      .header {
        flex-direction: column;
        gap: 16px;
        align-items: stretch;
      }

      .form-row {
        flex-direction: column;
      }
    }
  `]
})
export class IncomingPaymentsComponent implements OnInit {
  payments: IncomingPaymentDto[] = [];
  displayedColumns: string[] = ['docEntry', 'docDate', 'cardName', 'cardCode', 'cashSum', 'actions'];
  isLoading = false;
  isSubmitting = false;
  showForm = false;
  editingPayment: IncomingPaymentDto | null = null;

  filterForm: FormGroup;
  paymentForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private incomingPaymentService: IncomingPaymentService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar
  ) {
    this.filterForm = this.fb.group({
      search: ['']
    });

    this.paymentForm = this.fb.group({
      cardCode: ['', Validators.required],
      cardName: [''],
      docDate: [new Date(), Validators.required],
      cashSum: [0, [Validators.required, Validators.min(0)]],
      docCurrency: ['USD'],
      remarks: ['']
    });
  }

  ngOnInit(): void {
    this.loadPayments();
  }

  loadPayments(): void {
    this.isLoading = true;
    this.incomingPaymentService.getAll().subscribe({
      next: (data) => {
        this.payments = data;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading payments:', error);
        this.snackBar.open('Error loading incoming payments', 'Close', { duration: 3000 });
        this.isLoading = false;
      }
    });
  }

  applyFilter(): void {
    const filterValues = this.filterForm.value;
    const params: any = {};

    if (filterValues.search) {
      params.filter = `contains(CardName,'${filterValues.search}') or contains(CardCode,'${filterValues.search}')`;
    }

    this.isLoading = true;
    this.incomingPaymentService.getFiltered(params).subscribe({
      next: (data) => {
        this.payments = data;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error filtering payments:', error);
        this.snackBar.open('Error filtering incoming payments', 'Close', { duration: 3000 });
        this.isLoading = false;
      }
    });
  }

  clearFilter(): void {
    this.filterForm.reset();
    this.loadPayments();
  }

  openCreateForm(): void {
    this.editingPayment = null;
    this.paymentForm.reset({
      docDate: new Date(),
      docCurrency: 'USD',
      cashSum: 0
    });
    this.showForm = true;
  }

  editPayment(payment: IncomingPaymentDto): void {
    this.editingPayment = payment;
    this.paymentForm.patchValue({
      ...payment,
      docDate: new Date(payment.docDate)
    });
    this.showForm = true;
  }

  onSubmit(): void {
    if (this.paymentForm.valid) {
      this.isSubmitting = true;
      const paymentData: IncomingPaymentDto = {
        ...this.paymentForm.value,
        docEntry: this.editingPayment?.docEntry || 0
      };

      if (this.editingPayment) {
        this.incomingPaymentService.update(this.editingPayment.docEntry, paymentData).subscribe({
          next: () => {
            this.snackBar.open('Payment updated successfully', 'Close', { duration: 3000 });
            this.cancelForm();
            this.loadPayments();
            this.isSubmitting = false;
          },
          error: (error) => {
            console.error('Error updating payment:', error);
            this.snackBar.open('Error updating payment', 'Close', { duration: 3000 });
            this.isSubmitting = false;
          }
        });
      } else {
        this.incomingPaymentService.create(paymentData).subscribe({
          next: () => {
            this.snackBar.open('Payment created successfully', 'Close', { duration: 3000 });
            this.cancelForm();
            this.loadPayments();
            this.isSubmitting = false;
          },
          error: (error) => {
            console.error('Error creating payment:', error);
            this.snackBar.open('Error creating payment', 'Close', { duration: 3000 });
            this.isSubmitting = false;
          }
        });
      }
    }
  }

  cancelForm(): void {
    this.showForm = false;
    this.editingPayment = null;
    this.paymentForm.reset();
  }

  cancelPayment(docEntry: number): void {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      width: '400px',
      data: {
        title: 'Cancel Payment',
        message: `Are you sure you want to cancel payment ${docEntry}?`,
        confirmText: 'Cancel Payment',
        cancelText: 'Keep Payment'
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.incomingPaymentService.cancel(docEntry).subscribe({
          next: () => {
            this.snackBar.open('Payment canceled successfully', 'Close', { duration: 3000 });
            this.loadPayments();
          },
          error: (error) => {
            console.error('Error canceling payment:', error);
            this.snackBar.open('Error canceling payment', 'Close', { duration: 3000 });
          }
        });
      }
    });
  }
}