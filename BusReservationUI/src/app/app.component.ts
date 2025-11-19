import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { BusService, Station, RouteResult } from './services/bus.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit {
  title = 'Bus Reservation System';
  stations: Station[] = [];
  routes: RouteResult[] = [];
  selectedSource: string = '';
  selectedDestination: string = '';
  selectedSeats: { [key: number]: number } = {};
  searchPerformed: boolean = false;
  bookingMessage: string = '';

  constructor(private busService: BusService) {}

  ngOnInit() {
    this.loadStations();
  }

  loadStations() {
    this.busService.getStations().subscribe({
      next: (data) => {
        this.stations = data;
      },
      error: (error) => {
        console.error('Error loading stations:', error);
        alert('Error loading stations. Please make sure the API is running.');
      },
    });
  }

  searchRoutes() {
    if (!this.selectedSource || !this.selectedDestination) {
      alert('Please select both source and destination');
      return;
    }

    if (this.selectedSource === this.selectedDestination) {
      alert('Source and destination cannot be the same');
      return;
    }

    this.busService
      .searchRoutes({
        source: this.selectedSource,
        destination: this.selectedDestination,
      })
      .subscribe({
        next: (data) => {
          this.routes = data;
          this.searchPerformed = true;
          this.bookingMessage = '';
          this.selectedSeats = {};
        },
        error: (error) => {
          console.error('Error searching routes:', error);
          alert('Error searching routes');
        },
      });
  }

  bookRoute(route: RouteResult) {
    const seats = this.selectedSeats[route.routeId] || 0;

    if (seats <= 0) {
      alert('Please select number of seats');
      return;
    }

    if (seats > route.availableSeats) {
      alert(`Only ${route.availableSeats} seats available`);
      return;
    }

    this.busService
      .bookSeats({
        routeId: route.routeId,
        source: this.selectedSource,
        destination: this.selectedDestination,
        seats: seats,
      })
      .subscribe({
        next: (response) => {
          this.bookingMessage = `Booking successful! ${seats} seats booked on ${route.routeName}`;
          this.searchRoutes();
        },
        error: (error) => {
          console.error('Error booking seats:', error);
          alert('Error booking seats');
        },
      });
  }
}
