using ParkingSystem.Models;

Console.WriteLine("=== System Obsługi Parkingu ===\n");

ParkingSystem.Parking mojParking;

try
{
    mojParking = new ParkingSystem.Parking(6, 5);
    Console.WriteLine($"Parking stworzony: {mojParking.LiczbaWierszy} x {mojParking.LiczbaKolumn}\n");
}
catch (Exception ex)
{
    Console.WriteLine($"\nKRYTYCZNY BŁĄD APLIKACJI: {ex.Message}");
    Console.WriteLine($"Stack trace: {ex.StackTrace}");
    return;
}

bool dziala = true;

while (dziala)
{
    WyswietlMenu();
    Console.Write("Twój wybór: ");
    string? wybor = Console.ReadLine();

    switch (wybor)
    {
        case "1":
            DodajPojazdZMenu(mojParking);
            break;
        case "2":
            UsunPojazdZMenu(mojParking);
            break;
        case "3":
            mojParking.Wizualizacja();
            break;
        case "4":
            PokazHistorieTransakcji(mojParking);
            break;
        case "5":
            dziala = false;
            break;
        default:
            Console.WriteLine("Nieprawidłowa opcja. Wybierz 1-5.");
            break;
    }
}

Console.WriteLine("\n=== System zakończył pracę ===");

static void WyswietlMenu()
{
    Console.WriteLine("\n--- MENU ---");
    Console.WriteLine("1. Dodaj pojazd");
    Console.WriteLine("2. Usuń pojazd");
    Console.WriteLine("3. Pokaż wizualizację parkingu");
    Console.WriteLine("4. Pokaż historię transakcji");
    Console.WriteLine("5. Wyjście");
}

static void DodajPojazdZMenu(ParkingSystem.Parking parking)
{
    try
    {
        Console.WriteLine("\nTyp pojazdu:");
        Console.WriteLine("1. Motocykl (1 miejsce)");
        Console.WriteLine("2. Samochód osobowy (2 miejsca)");
        Console.WriteLine("3. Ciężarówka (3 miejsca)");
        Console.WriteLine("4. Autobus (2x2)");

        Console.Write("Wybierz typ (1-4): ");
        string? typPojazdu = Console.ReadLine();

        Console.Write("Podaj numer rejestracyjny: ");
        string? nrRejestracyjny = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(nrRejestracyjny))
        {
            Console.WriteLine("BŁĄD: Numer rejestracyjny nie może być pusty.");
            return;
        }

        Pojazd nowyPojazd = typPojazdu switch
        {
            "1" => new Motorcycle(nrRejestracyjny),
            "2" => new Car(nrRejestracyjny),
            "3" => new Truck(nrRejestracyjny),
            "4" => new Bus(nrRejestracyjny),
            _ => throw new ArgumentException("Nieprawidłowy typ pojazdu.")
        };

        int startWiersz = WczytajLiczbe("Podaj wiersz startowy: ");
        int startKolumna = WczytajLiczbe("Podaj kolumnę startową: ");

        parking.DodajPojazd(nowyPojazd, startWiersz, startKolumna);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"BŁĄD: {ex.Message}");
    }
}

static void UsunPojazdZMenu(ParkingSystem.Parking parking)
{
    Console.Write("\nPodaj numer rejestracyjny pojazdu do usunięcia: ");
    string? nrRejestracyjny = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(nrRejestracyjny))
    {
        Console.WriteLine("BŁĄD: Numer rejestracyjny nie może być pusty.");
        return;
    }

    parking.UsunPojazd(nrRejestracyjny);
}

static void PokazHistorieTransakcji(ParkingSystem.Parking parking)
{
    Console.WriteLine("\n--- HISTORIA TRANSAKCJI ---");
    var historia = parking.PobierzHistorieTransakcji(50);

    if (historia.Count == 0)
    {
        Console.WriteLine("Brak transakcji do wyświetlenia.");
        return;
    }

    foreach (var wpis in historia)
    {
        Console.WriteLine(wpis);
    }
}

static int WczytajLiczbe(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        string? input = Console.ReadLine();

        if (int.TryParse(input, out int value))
        {
            return value;
        }

        Console.WriteLine("Nieprawidłowa liczba. Spróbuj ponownie.");
    }
}
