import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

import { ItemService } from '../../core/services/item.service';
import { ItemDto } from '../../core/models/item.models';
import { LoadingComponent } from '../../shared/components/loading/loading.component';
import { ConfirmationDialogComponent } from '../../shared/components/confirmation-dialog/confirmation-dialog.component';

@Component({
  selector: 'app-items',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatCardModule,
    MatDialogModule,
    MatSnackBarModule,
    LoadingComponent
  ],
  template: `
    <div class="container">
      <div class="header">
        <h1>Items</h1>
        <button mat-raised-button color="primary" (click)="openCreateForm()">
          <mat-icon>add</mat-icon>
          Add Item
        </button>
      </div>

      <!-- Filter Section -->
      <mat-card class="filter-container">
        <form [formGroup]="filterForm" (ngSubmit)="applyFilter()">
          <div class="filter-row">
            <mat-form-field class="filter-field">
              <mat-label>Search</mat-label>
              <input matInput formControlName="search" placeholder="Search by item code or name">
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
          <mat-card-title>{{ editingItem ? 'Edit' : 'Create' }} Item</mat-card-title>
        </mat-card-header>
        <mat-card-content>
          <form [formGroup]="itemForm" (ngSubmit)="onSubmit()">
            <div class="form-row">
              <mat-form-field class="form-field">
                <mat-label>Item Code</mat-label>
                <input matInput formControlName="itemCode" placeholder="Enter item code">
                <mat-error *ngIf="itemForm.get('itemCode')?.hasError('required')">
                  Item code is required
                </mat-error>
              </mat-form-field>
              <mat-form-field class="form-field">
                <mat-label>Item Name</mat-label>
                <input matInput formControlName="itemName" placeholder="Enter item name">
                <mat-error *ngIf="itemForm.get('itemName')?.hasError('required')">
                  Item name is required
                </mat-error>
              </mat-form-field>
            </div>
            <div class="form-row">
              <mat-form-field class="form-field">
                <mat-label>Inventory UOM</mat-label>
                <input matInput formControlName="inventoryUOM" placeholder="e.g., pcs, kg, m">
                <mat-error *ngIf="itemForm.get('inventoryUOM')?.hasError('required')">
                  Inventory UOM is required
                </mat-error>
              </mat-form-field>
              <mat-form-field class="form-field">
                <mat-label>Items Group Code</mat-label>
                <input matInput type="number" formControlName="itemsGroupCode" placeholder="Enter group code">
                <mat-error *ngIf="itemForm.get('itemsGroupCode')?.hasError('required')">
                  Items group code is required
                </mat-error>
              </mat-form-field>
            </div>
            <mat-form-field class="form-field">
              <mat-label>Type Group</mat-label>
              <input matInput formControlName="u_TypeGroup" placeholder="Enter type group">
            </mat-form-field>
            <div class="action-buttons">
              <button mat-button type="button" (click)="cancelForm()">Cancel</button>
              <button mat-raised-button color="primary" type="submit" [disabled]="itemForm.invalid || isSubmitting">
                {{ editingItem ? 'Update' : 'Create' }}
              </button>
            </div>
          </form>
        </mat-card-content>
      </mat-card>

      <!-- Loading -->
      <app-loading *ngIf="isLoading"></app-loading>

      <!-- Data Table -->
      <mat-card *ngIf="!isLoading" class="table-container">
        <table mat-table [dataSource]="items" class="data-table">
          <ng-container matColumnDef="itemCode">
            <th mat-header-cell *matHeaderCellDef>Item Code</th>
            <td mat-cell *matCellDef="let item">{{ item.itemCode }}</td>
          </ng-container>

          <ng-container matColumnDef="itemName">
            <th mat-header-cell *matHeaderCellDef>Item Name</th>
            <td mat-cell *matCellDef="let item">{{ item.itemName }}</td>
          </ng-container>

          <ng-container matColumnDef="inventoryUOM">
            <th mat-header-cell *matHeaderCellDef>UOM</th>
            <td mat-cell *matCellDef="let item">{{ item.inventoryUOM }}</td>
          </ng-container>

          <ng-container matColumnDef="itemsGroupCode">
            <th mat-header-cell *matHeaderCellDef>Group Code</th>
            <td mat-cell *matCellDef="let item">{{ item.itemsGroupCode }}</td>
          </ng-container>

          <ng-container matColumnDef="u_TypeGroup">
            <th mat-header-cell *matHeaderCellDef>Type Group</th>
            <td mat-cell *matCellDef="let item">{{ item.u_TypeGroup || '-' }}</td>
          </ng-container>

          <ng-container matColumnDef="actions">
            <th mat-header-cell *matHeaderCellDef>Actions</th>
            <td mat-cell *matCellDef="let item">
              <button mat-icon-button color="primary" (click)="editItem(item)">
                <mat-icon>edit</mat-icon>
              </button>
              <button mat-icon-button color="warn" (click)="deleteItem(item.itemCode)">
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
export class ItemsComponent implements OnInit {
  items: ItemDto[] = [];
  displayedColumns: string[] = ['itemCode', 'itemName', 'inventoryUOM', 'itemsGroupCode', 'u_TypeGroup', 'actions'];
  isLoading = false;
  isSubmitting = false;
  showForm = false;
  editingItem: ItemDto | null = null;

  filterForm: FormGroup;
  itemForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private itemService: ItemService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar
  ) {
    this.filterForm = this.fb.group({
      search: ['']
    });

    this.itemForm = this.fb.group({
      itemCode: ['', Validators.required],
      itemName: ['', Validators.required],
      inventoryUOM: ['', Validators.required],
      itemsGroupCode: [0, Validators.required],
      u_TypeGroup: ['']
    });
  }

  ngOnInit(): void {
    this.loadItems();
  }

  loadItems(): void {
    this.isLoading = true;
    this.itemService.getAll().subscribe({
      next: (data) => {
        this.items = data;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading items:', error);
        this.snackBar.open('Error loading items', 'Close', { duration: 3000 });
        this.isLoading = false;
      }
    });
  }

  applyFilter(): void {
    const filterValues = this.filterForm.value;
    const params: any = {};

    if (filterValues.search) {
      params.filter = `contains(ItemCode,'${filterValues.search}') or contains(ItemName,'${filterValues.search}')`;
    }

    this.isLoading = true;
    this.itemService.getFiltered(params).subscribe({
      next: (data) => {
        this.items = data;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error filtering items:', error);
        this.snackBar.open('Error filtering items', 'Close', { duration: 3000 });
        this.isLoading = false;
      }
    });
  }

  clearFilter(): void {
    this.filterForm.reset();
    this.loadItems();
  }

  openCreateForm(): void {
    this.editingItem = null;
    this.itemForm.reset();
    this.showForm = true;
  }

  editItem(item: ItemDto): void {
    this.editingItem = item;
    this.itemForm.patchValue(item);
    this.showForm = true;
  }

  onSubmit(): void {
    if (this.itemForm.valid) {
      this.isSubmitting = true;
      const itemData: ItemDto = this.itemForm.value;

      if (this.editingItem && this.editingItem.itemCode) {
        this.itemService.update(this.editingItem.itemCode, itemData).subscribe({
          next: () => {
            this.snackBar.open('Item updated successfully', 'Close', { duration: 3000 });
            this.cancelForm();
            this.loadItems();
            this.isSubmitting = false;
          },
          error: (error) => {
            console.error('Error updating item:', error);
            this.snackBar.open('Error updating item', 'Close', { duration: 3000 });
            this.isSubmitting = false;
          }
        });
      } else {
        this.itemService.create(itemData).subscribe({
          next: () => {
            this.snackBar.open('Item created successfully', 'Close', { duration: 3000 });
            this.cancelForm();
            this.loadItems();
            this.isSubmitting = false;
          },
          error: (error) => {
            console.error('Error creating item:', error);
            this.snackBar.open('Error creating item', 'Close', { duration: 3000 });
            this.isSubmitting = false;
          }
        });
      }
    }
  }

  cancelForm(): void {
    this.showForm = false;
    this.editingItem = null;
    this.itemForm.reset();
  }

  deleteItem(itemCode: string | undefined): void {
    if (!itemCode) return;

    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      width: '400px',
      data: {
        title: 'Delete Item',
        message: `Are you sure you want to delete item ${itemCode}?`,
        confirmText: 'Delete',
        cancelText: 'Cancel'
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.itemService.delete(itemCode).subscribe({
          next: () => {
            this.snackBar.open('Item deleted successfully', 'Close', { duration: 3000 });
            this.loadItems();
          },
          error: (error) => {
            console.error('Error deleting item:', error);
            this.snackBar.open('Error deleting item', 'Close', { duration: 3000 });
          }
        });
      }
    });
  }
}