import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatCardModule } from '@angular/material/card';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

import { BusinessPartnerService } from '../../core/services/business-partner.service';
import { BusinessPartnersGetDto, BusinessPartnerDto } from '../../core/models/business-partner.models';
import { LoadingComponent } from '../../shared/components/loading/loading.component';
import { ConfirmationDialogComponent } from '../../shared/components/confirmation-dialog/confirmation-dialog.component';

@Component({
  selector: 'app-business-partners',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatCardModule,
    MatDialogModule,
    MatSnackBarModule,
    LoadingComponent
  ],
  template: `
    <div class="container">
      <div class="header">
        <h1>Business Partners</h1>
        <button mat-raised-button color="primary" (click)="openCreateForm()">
          <mat-icon>add</mat-icon>
          Add Business Partner
        </button>
      </div>

      <!-- Filter Section -->
      <mat-card class="filter-container">
        <form [formGroup]="filterForm" (ngSubmit)="applyFilter()">
          <div class="filter-row">
            <mat-form-field class="filter-field">
              <mat-label>Search</mat-label>
              <input matInput formControlName="search" placeholder="Search by name or code">
            </mat-form-field>
            <mat-form-field class="filter-field">
              <mat-label>Card Type</mat-label>
              <mat-select formControlName="cardType">
                <mat-option value="">All</mat-option>
                <mat-option value="cCustomer">Customer</mat-option>
                <mat-option value="cSupplier">Supplier</mat-option>
                <mat-option value="cLid">Lead</mat-option>
              </mat-select>
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
          <mat-card-title>{{ editingPartner ? 'Edit' : 'Create' }} Business Partner</mat-card-title>
        </mat-card-header>
        <mat-card-content>
          <form [formGroup]="partnerForm" (ngSubmit)="onSubmit()">
            <div class="form-row">
              <mat-form-field class="form-field">
                <mat-label>Card Code</mat-label>
                <input matInput formControlName="cardCode" placeholder="Enter card code">
                <mat-error *ngIf="partnerForm.get('cardCode')?.hasError('required')">
                  Card code is required
                </mat-error>
              </mat-form-field>
              <mat-form-field class="form-field">
                <mat-label>Card Name</mat-label>
                <input matInput formControlName="cardName" placeholder="Enter card name">
                <mat-error *ngIf="partnerForm.get('cardName')?.hasError('required')">
                  Card name is required
                </mat-error>
              </mat-form-field>
            </div>
            <mat-form-field class="form-field">
              <mat-label>Card Type</mat-label>
              <mat-select formControlName="cardType">
                <mat-option value="cCustomer">Customer</mat-option>
                <mat-option value="cSupplier">Supplier</mat-option>
                <mat-option value="cLid">Lead</mat-option>
              </mat-select>
              <mat-error *ngIf="partnerForm.get('cardType')?.hasError('required')">
                Card type is required
              </mat-error>
            </mat-form-field>
            <div class="action-buttons">
              <button mat-button type="button" (click)="cancelForm()">Cancel</button>
              <button mat-raised-button color="primary" type="submit" [disabled]="partnerForm.invalid || isSubmitting">
                {{ editingPartner ? 'Update' : 'Create' }}
              </button>
            </div>
          </form>
        </mat-card-content>
      </mat-card>

      <!-- Loading -->
      <app-loading *ngIf="isLoading"></app-loading>

      <!-- Data Table -->
      <mat-card *ngIf="!isLoading" class="table-container">
        <table mat-table [dataSource]="partners" class="data-table">
          <ng-container matColumnDef="cardCode">
            <th mat-header-cell *matHeaderCellDef>Card Code</th>
            <td mat-cell *matCellDef="let partner">{{ partner.cardCode }}</td>
          </ng-container>

          <ng-container matColumnDef="cardName">
            <th mat-header-cell *matHeaderCellDef>Card Name</th>
            <td mat-cell *matCellDef="let partner">{{ partner.cardName }}</td>
          </ng-container>

          <ng-container matColumnDef="cardType">
            <th mat-header-cell *matHeaderCellDef>Type</th>
            <td mat-cell *matCellDef="let partner">
              <span [class]="getCardTypeClass(partner.cardType)">
                {{ getCardTypeDisplay(partner.cardType) }}
              </span>
            </td>
          </ng-container>

          <ng-container matColumnDef="phone1">
            <th mat-header-cell *matHeaderCellDef>Phone</th>
            <td mat-cell *matCellDef="let partner">{{ partner.phone1 || '-' }}</td>
          </ng-container>

          <ng-container matColumnDef="creditLimit">
            <th mat-header-cell *matHeaderCellDef>Credit Limit</th>
            <td mat-cell *matCellDef="let partner">{{ partner.creditLimit | currency }}</td>
          </ng-container>

          <ng-container matColumnDef="actions">
            <th mat-header-cell *matHeaderCellDef>Actions</th>
            <td mat-cell *matCellDef="let partner">
              <button mat-icon-button color="primary" (click)="editPartner(partner)">
                <mat-icon>edit</mat-icon>
              </button>
              <button mat-icon-button color="warn" (click)="deletePartner(partner.cardCode)">
                <mat-icon>delete</mat-icon>
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

    .status-customer { color: #4caf50; }
    .status-supplier { color: #2196f3; }
    .status-lead { color: #ff9800; }

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
export class BusinessPartnersComponent implements OnInit {
  partners: BusinessPartnersGetDto[] = [];
  displayedColumns: string[] = ['cardCode', 'cardName', 'cardType', 'phone1', 'creditLimit', 'actions'];
  isLoading = false;
  isSubmitting = false;
  showForm = false;
  editingPartner: BusinessPartnersGetDto | null = null;

  filterForm: FormGroup;
  partnerForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private businessPartnerService: BusinessPartnerService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar
  ) {
    this.filterForm = this.fb.group({
      search: [''],
      cardType: ['']
    });

    this.partnerForm = this.fb.group({
      cardCode: ['', Validators.required],
      cardName: ['', Validators.required],
      cardType: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.loadPartners();
  }

  loadPartners(): void {
    this.isLoading = true;
    this.businessPartnerService.getAll().subscribe({
      next: (data) => {
        this.partners = data;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading partners:', error);
        this.snackBar.open('Error loading business partners', 'Close', { duration: 3000 });
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

    if (filterValues.cardType) {
      const typeFilter = `CardType eq '${filterValues.cardType}'`;
      params.filter = params.filter ? `(${params.filter}) and ${typeFilter}` : typeFilter;
    }

    this.isLoading = true;
    this.businessPartnerService.getFiltered(params).subscribe({
      next: (data) => {
        this.partners = data;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error filtering partners:', error);
        this.snackBar.open('Error filtering business partners', 'Close', { duration: 3000 });
        this.isLoading = false;
      }
    });
  }

  clearFilter(): void {
    this.filterForm.reset();
    this.loadPartners();
  }

  openCreateForm(): void {
    this.editingPartner = null;
    this.partnerForm.reset();
    this.showForm = true;
  }

  editPartner(partner: BusinessPartnersGetDto): void {
    this.editingPartner = partner;
    this.partnerForm.patchValue({
      cardCode: partner.cardCode,
      cardName: partner.cardName,
      cardType: partner.cardType
    });
    this.showForm = true;
  }

  onSubmit(): void {
    if (this.partnerForm.valid) {
      this.isSubmitting = true;
      const partnerData: BusinessPartnerDto = this.partnerForm.value;

      if (this.editingPartner) {
        this.businessPartnerService.update(this.editingPartner.cardCode!, partnerData).subscribe({
          next: () => {
            this.snackBar.open('Business partner updated successfully', 'Close', { duration: 3000 });
            this.cancelForm();
            this.loadPartners();
            this.isSubmitting = false;
          },
          error: (error) => {
            console.error('Error updating partner:', error);
            this.snackBar.open('Error updating business partner', 'Close', { duration: 3000 });
            this.isSubmitting = false;
          }
        });
      } else {
        this.businessPartnerService.create(partnerData).subscribe({
          next: () => {
            this.snackBar.open('Business partner created successfully', 'Close', { duration: 3000 });
            this.cancelForm();
            this.loadPartners();
            this.isSubmitting = false;
          },
          error: (error) => {
            console.error('Error creating partner:', error);
            this.snackBar.open('Error creating business partner', 'Close', { duration: 3000 });
            this.isSubmitting = false;
          }
        });
      }
    }
  }

  cancelForm(): void {
    this.showForm = false;
    this.editingPartner = null;
    this.partnerForm.reset();
  }

  deletePartner(cardCode: string): void {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      width: '400px',
      data: {
        title: 'Delete Business Partner',
        message: `Are you sure you want to delete business partner ${cardCode}?`,
        confirmText: 'Delete',
        cancelText: 'Cancel'
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.businessPartnerService.delete(cardCode).subscribe({
          next: () => {
            this.snackBar.open('Business partner deleted successfully', 'Close', { duration: 3000 });
            this.loadPartners();
          },
          error: (error) => {
            console.error('Error deleting partner:', error);
            this.snackBar.open('Error deleting business partner', 'Close', { duration: 3000 });
          }
        });
      }
    });
  }

  getCardTypeDisplay(cardType: string | undefined): string {
    switch (cardType) {
      case 'cCustomer': return 'Customer';
      case 'cSupplier': return 'Supplier';
      case 'cLid': return 'Lead';
      default: return cardType || '';
    }
  }

  getCardTypeClass(cardType: string | undefined): string {
    switch (cardType) {
      case 'cCustomer': return 'status-customer';
      case 'cSupplier': return 'status-supplier';
      case 'cLid': return 'status-lead';
      default: return '';
    }
  }
}