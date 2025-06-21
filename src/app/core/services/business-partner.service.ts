import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BusinessPartnerDto, BusinessPartnersGetDto } from '../models/business-partner.models';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class BusinessPartnerService {
  private readonly API_URL = `${environment.apiUrl}/businesspartner`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<BusinessPartnersGetDto[]> {
    return this.http.get<BusinessPartnersGetDto[]>(this.API_URL);
  }

  getById(cardCode: string): Observable<BusinessPartnerDto> {
    return this.http.get<BusinessPartnerDto>(`${this.API_URL}/${cardCode}`);
  }

  getFiltered(params: {
    filter?: string;
    select?: string;
    orderBy?: string;
    top?: number;
    skip?: number;
  }): Observable<BusinessPartnersGetDto[]> {
    let httpParams = new HttpParams();
    
    Object.entries(params).forEach(([key, value]) => {
      if (value !== undefined && value !== null) {
        httpParams = httpParams.set(key, value.toString());
      }
    });

    return this.http.get<BusinessPartnersGetDto[]>(`${this.API_URL}/filter`, { params: httpParams });
  }

  create(partner: BusinessPartnerDto): Observable<BusinessPartnerDto> {
    return this.http.post<BusinessPartnerDto>(this.API_URL, partner);
  }

  update(cardCode: string, partner: BusinessPartnerDto): Observable<void> {
    return this.http.put<void>(`${this.API_URL}/${cardCode}`, partner);
  }

  delete(cardCode: string): Observable<void> {
    return this.http.delete<void>(`${this.API_URL}/${cardCode}`);
  }
}