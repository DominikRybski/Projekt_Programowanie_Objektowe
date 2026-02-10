# Parking System

This project is a console-based parking lot management system developed in C#. It simulates the operations of a parking lot, including adding and removing vehicles of different types, visualizing the parking grid, and logging events to a SQL Server database.

## Features

- **Grid-Based Parking:** Manages a parking lot represented as a grid of spaces with designated driveways.
- **Multiple Vehicle Types:** Supports different vehicle types with specific space requirements:
  - **Motorcycle:** Requires 1 parking space.
  - **Car:** Requires 2 adjacent parking spaces.
  - **Truck:** Requires 3 adjacent parking spaces.
  - **Bus:** Requires a 2x2 block of parking spaces.
- **Interface-Based Architecture:** Implements clean interfaces (`IPojazd`, `ITransakcja`, `IWalidowalne`) for maintainable code structure.
- **Data Validation:** Comprehensive validation of registration numbers, coordinates, and operation types.
- **Exception Handling:** Robust error handling throughout the application with detailed error messages.
- **Console Visualization:** Provides a simple text-based visualization of the parking lot's current occupancy.
- **Parking Logic:** Implements rules for parking, such as checking for available space, preventing parking on driveways, and avoiding conflicts with already parked vehicles.
- **Database Logging:** Records all vehicle arrivals and departures as transactions in a Microsoft SQL Server database.

## Technologies Used

- **C#** and **.NET 9.0**
- **Microsoft SQL Server** for data persistence.
- **Microsoft.Extensions.Configuration** for handling application settings.

## Setup and Configuration

Follow these steps to set up and run the project locally.

### 1. Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download)
- [Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (Express edition is sufficient)

### 2. Clone the Repository

```bash
git clone https://github.com/DominikRybski/Projekt_Programowanie_Objektowe.git
cd Projekt_Programowanie_Objektowe/ParkingSystem
```

### 3. Database Setup

1.  Connect to your SQL Server instance.
2.  Create a new database (e.g., `ParkingDB`).
3.  Run the following SQL script in your new database to create the `Transakcje` table:

```sql
CREATE TABLE Transakcje (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    NrRejestracyjny NVARCHAR(50) NOT NULL,
    DataCzas DATETIME2 NOT NULL,
    TypOperacji NVARCHAR(50) NOT NULL
);
```

### 4. Configure Connection String

1.  In the `ParkingSystem` directory, create a new file named `appsettings.json`.
2.  Copy the contents of `appsettings_example.json` into your new `appsettings.json` file.
3.  Update the `DefaultConnection` string with your SQL Server credentials.

**`appsettings.json`:**

```json
{
	"ConnectionStrings": {
		"DefaultConnection": "Server=YOUR_SERVER_ADDRESS;Database=ParkingDB;User Id=YOUR_USERNAME;Password=YOUR_PASSWORD;Encrypt=false;"
	}
}
```

## Usage

Navigate to the `ParkingSystem` directory in your terminal and run the application using the .NET CLI:

```bash
dotnet run
```

The program will execute the predefined simulation in `Program.cs`, which demonstrates adding and removing vehicles, and will print the state of the parking lot to the console after each major operation.

### Example Execution Flow

The main program (`Program.cs`) demonstrates the parking system functionality with comprehensive error handling:

```csharp
try
{
    // Creates a 6x5 parking lot
    Parking mojParking = new Parking(6, 5);

    // Creates different vehicle types with validated registration numbers
    Pojazd motocykl = new Motorcycle("WA987");
    Pojazd samochod = new Car("KR123");
    Pojazd autobus = new Bus("GD000");
    Pojazd ciezarowka = new Truck("PO456");

    // Parks the vehicles at specified coordinates
    mojParking.DodajPojazd(motocykl, 1, 3);
    mojParking.DodajPojazd(samochod, 0, 2);
    mojParking.DodajPojazd(autobus, 3, 1);
    mojParking.DodajPojazd(ciezarowka, 0, 0);

    // Displays the current state of the parking lot
    mojParking.Wizualizacja();

    // Removes vehicles from the lot
    mojParking.UsunPojazd("WA987");
    mojParking.UsunPojazd("KR123");

    mojParking.Wizualizacja();
}
catch (Exception ex)
{
    Console.WriteLine($"ERROR: {ex.Message}");
}
```

### Validation

The system validates:

- **Registration numbers:** Must be 4-8 characters (2-3 letters + 2-5 alphanumeric characters, e.g., KR123, WA987)
- **Coordinates:** Must be within parking lot boundaries
- **Operation types:** Only "Przyjazd" (arrival) and "Odjazd" (departure) are allowed

## Project Structure

```
ParkingSystem/
├── Program.cs                    # Entry point with demonstration scenarios
├── Models/                       # Domain models
│   ├── Pojazd.cs                # Abstract base class for all vehicles (implements IPojazd)
│   ├── Car.cs                   # Car implementation (2 spaces)
│   ├── Motorcycle.cs            # Motorcycle implementation (1 space)
│   ├── Bus.cs                   # Bus implementation (4 spaces in 2x2 grid)
│   ├── Truck.cs                 # Truck implementation (3 spaces)
│   ├── Parking.cs               # Core parking lot management logic
│   └── Transakcja.cs            # Transaction record model (implements ITransakcja)
├── Interfaces/                   # Interface definitions
│   ├── IPojazd.cs               # Vehicle interface
│   ├── ITransakcja.cs           # Transaction interface
│   └── IWalidowalne.cs          # Validation interface
├── Helpers/                      # Utility classes
│   └── ValidationHelper.cs      # Data validation methods
├── Services/                     # Service layer
│   └── MSSqlManager.cs          # Database operations handler
├── appsettings.json             # Application configuration (not in repo)
├── appsettings_example.json     # Configuration template
└── ParkingSystem.csproj         # Project file
```
