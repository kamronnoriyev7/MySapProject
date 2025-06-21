import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { EmployeeDto } from '../models/employee.models';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  private readonly API_URL = `${environment.apiUrl}/employee`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<EmployeeDto[]> {
    return this.http.get<EmployeeDto[]>(this.API_URL);
  }

  getById(id: number): Observable<EmployeeDto> {
    return this.http.get<EmployeeDto>(`${this.API_URL}/${id}`);
  }

  getFiltered(params: {
    filter?: string;
    select?: string;
    orderBy?: string;
    top?: number;
    skip?: number;
  }): Observable<EmployeeDto[]> {
    let httpParams = new HttpParams();
    
    Object.entries(params).forEach(([key, value]) => {
      if (value !== undefined && value !== null) {
        httpParams = httpParams.set(key, value.toString());
      }
    });

    return this.http.get<EmployeeDto[]>(`${this.API_URL}/filter`, { params: httpParams });
  }

  create(employee: EmployeeDto): Observable<EmployeeDto> {
    return this.http.post<EmployeeDto>(this.API_URL, employee);
  }

  update(id: number, employee: EmployeeDto): Observable<void> {
    return this.http.put<void>(`${this.API_URL}/${id}`, employee);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.API_URL}/${id}`);
  }
}