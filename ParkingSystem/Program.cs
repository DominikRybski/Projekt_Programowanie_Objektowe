using ParkingSystem;
using ParkingSystem.Models;
using System.Collections.Generic;

Console.WriteLine("=== System Obsługi Parkingu ===\n");

try
{
    Parking mojParking = new Parking(6, 5); 
    Console.WriteLine($"Parking stworzony: {mojParking.LiczbaWierszy} x {mojParking.LiczbaKolumn}\n"); 

    // Test podstawowych pojazdów
    try
    {
        Console.WriteLine("--- Dodawanie pojazdów ---");
        Pojazd motocykl = new Motorcycle("WA987");
        Pojazd samochod = new Car("KR123");
        Pojazd autobus = new Bus("GD000");
        Pojazd ciezarowka = new Truck("PO456");

        mojParking.DodajPojazd(motocykl, 1, 3);
        mojParking.DodajPojazd(samochod, 0, 2);
        mojParking.DodajPojazd(autobus, 3, 1);
        mojParking.DodajPojazd(ciezarowka, 0, 0);

        mojParking.Wizualizacja();
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"BŁĄD WALIDACJI: {ex.Message}");
    }

    // Test usuwania
    try
    {
        Console.WriteLine("\n--- Usuwanie pojazdów ---");
        mojParking.UsunPojazd("WA987");
        mojParking.UsunPojazd("KR123");
        
        mojParking.Wizualizacja();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"BŁĄD: {ex.Message}");
    }

    // Test nieprawidłowych danych
    try
    {
        Console.WriteLine("\n--- Test walidacji ---");
        Pojazd nieprawidlowyPojazd = new Car("XYZ"); // Za krótki numer
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"Przechwycono błąd walidacji (oczekiwane): {ex.Message}");
    }

    // Dalsze czyszczenie
    try
    {
        Console.WriteLine("\n--- Finalne usuwanie ---");
        mojParking.UsunPojazd("GD000");
        mojParking.UsunPojazd("PO456");
        
        mojParking.Wizualizacja();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"BŁĄD: {ex.Message}");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"\nKRYTYCZNY BŁĄD APLIKACJI: {ex.Message}");
    Console.WriteLine($"Stack trace: {ex.StackTrace}");
    return;
}

Console.WriteLine("\n=== System zakończył pracę ===");
