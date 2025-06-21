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

import { EmployeeService } from '../../core/services/employee.service';
import { EmployeeDto } from '../../core/models/employee.models';
import { LoadingComponent } from '../../shared/components/loading/loading.component';
import { ConfirmationDialogComponent } from '../../shared/components/confirmation-dialog/confirmation-dialog.component';

@Component({
  selector: 'app-employees',
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
        <h1>Employees</h1>
        <button mat-raised-button color="primary" (click)="openCreateForm()">
          <mat-icon>add</mat-icon>
          Add Employee
        </button>
      </div>

      <!-- Filter Section -->
      <mat-card class="filter-container">
        <form [formGroup]="filterForm" (ngSubmit)="applyFilter()">
          <div class="filter-row">
            <mat-form-field class="filter-field">
              <mat-label>Search</mat-label>
              <input matInput formControlName="search" placeholder="Search by name or job title">
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
          <mat-card-title>{{ editingEmployee ? 'Edit' : 'Create' }} Employee</mat-card-title>
        </mat-card-header>
        <mat-card-content>
          <form [formGroup]="employeeForm" (ngSubmit)="onSubmit()">
            <div class="form-row">
              <mat-form-field class="form-field">
                <mat-label>First Name</mat-label>
                <input matInput formControlName="firstName" placeholder="Enter first name">
                <mat-error *ngIf="employeeForm.get('firstName')?.hasError('required')">
                  First name is required
                </mat-error>
              </mat-form-field>
              <mat-form-field class="form-field">
                <mat-label>Last Name</mat-label>
                <input matInput formControlName="lastName" placeholder="Enter last name">
                <mat-error *ngIf="employeeForm.get('lastName')?.hasError('required')">
                  Last name is required
                </mat-error>
              </mat-form-field>
            </div>
            <div class="form-row">
              <mat-form-field class="form-field">
                <mat-label>Job Title</mat-label>
                <input matInput formControlName="jobTitle" placeholder="Enter job title">
                <mat-error *ngIf="employeeForm.get('jobTitle')?.hasError('required')">
                  Job title is required
                </mat-error>
              </mat-form-field>
              <mat-form-field class="form-field">
                <mat-label>Work Country Code</mat-label>
                <input matInput formControlName="workCountryCode" placeholder="e.g., US, UZ">
                <mat-error *ngIf="employeeForm.get('workCountryCode')?.hasError('required')">
                  Work country code is required
                </mat-error>
              </mat-form-field>
            </div>
            <div class="form-row">
              <mat-form-field class="form-field">
                <mat-label>Department</mat-label>
                <input matInput type="number" formControlName="department" placeholder="Department code">
              </mat-form-field>
              <mat-form-field class="form-field">
                <mat-label>Branch</mat-label>
                <input matInput type="number" formControlName="branch" placeholder="Branch code">
              </mat-form-field>
            </div>
            <mat-form-field class="form-field">
              <mat-label>Remarks</mat-label>
              <textarea matInput formControlName="remarks" placeholder="Additional remarks" rows="3"></textarea>
            </mat-form-field>
            <div class="action-buttons">
              <button mat-button type="button" (click)="cancelForm()">Cancel</button>
              <button mat-raised-button color="primary" type="submit" [disabled]="employeeForm.invalid || isSubmitting">
                {{ editingEmployee ? 'Update' : 'Create' }}
              </button>
            </div>
          </form>
        </mat-card-content>
      </mat-card>

      <!-- Loading -->
      <app-loading *ngIf="isLoading"></app-loading>

      <!-- Data Table -->
      <mat-card *ngIf="!isLoading" class="table-container">
        <table mat-table [dataSource]="employees" class="data-table">
          <ng-container matColumnDef="employeeId">
            <th mat-header-cell *matHeaderCellDef>ID</th>
            <td mat-cell *matCellDef="let employee">{{ employee.employeeId }}</td>
          </ng-container>

          <ng-container matColumnDef="fullName">
            <th mat-header-cell *matHeaderCellDef>Full Name</th>
            <td mat-cell *matCellDef="let employee">{{ employee.firstName }} {{ employee.lastName }}</td>
          </ng-container>

          <ng-container matColumnDef="jobTitle">
            <th mat-header-cell *matHeaderCellDef>Job Title</th>
            <td mat-cell *matCellDef="let employee">{{ employee.jobTitle }}</td>
          </ng-container>

          <ng-container matColumnDef="workCountryCode">
            <th mat-header-cell *matHeaderCellDef>Country</th>
            <td mat-cell *matCellDef="let employee">{{ employee.workCountryCode }}</td>
          </ng-container>

          <ng-container matColumnDef="department">
            <th mat-header-cell *matHeaderCellDef>Department</th>
            <td mat-cell *matCellDef="let employee">{{ employee.department || '-' }}</td>
          </ng-container>

          <ng-container matColumnDef="actions">
            <th mat-header-cell *matHeaderCellDef>Actions</th>
            <td mat-cell *matCellDef="let employee">
              <button mat-icon-button color="primary" (click)="editEmployee(employee)">
                <mat-icon>edit</mat-icon>
              </button>
              <button mat-icon-button color="warn" (click)="deleteEmployee(employee.employeeId)">
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
export class EmployeesComponent implements OnInit {
  employees: EmployeeDto[] = [];
  displayedColumns: string[] = ['employeeId', 'fullName', 'jobTitle', 'workCountryCode', 'department', 'actions'];
  isLoading = false;
  isSubmitting = false;
  showForm = false;
  editingEmployee: EmployeeDto | null = null;

  filterForm: FormGroup;
  employeeForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private employeeService: EmployeeService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar
  ) {
    this.filterForm = this.fb.group({
      search: ['']
    });

    this.employeeForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      jobTitle: ['', Validators.required],
      workCountryCode: ['', Validators.required],
      department: [-2],
      branch: [-2],
      remarks: ['']
    });
  }

  ngOnInit(): void {
    this.loadEmployees();
  }

  loadEmployees(): void {
    this.isLoading = true;
    this.employeeService.getAll().subscribe({
      next: (data) => {
        this.employees = data;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading employees:', error);
        this.snackBar.open('Error loading employees', 'Close', { duration: 3000 });
        this.isLoading = false;
      }
    });
  }

  applyFilter(): void {
    const filterValues = this.filterForm.value;
    const params: any = {};

    if (filterValues.search) {
      params.filter = `contains(FirstName,'${filterValues.search}') or contains(LastName,'${filterValues.search}') or contains(JobTitle,'${filterValues.search}')`;
    }

    this.isLoading = true;
    this.employeeService.getFiltered(params).subscribe({
      next: (data) => {
        this.employees = data;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error filtering employees:', error);
        this.snackBar.open('Error filtering employees', 'Close', { duration: 3000 });
        this.isLoading = false;
      }
    });
  }

  clearFilter(): void {
    this.filterForm.reset();
    this.loadEmployees();
  }

  openCreateForm(): void {
    this.editingEmployee = null;
    this.employeeForm.reset({
      department: -2,
      branch: -2
    });
    this.showForm = true;
  }

  editEmployee(employee: EmployeeDto): void {
    this.editingEmployee = employee;
    this.employeeForm.patchValue(employee);
    this.showForm = true;
  }

  onSubmit(): void {
    if (this.employeeForm.valid) {
      this.isSubmitting = true;
      const employeeData: EmployeeDto = this.employeeForm.value;

      if (this.editingEmployee && this.editingEmployee.employeeId) {
        this.employeeService.update(this.editingEmployee.employeeId, employeeData).subscribe({
          next: () => {
            this.snackBar.open('Employee updated successfully', 'Close', { duration: 3000 });
            this.cancelForm();
            this.loadEmployees();
            this.isSubmitting = false;
          },
          error: (error) => {
            console.error('Error updating employee:', error);
            this.snackBar.open('Error updating employee', 'Close', { duration: 3000 });
            this.isSubmitting = false;
          }
        });
      } else {
        this.employeeService.create(employeeData).subscribe({
          next: () => {
            this.snackBar.open('Employee created successfully', 'Close', { duration: 3000 });
            this.cancelForm();
            this.loadEmployees();
            this.isSubmitting = false;
          },
          error: (error) => {
            console.error('Error creating employee:', error);
            this.snackBar.open('Error creating employee', 'Close', { duration: 3000 });
            this.isSubmitting = false;
          }
        });
      }
    }
  }

  cancelForm(): void {
    this.showForm = false;
    this.editingEmployee = null;
    this.employeeForm.reset();
  }

  deleteEmployee(employeeId: number | undefined): void {
    if (!employeeId) return;

    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      width: '400px',
      data: {
        title: 'Delete Employee',
        message: `Are you sure you want to delete employee ID ${employeeId}?`,
        confirmText: 'Delete',
        cancelText: 'Cancel'
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.employeeService.delete(employeeId).subscribe({
          next: () => {
            this.snackBar.open('Employee deleted successfully', 'Close', { duration: 3000 });
            this.loadEmployees();
          },
          error: (error) => {
            console.error('Error deleting employee:', error);
            this.snackBar.open('Error deleting employee', 'Close', { duration: 3000 });
          }
        });
      }
    });
  }
}