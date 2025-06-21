import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { LoginRequest, LoginResponse } from '../models/auth.models';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly API_URL = environment.apiUrl;
  private isAuthenticatedSubject = new BehaviorSubject<boolean>(false);
  public isAuthenticated$ = this.isAuthenticatedSubject.asObservable();

  constructor(private http: HttpClient) {
    this.checkAuthStatus();
  }

  login(loginRequest: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.API_URL}/auth/login`, loginRequest)
      .pipe(
        tap(response => {
          if (response.sessionId) {
            localStorage.setItem('sessionId', response.sessionId);
            localStorage.setItem('routeId', response.routeId);
            this.isAuthenticatedSubject.next(true);
          }
        })
      );
  }

  logout(): Observable<any> {
    return this.http.post(`${this.API_URL}/auth/logout`, {})
      .pipe(
        tap(() => {
          this.clearSession();
        })
      );
  }

  private clearSession(): void {
    localStorage.removeItem('sessionId');
    localStorage.removeItem('routeId');
    this.isAuthenticatedSubject.next(false);
  }

  private checkAuthStatus(): void {
    const sessionId = localStorage.getItem('sessionId');
    this.isAuthenticatedSubject.next(!!sessionId);
  }

  isAuthenticated(): boolean {
    return !!localStorage.getItem('sessionId');
  }

  getSessionId(): string | null {
    return localStorage.getItem('sessionId');
  }

  getRouteId(): string | null {
    return localStorage.getItem('routeId');
  }
}