import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PurchaseInvoiceDto } from '../models/purchase-invoice.models';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PurchaseInvoiceService {
  private readonly API_URL = `${environment.apiUrl}/purchaseinvoices`;

  constructor(private http: HttpClient) {}

  getAll(skip: number = 0, top: number = 10): Observable<PurchaseInvoiceDto[]> {
    const params = new HttpParams()
      .set('skip', skip.toString())
      .set('top', top.toString());
    
    return this.http.get<PurchaseInvoiceDto[]>(this.API_URL, { params });
  }

  getById(id: number): Observable<PurchaseInvoiceDto> {
    return this.http.get<PurchaseInvoiceDto>(`${this.API_URL}/${id}`);
  }

  create(invoice: PurchaseInvoiceDto): Observable<PurchaseInvoiceDto> {
    return this.http.post<PurchaseInvoiceDto>(this.API_URL, invoice);
  }

  update(id: number, invoice: PurchaseInvoiceDto): Observable<void> {
    return this.http.put<void>(`${this.API_URL}/${id}`, invoice);
  }

  cancel(id: number): Observable<string> {
    return this.http.post<string>(`${this.API_URL}/${id}/cancel`, {});
  }
}