# Parking System

Konsolowy system zarządzania parkingiem napisany w C# (.NET 9).  
Aplikacja zarządza zajętością miejsc na siatce, obsługuje różne typy pojazdów, waliduje dane wejściowe oraz zapisuje przyjazdy i odjazdy do Microsoft SQL Server.

## Funkcjonalności

- **Interaktywne menu konsolowe (1-5)**
  - Dodaj pojazd
  - Usuń pojazd
  - Pokaż wizualizację parkingu
  - Pokaż historię transakcji
  - Wyjście
- **Model parkingu oparty o siatkę** z przejazdami
  - Wiersze przejazdów wyznacza reguła: `row % 3 == 2` (np. 2, 5, 8...)
  - Na przejazdach parkowanie jest zablokowane
- **Typy pojazdów i wymagane miejsce**
  - Motocykl: 1 miejsce
  - Samochód: 2 sąsiadujące miejsca (w jednym wierszu)
  - Ciężarówka: 3 sąsiadujące miejsca (w jednym wierszu)
  - Autobus: blok 2x2 (4 miejsca)
- **Walidacja danych**
  - Format numeru rejestracyjnego
  - Współrzędne w granicach parkingu
  - Dozwolone typy operacji (`Przyjazd`, `Odjazd`)
- **Logowanie transakcji do bazy (SQL Server)**
  - Przyjazd/odjazd zapisuje wpis w tabeli `Transakcje`
  - Historia pobierana z bazy (`TOP N`, od najnowszych)
- **Działanie awaryjne bez bazy**
  - Gdy brak połączenia z DB, logika parkingu nadal działa (bez trwałego zapisu transakcji)

## Technologie

- C# / .NET 9 (`net9.0`)
- Microsoft SQL Server
- `System.Data.SqlClient`
- `Microsoft.Extensions.Configuration`
- `Microsoft.Extensions.Configuration.Json`

## Wymagania

- .NET 9 SDK
- Działająca instancja SQL Server

## Konfiguracja

### 1) Klonowanie repozytorium

```bash
git clone https://github.com/DominikRybski/Projekt_Programowanie_Objektowe.git
cd Projekt_Programowanie_Objektowe/ParkingSystem
```

### 2) Przygotowanie bazy danych

Utwórz bazę (np. `ParkingDB`) oraz tabelę:

```sql
CREATE TABLE Transakcje (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    NrRejestracyjny NVARCHAR(50) NOT NULL,
    DataCzas DATETIME2 NOT NULL,
    TypOperacji NVARCHAR(50) NOT NULL
);
```

### 3) Ustawienie connection stringa

Użyj pliku `appsettings.json` w katalogu projektu:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=127.0.0.1,1433;Database=ParkingDB;User Id=sa;Password=TwojeHaslo;Encrypt=false;"
  }
}
```

Możesz zacząć od `appsettings_example.json`.

## Uruchomienie

```bash
dotnet run
```

Po starcie aplikacja tworzy parking `6 x 5` i wyświetla interaktywne menu.

## Obsługa menu

1. **Dodaj pojazd**
   - wybór typu (1-4)
   - podanie numeru rejestracyjnego
   - podanie wiersza i kolumny startowej
2. **Usuń pojazd**
   - podanie numeru rejestracyjnego
3. **Pokaż wizualizację parkingu**
   - `[ ]` wolne, `[X]` zajęte, przejazdy oznaczone osobno
4. **Pokaż historię transakcji**
   - wyświetla ostatnie wpisy z bazy (do 50)
5. **Wyjście**

## Zasady walidacji

- Numer rejestracyjny: 4-8 znaków, format np. `KR123`, `WA987`, `GD12345`
- Współrzędne muszą mieścić się w granicach parkingu
- Typ operacji musi być `Przyjazd` albo `Odjazd`
- Duplikat numeru rejestracyjnego na parkingu jest odrzucany

## Struktura projektu

```text
ParkingSystem/
├── Program.cs
├── MSSqlManager.cs
├── Models/
│   ├── Parking.cs
│   ├── Pojazd.cs
│   ├── Car.cs
│   ├── Motorcycle.cs
│   ├── Truck.cs
│   ├── Bus.cs
│   └── Transakcja.cs
├── Interfaces/
│   ├── IPojazd.cs
│   ├── ITransakcja.cs
│   └── IWalidowalne.cs
├── Helpers/
│   └── ValidationHelper.cs
├── appsettings.json
├── appsettings_example.json
└── ParkingSystem.csproj
```

## Uwagi

- Operacje SQL obsługuje `MSSqlManager`.
- Główna logika parkingu znajduje się w `Models/Parking`.
