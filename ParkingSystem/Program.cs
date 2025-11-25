// Program.cs
using ParkingSystem.Models; 

Console.WriteLine("System Obsługi Parkingu.");

Parking mojParking = new Parking(10, 5); 

// Wyświetlenie wymiarów
Console.WriteLine($"Parking stworzony: {mojParking.LiczbaWierszy} x {mojParking.LiczbaKolumn}"); 

// Nowa linia!
mojParking.Wizualizacja();