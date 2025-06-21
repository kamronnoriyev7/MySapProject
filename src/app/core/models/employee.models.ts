export interface EmployeeDto {
  employeeId?: number;
  firstName: string;
  lastName: string;
  jobTitle: string;
  department?: number;
  branch?: number;
  workCountryCode: string;
  remarks: string;
}