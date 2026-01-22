# Weather History App (.NET 8 + Angular)

This project is a small full-stack application built using **.NET 8 Web API** and **Angular**.  
It reads dates from a file, fetches historical weather data from the Open-Meteo API, stores results locally, and displays them in a simple UI.

---

## Prerequisites

- .NET SDK 8.0+
- Node.js 18+
- Angular CLI
- Internet connection

---

## How to Run the Backend

1. Navigate to the backend project folder:

   cd Weather.Api

2. Restore dependencies:

    dotnet restore


3. Run the API:

    dotnet run


4. Backend will start at:

https://localhost:7142/

Backend Endpoint
GET /api/weather


Returns weather data for all valid dates along with error information for invalid ones.


How to Run the UI

1. Navigate to the Angular UI project:

    cd WeatherUi


2. Install dependencies:

    npm install


3. Start the UI:

    ng serve


Open in browser:

http://localhost:4200


# Note
The backend port can vary per system, so I avoid hardcoding URLs.
I use a proxy configuration file to decouple the UI from backend ports and environments.
In this project I'm using proxy.conf.json to configure the port number.