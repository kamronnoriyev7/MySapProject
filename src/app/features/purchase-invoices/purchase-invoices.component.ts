import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormArray, Validators, ReactiveFormsModule } from '@angular/forms';
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

import { PurchaseInvoiceService } from '../../core/services/purchase-invoice.service';
import { PurchaseInvoiceDto, PurchaseInvoiceLineDto } from '../../core/models/purchase-invoice.models';
import { LoadingComponent } from '../../shared/components/loading/loading.component';
import { ConfirmationDialogComponent } from '../../shared/components/confirmation-dialog/confirmation-dialog.component';

@Component({
  selector: 'app-purchase-invoices',
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
        <h1>Purchase Invoices</h1>
        <button mat-raised-button color="primary" (click)="openCreateForm()">
          <mat-icon>add</mat-icon>
          Add Invoice
        </button>
      </div>

      <!-- Create/Edit Form -->
      <mat-card *ngIf="showForm" class="form-container">
        <mat-card-header>
          <mat-card-title>{{ editingInvoice ? 'Edit' : 'Create' }} Purchase Invoice</mat-card-title>
        </mat-card-header>
        <mat-card-content>
          <form [formGroup]="invoiceForm" (ngSubmit)="onSubmit()">
            <div class="form-row">
              <mat-form-field class="form-field">
                <mat-label>Card Code</mat-label>
                <input matInput formControlName="cardCode" placeholder="Enter supplier code">
                <mat-error *ngIf="invoiceForm.get('cardCode')?.hasError('required')">
                  Card code is required
                </mat-error>
              </mat-form-field>
              <mat-form-field class="form-field">
                <mat-label>Document Date</mat-label>
                <input matInput [matDatepicker]="picker" formControlName="docDate">
                <mat-datepicker-toggle matSuffix [for]="picker"></mat-datepicker-toggle>
                <mat-datepicker #picker></mat-datepicker>
                <mat-error *ngIf="invoiceForm.get('docDate')?.hasError('required')">
                  Document date is required
                </mat-error>
              </mat-form-field>
            </div>
            <div class="form-row">
              <mat-form-field class="form-field">
                <mat-label>Document Currency</mat-label>
                <input matInput formControlName="docCurrency" placeholder="e.g., USD, EUR">
                <mat-error *ngIf="invoiceForm.get('docCurrency')?.hasError('required')">
                  Document currency is required
                </mat-error>
              </mat-form-field>
            </div>
            <mat-form-field class="form-field">
              <mat-label>Comments</mat-label>
              <textarea matInput formControlName="comments" placeholder="Additional comments" rows="3"></textarea>
            </mat-form-field>

            <!-- Document Lines -->
            <div class="document-lines-section">
              <div class="section-header">
                <h3>Document Lines</h3>
                <button mat-raised-button type="button" color="accent" (click)="addDocumentLine()">
                  <mat-icon>add</mat-icon>
                  Add Line
                </button>
              </div>
              
              <div formArrayName="documentLines" class="document-lines">
                <mat-card *ngFor="let line of documentLines.controls; let i = index" [formGroupName]="i" class="line-card">
                  <div class="line-header">
                    <h4>Line {{ i + 1 }}</h4>
                    <button mat-icon-button type="button" color="warn" (click)="removeDocumentLine(i)" [disabled]="documentLines.length === 1">
                      <mat-icon>delete</mat-icon>
                    </button>
                  </div>
                  <div class="line-form">
                    <mat-form-field class="line-field">
                      <mat-label>Item Code</mat-label>
                      <input matInput formControlName="itemCode" placeholder="Enter item code">
                      <mat-error *ngIf="line.get('itemCode')?.hasError('required')">
                        Item code is required
                      </mat-error>
                    </mat-form-field>
                    <mat-form-field class="line-field">
                      <mat-label>Quantity</mat-label>
                      <input matInput type="number" formControlName="quantity" placeholder="Enter quantity">
                      <mat-error *ngIf="line.get('quantity')?.hasError('required')">
                        Quantity is required
                      </mat-error>
                    </mat-form-field>
                    <mat-form-field class="line-field">
                      <mat-label>Price</mat-label>
                      <input matInput type="number" formControlName="price" placeholder="Enter price">
                      <mat-error *ngIf="line.get('price')?.hasError('required')">
                        Price is required
                      </mat-error>
                    </mat-form-field>
                  </div>
                </mat-card>
              </div>
            </div>

            <div class="action-buttons">
              <button mat-button type="button" (click)="cancelForm()">Cancel</button>
              <button mat-raised-button color="primary" type="submit" [disabled]="invoiceForm.invalid || isSubmitting">
                {{ editingInvoice ? 'Update' : 'Create' }}
              </button>
            </div>
          </form>
        </mat-card-content>
      </mat-card>

      <!-- Loading -->
      <app-loading *ngIf="isLoading"></app-loading>

      <!-- Data Table -->
      <mat-card *ngIf="!isLoading" class="table-container">
        <table mat-table [dataSource]="invoices" class="data-table">
          <ng-container matColumnDef="cardCode">
            <th mat-header-cell *matHeaderCellDef>Card Code</th>
            <td mat-cell *matCellDef="let invoice">{{ invoice.cardCode }}</td>
          </ng-container>

          <ng-container matColumnDef="docDate">
            <th mat-header-cell *matHeaderCellDef>Date</th>
            <td mat-cell *matCellDef="let invoice">{{ invoice.docDate | date:'short' }}</td>
          </ng-container>

          <ng-container matColumnDef="docCurrency">
            <th mat-header-cell *matHeaderCellDef>Currency</th>
            <td mat-cell *matCellDef="let invoice">{{ invoice.docCurrency }}</td>
          </ng-container>

          <ng-container matColumnDef="comments">
            <th mat-header-cell *matHeaderCellDef>Comments</th>
            <td mat-cell *matCellDef="let invoice">{{ invoice.comments || '-' }}</td>
          </ng-container>

          <ng-container matColumnDef="lineCount">
            <th mat-header-cell *matHeaderCellDef>Lines</th>
            <td mat-cell *matCellDef="let invoice">{{ invoice.documentLines?.length || 0 }}</td>
          </ng-container>

          <ng-container matColumnDef="actions">
            <th mat-header-cell *matHeaderCellDef>Actions</th>
            <td mat-cell *matCellDef="let invoice; let i = index">
              <button mat-icon-button color="primary" (click)="editInvoice(invoice, i)">
                <mat-icon>edit</mat-icon>
              </button>
              <button mat-icon-button color="warn" (click)="cancelInvoice(i)">
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

    .document-lines-section {
      margin: 24px 0;
    }

    .section-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 16px;
    }

    .section-header h3 {
      margin: 0;
      color: #333;
    }

    .document-lines {
      display: flex;
      flex-direction: column;
      gap: 16px;
    }

    .line-card {
      padding: 16px;
      background: #f9f9f9;
    }

    .line-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 16px;
    }

    .line-header h4 {
      margin: 0;
      color: #666;
    }

    .line-form {
      display: flex;
      gap: 16px;
    }

    .line-field {
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

      .line-form {
        flex-direction: column;
      }

      .section-header {
        flex-direction: column;
        gap: 12px;
        align-items: stretch;
      }
    }
  `]
})
export class PurchaseInvoicesComponent implements OnInit {
  invoices: PurchaseInvoiceDto[] = [];
  displayedColumns: string[] = ['cardCode', 'docDate', 'docCurrency', 'comments', 'lineCount', 'actions'];
  isLoading = false;
  isSubmitting = false;
  showForm = false;
  editingInvoice: PurchaseInvoiceDto | null = null;
  editingIndex: number = -1;

  invoiceForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private purchaseInvoiceService: PurchaseInvoiceService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar
  ) {
    this.invoiceForm = this.fb.group({
      cardCode: ['', Validators.required],
      docDate: [new Date(), Validators.required],
      docCurrency: ['USD', Validators.required],
      comments: [''],
      documentLines: this.fb.array([this.createDocumentLine()])
    });
  }

  get documentLines(): FormArray {
    return this.invoiceForm.get('documentLines') as FormArray;
  }

  ngOnInit(): void {
    this.loadInvoices();
  }

  createDocumentLine(): FormGroup {
    return this.fb.group({
      itemCode: ['', Validators.required],
      quantity: [1, [Validators.required, Validators.min(0.01)]],
      price: [0, [Validators.require, Validators.min(0)]]
    });
  }

  addDocumentLine(): void {
    this.documentLines.push(this.createDocumentLine());
  }

  removeDocumentLine(index: number): void {
    if (this.documentLines.length > 1) {
      this.documentLines.removeAt(index);
    }
  }

  loadInvoices(): void {
    this.isLoading = true;
    this.purchaseInvoiceService.getAll().subscribe({
      next: (data) => {
        this.invoices = data;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading invoices:', error);
        this.snackBar.open('Error loading purchase invoices', 'Close', { duration: 3000 });
        this.isLoading = false;
      }
    });
  }

  openCreateForm(): void {
    this.editingInvoice = null;
    this.editingIndex = -1;
    this.invoiceForm.reset({
      docDate: new Date(),
      docCurrency: 'USD'
    });
    // Reset document lines to have one empty line
    while (this.documentLines.length > 1) {
      this.documentLines.removeAt(1);
    }
    this.documentLines.at(0).reset();
    this.showForm = true;
  }

  editInvoice(invoice: PurchaseInvoiceDto, index: number): void {
    this.editingInvoice = invoice;
    this.editingIndex = index;
    
    // Clear existing document lines
    while (this.documentLines.length > 0) {
      this.documentLines.removeAt(0);
    }
    
    // Add document lines from invoice
    invoice.documentLines.forEach(line => {
      this.documentLines.push(this.fb.group({
        itemCode: [line.itemCode, Validators.required],
        quantity: [line.quantity, [Validators.required, Validators.min(0.01)]],
        price: [line.price, [Validators.required, Validators.min(0)]]
      }));
    });

    this.invoiceForm.patchValue({
      cardCode: invoice.cardCode,
      docDate: new Date(invoice.docDate),
      docCurrency: invoice.docCurrency,
      comments: invoice.comments
    });
    
    this.showForm = true;
  }

  onSubmit(): void {
    if (this.invoiceForm.valid) {
      this.isSubmitting = true;
      const invoiceData: PurchaseInvoiceDto = this.invoiceForm.value;

      if (this.editingInvoice) {
        this.purchaseInvoiceService.update(this.editingIndex, invoiceData).subscribe({
          next: () => {
            this.snackBar.open('Invoice updated successfully', 'Close', { duration: 3000 });
            this.cancelForm();
            this.loadInvoices();
            this.isSubmitting = false;
          },
          error: (error) => {
            console.error('Error updating invoice:', error);
            this.snackBar.open('Error updating invoice', 'Close', { duration: 3000 });
            this.isSubmitting = false;
          }
        });
      } else {
        this.purchaseInvoiceService.create(invoiceData).subscribe({
          next: () => {
            this.snackBar.open('Invoice created successfully', 'Close', { duration: 3000 });
            this.cancelForm();
            this.loadInvoices();
            this.isSubmitting = false;
          },
          error: (error) => {
            console.error('Error creating invoice:', error);
            this.snackBar.open('Error creating invoice', 'Close', { duration: 3000 });
            this.isSubmitting = false;
          }
        });
      }
    }
  }

  cancelForm(): void {
    this.showForm = false;
    this.editingInvoice = null;
    this.editingIndex = -1;
    this.invoiceForm.reset();
  }

  cancelInvoice(id: number): void {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      width: '400px',
      data: {
        title: 'Cancel Invoice',
        message: `Are you sure you want to cancel invoice ${id}?`,
        confirmText: 'Cancel Invoice',
        cancelText: 'Keep Invoice'
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.purchaseInvoiceService.cancel(id).subscribe({
          next: () => {
            this.snackBar.open('Invoice canceled successfully', 'Close', { duration: 3000 });
            this.loadInvoices();
          },
          error: (error) => {
            console.error('Error canceling invoice:', error);
            this.snackBar.open('Error canceling invoice', 'Close', { duration: 3000 });
          }
        });
      }
    });
  }
}