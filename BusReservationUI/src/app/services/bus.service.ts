import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Station {
  id: number;
  name: string;
}

export interface RouteResult {
  routeId: number;
  routeName: string;
  availableSeats: number;
  totalSeats: number;
}

export interface SearchRequest {
  source: string;
  destination: string;
}

export interface BookingRequest {
  routeId: number;
  source: string;
  destination: string;
  seats: number;
}

@Injectable({
  providedIn: 'root'
})
export class BusService {
  private apiUrl = 'http://localhost:5000/api';

  constructor(private http: HttpClient) { }

  getStations(): Observable<Station[]> {
    return this.http.get<Station[]>(`${this.apiUrl}/stations`);
  }

  searchRoutes(request: SearchRequest): Observable<RouteResult[]> {
    return this.http.post<RouteResult[]>(`${this.apiUrl}/bus/search`, request);
  }

  bookSeats(request: BookingRequest): Observable<any> {
    return this.http.post(`${this.apiUrl}/bus/book`, request);
  }
}
