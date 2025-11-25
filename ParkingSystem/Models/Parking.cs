using ParkingSystem.Models;
using System.Linq; // Potrzebne do metody Any()

namespace ParkingSystem
{
    public class Parking
    {
        public readonly int LiczbaWierszy;
        public readonly int LiczbaKolumn;

        private string[,] SiatkaMiejsc;
        private List<Pojazd> PojazdyNaParkingu;
        private List<Transakcja> HistoriaTransakcji;
        
        public Parking(int wiersze, int kolumny)
        {
            LiczbaWierszy = wiersze;
            LiczbaKolumn = kolumny;

            SiatkaMiejsc = new string[wiersze, kolumny];
            PojazdyNaParkingu = new List<Pojazd>();
            HistoriaTransakcji = new List<Transakcja>();
        }

        public void Wizualizacja()
        {
            Console.WriteLine("\n--- AKTUALNY STAN PARKINGU ---");

            for (int r = 0; r < LiczbaWierszy; r++)
            {
                if (r % 3 == 2) 
                {
                    Console.WriteLine(new string('=', LiczbaKolumn * 3 + 1) + " PRZEJAZD");
                    continue;
                }

                for (int k = 0; k < LiczbaKolumn; k++)
                {
                    string status = string.IsNullOrEmpty(SiatkaMiejsc[r, k]) ? "[ ]" : "[X]"; 
                    Console.Write(status);
                }
                Console.WriteLine();
            }
        }
        
        public bool DodajPojazd(Pojazd nowyPojazd, int startWiersz, int startKolumna)
        {
            // 1. Walidacja Wstępna
            if (startWiersz < 0 || startWiersz >= LiczbaWierszy || startKolumna < 0 || startKolumna >= LiczbaKolumn)
            {
                Console.WriteLine($"Błąd: Współrzędne: ({startWiersz},{startKolumna}) poza parkingiem."); 
                return false;
            }
            if (startWiersz % 3 == 2)
            {
                Console.WriteLine($"Błąd: Nie mozna parkowac na przejezdzie."); 
                return false;
            }
            if (PojazdyNaParkingu.Any(p => p.NrRejestracyjny == nowyPojazd.NrRejestracyjny))
            {
                Console.WriteLine($"Błąd: Pojazd o numerze rejestracyjnym: {nowyPojazd.NrRejestracyjny} jest juz Parkingu.");
                return false;
            }
            
            // 2. Logika Alokacji
            List<(int r, int k)> polaDoZajecia = new List<(int r, int k)>();

            if (nowyPojazd.RozmiarWymagany == 1 || nowyPojazd.RozmiarWymagany == 2)
            {
                int requiredSize = nowyPojazd.RozmiarWymagany;

                if (startKolumna + requiredSize > LiczbaKolumn)
                {
                    Console.WriteLine($"Błąd: Brak miejsca dla {requiredSize} miejsc."); 
                    return false;
                }

                for (int k = 0; k < requiredSize; k++)
                {
                    // Sprawdzenie miejsca [wiersz, kolumna startowa + k]
                    if (!string.IsNullOrEmpty(SiatkaMiejsc[startWiersz, startKolumna + k])) 
                    {
                        Console.WriteLine($"Błąd: Miejsce w rzędzie {startWiersz} i kolumnie {startKolumna + k} jest juz zajete."); 
                        return false;
                    }
                    polaDoZajecia.Add((startWiersz, startKolumna + k));
                }

            }
            else if (nowyPojazd.RozmiarWymagany == 4)
            {
                // Walidacja 1: Czy starczy miejsca w wymiarach 2x2
                if (startWiersz + 1 >= LiczbaWierszy || startKolumna + 1 >= LiczbaKolumn)
                {
                    Console.WriteLine("BŁĄD: Za mało miejsca na autobus (wymagany blok 2x2).");
                    return false;
                }
                
                // Sprawdzanie dostępności całego bloku 2x2
                for (int r = startWiersz; r < startWiersz + 2; r++)
                {
                    // Weryfikacja: Czy drugi wiersz nie jest przypadkiem przejazdem (choć już zabezpieczone logicznie, warto sprawdzić)
                    if (r % 3 == 2)
                    {
                        Console.WriteLine($"BŁĄD: Rząd {r} jest przejazdem! Nie można parkować 2x2.");
                        return false;
                    }

                    for (int k = startKolumna; k < startKolumna + 2; k++)
                    {
                        if (!string.IsNullOrEmpty(SiatkaMiejsc[r, k]))
                        {
                            Console.WriteLine($"BŁĄD: Miejsce w rzędzie {r}, kolumnie {k} jest zajęte.");
                            return false;
                        }
                        polaDoZajecia.Add((r, k));
                    }
                }
            }
            else
            {
                Console.WriteLine("BŁĄD: Nieznany wymagany rozmiar pojazdu.");
                return false;
            }

            // 3. REZERWACJA MIEJSC I ZAPIS STANU 
            foreach (var pole in polaDoZajecia)
            {
                SiatkaMiejsc[pole.r, pole.k] = nowyPojazd.NrRejestracyjny;
                nowyPojazd.WspolrzedneZajete.Add(pole); 
            }

            PojazdyNaParkingu.Add(nowyPojazd);
            
            HistoriaTransakcji.Add(new Transakcja()
            {
                NrRejestracyjny = nowyPojazd.NrRejestracyjny,
                DataCzas = DateTime.Now,
                TypOperacji = "Przyjazd"
            });

            Console.WriteLine($"POWODZENIE: Dodano {nowyPojazd.WyswietlTypPojazdu()} ({nowyPojazd.NrRejestracyjny}).");
            return true;
        }
    }
}