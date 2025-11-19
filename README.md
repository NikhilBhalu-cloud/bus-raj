# Bus Reservation System

A complete bus reservation system built with Angular frontend and .NET Core Web API backend with SQLite database.

## Features

- Search for bus routes between stations
- View available seats for each route
- Book seats with intelligent seat allocation
- Segment-based booking (seats are allocated based on journey segments)
- Real-time seat availability updates

## Stations

The system includes the following stations:
- Ahmedabad
- Baroda
- Bharuch
- Surat
- Mumbai

## Routes

1. **Ahmedabad to Mumbai** - Stops at: Ahmedabad → Baroda → Bharuch → Surat → Mumbai (60 seats)
2. **Ahmedabad to Surat** - Stops at: Ahmedabad → Baroda → Bharuch → Surat (60 seats)

## Booking Logic

The system implements segment-based seat allocation:
- Bookings only affect the specific journey segment
- Example: A booking from Ahmedabad to Baroda will reduce available seats for:
  - Ahmedabad to Baroda ✓
  - Ahmedabad to Mumbai ✓
  - But NOT for Baroda to Mumbai ✗

## Technology Stack

### Backend
- .NET 8.0 Web API
- Entity Framework Core
- SQLite Database (file-based)
- CORS enabled for Angular frontend

### Frontend
- Angular 17
- TypeScript
- Standalone components
- HttpClient for API communication
- Simple, clean UI

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) (v18 or higher)
- [Angular CLI](https://angular.io/cli) (v17)

## Running the Application

### 1. Start the Backend API

```bash
cd BusReservationApi
dotnet run --launch-profile http
```

The API will start at `http://localhost:5000`

### 2. Start the Frontend

In a new terminal:

```bash
cd BusReservationUI
npm install
npm start
```

The Angular app will start at `http://localhost:4200`

## API Endpoints

### Get Stations
```
GET /api/stations
```

### Search Routes
```
POST /api/bus/search
Content-Type: application/json

{
  "source": "Ahmedabad",
  "destination": "Baroda"
}
```

### Book Seats
```
POST /api/bus/book
Content-Type: application/json

{
  "routeId": 1,
  "source": "Ahmedabad",
  "destination": "Baroda",
  "seats": 2
}
```

## Database

The SQLite database file (`busreservation.db`) is automatically created in the `BusReservationApi` directory on first run. The database is seeded with:
- 5 stations
- 2 routes

## Project Structure

```
bus-raj/
├── BusReservationApi/          # .NET Core Web API
│   ├── Controllers/            # API Controllers
│   ├── Data/                   # DbContext
│   ├── DTOs/                   # Data Transfer Objects
│   ├── Models/                 # Entity Models
│   └── busreservation.db       # SQLite database (auto-generated)
├── BusReservationUI/           # Angular Frontend
│   └── src/
│       └── app/
│           ├── services/       # API Services
│           └── app.component.* # Main component
└── README.md
```

## Testing Scenarios

### Scenario 1: Book from Ahmedabad to Baroda
1. Search: Ahmedabad → Baroda
2. Result: Both routes show 60 available seats
3. Book 2 seats on "Ahmedabad to Mumbai" route
4. Search again: Ahmedabad → Baroda
5. Result: "Ahmedabad to Mumbai" shows 58 seats

### Scenario 2: Search Baroda to Mumbai after booking
1. Search: Baroda → Mumbai
2. Result: "Ahmedabad to Mumbai" shows 60 seats (booking doesn't affect this segment)

### Scenario 3: Search full route
1. Search: Ahmedabad → Mumbai
2. Result: "Ahmedabad to Mumbai" shows 58 seats (affected by the booking)

## License

This project is created for demonstration purposes.