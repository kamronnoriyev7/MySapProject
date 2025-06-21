import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ItemDto } from '../models/item.models';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ItemService {
  private readonly API_URL = `${environment.apiUrl}/item`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<ItemDto[]> {
    return this.http.get<ItemDto[]>(this.API_URL);
  }

  getById(itemCode: string): Observable<ItemDto> {
    return this.http.get<ItemDto>(`${this.API_URL}/${itemCode}`);
  }

  getFiltered(params: {
    filter?: string;
    select?: string;
    orderBy?: string;
    top?: number;
    skip?: number;
  }): Observable<ItemDto[]> {
    let httpParams = new HttpParams();
    
    Object.entries(params).forEach(([key, value]) => {
      if (value !== undefined && value !== null) {
        httpParams = httpParams.set(key, value.toString());
      }
    });

    return this.http.get<ItemDto[]>(`${this.API_URL}/filter`, { params: httpParams });
  }

  create(item: ItemDto): Observable<ItemDto> {
    return this.http.post<ItemDto>(this.API_URL, item);
  }

  update(itemCode: string, item: ItemDto): Observable<void> {
    return this.http.put<void>(`${this.API_URL}/${itemCode}`, item);
  }

  delete(itemCode: string): Observable<void> {
    return this.http.delete<void>(`${this.API_URL}/${itemCode}`);
  }
}