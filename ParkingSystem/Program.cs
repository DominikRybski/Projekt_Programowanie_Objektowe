using ParkingSystem;
using ParkingSystem.Models;

Console.WriteLine("System Obsługi Parkingu.");

Parking mojParking = new Parking(6, 5); 

Console.WriteLine($"Parking stworzony: {mojParking.LiczbaWierszy} x {mojParking.LiczbaKolumn}"); 

Pojazd samochod = new Car("KR123");
Pojazd motocykl = new Motorcycle("WA987");
Pojazd autobus = new Bus("GD000");

mojParking.DodajPojazd(motocykl, 0, 0);
mojParking.DodajPojazd(samochod, 0,3);
mojParking.DodajPojazd(autobus, 3, 0);

mojParking.Wizualizacja();