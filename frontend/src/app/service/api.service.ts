import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  private apiUrl = 'http://localhost:5029'; // ajuste para sua porta do .NET

  constructor(private http: HttpClient) {}

  login(data: { username: string; password: string }): Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, data);
  }

  register(data: { username: string; password: string }): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, data);
  }

  getEventos(token: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/event`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
  }

  // outros métodos para eventos, funcionários etc.
}
