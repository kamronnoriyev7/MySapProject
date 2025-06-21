import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IncomingPaymentDto } from '../models/incoming-payment.models';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class IncomingPaymentService {
  private readonly API_URL = `${environment.apiUrl}/incomingpayment`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<IncomingPaymentDto[]> {
    return this.http.get<IncomingPaymentDto[]>(this.API_URL);
  }

  getById(id: number): Observable<IncomingPaymentDto> {
    return this.http.get<IncomingPaymentDto>(`${this.API_URL}/${id}`);
  }

  getFiltered(params: {
    filter?: string;
    select?: string;
    orderBy?: string;
    top?: number;
    skip?: number;
  }): Observable<IncomingPaymentDto[]> {
    let httpParams = new HttpParams();
    
    Object.entries(params).forEach(([key, value]) => {
      if (value !== undefined && value !== null) {
        httpParams = httpParams.set(key, value.toString());
      }
    });

    return this.http.get<IncomingPaymentDto[]>(`${this.API_URL}/filter`, { params: httpParams });
  }

  create(payment: IncomingPaymentDto): Observable<IncomingPaymentDto> {
    return this.http.post<IncomingPaymentDto>(this.API_URL, payment);
  }

  update(id: number, updatePayload: any): Observable<void> {
    return this.http.put<void>(`${this.API_URL}/${id}`, updatePayload);
  }

  cancel(id: number): Observable<void> {
    return this.http.delete<void>(`${this.API_URL}/${id}`);
  }

  getApprovalTemplates(id: number): Observable<string> {
    return this.http.get<string>(`${this.API_URL}/${id}/approval-templates`);
  }

  cancelByCurrentSystemDate(id: number): Observable<string> {
    return this.http.post<string>(`${this.API_URL}/${id}/cancel-by-current-system-date`, {});
  }

  requestApproveCancellation(id: number): Observable<string> {
    return this.http.post<string>(`${this.API_URL}/${id}/request-approve-cancellation`, {});
  }
}