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

  getAllEvents(): Observable<any> {
    return this.http.get(`${this.apiUrl}/event`);
  }

  getAllEmployees(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/employee`);
  }
  
  getEventsByEmployeeId(id: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/employee/${id}/events`);
  }
  

  // outros métodos para eventos, funcionários etc.
}
