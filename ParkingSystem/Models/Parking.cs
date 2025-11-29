using ParkingSystem.Models;
using System.Linq;
using ParkingSystem.Services;

namespace ParkingSystem
{
    public class Parking
    {
        public readonly int LiczbaWierszy;
        public readonly int LiczbaKolumn;

        private string?[,] SiatkaMiejsc;
        private List<Pojazd> PojazdyNaParkingu;
        
        private MySqlManager _dbManager = new MySqlManager();
        
        public Parking(int wiersze, int kolumny)
        {
            LiczbaWierszy = wiersze;
            LiczbaKolumn = kolumny;

            SiatkaMiejsc = new string[wiersze, kolumny];
            PojazdyNaParkingu = new List<Pojazd>();
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
                
                if (startWiersz + 1 >= LiczbaWierszy || startKolumna + 1 >= LiczbaKolumn)
                {
                    Console.WriteLine("BŁĄD: Za mało miejsca na autobus (wymagany blok 2x2).");
                    return false;
                }
                
                
                for (int r = startWiersz; r < startWiersz + 2; r++)
                {
                    
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

            
            foreach (var pole in polaDoZajecia)
            {
                SiatkaMiejsc[pole.r, pole.k] = nowyPojazd.NrRejestracyjny;
                nowyPojazd.WspolrzedneZajete.Add(pole); 
            }

            PojazdyNaParkingu.Add(nowyPojazd);
            
            _dbManager.ZapiszTransakcje(nowyPojazd.NrRejestracyjny, DateTime.Now, "Przyjazd");

            Console.WriteLine($"POWODZENIE: Dodano {nowyPojazd.WyswietlTypPojazdu()} ({nowyPojazd.NrRejestracyjny}).");
            return true;
        }
        public bool UsunPojazd(string nrRejestracyjny)
        {
            Pojazd? pojazdDoUsuniecia = PojazdyNaParkingu.FirstOrDefault(pojazdDoUsuniecia => pojazdDoUsuniecia.NrRejestracyjny == nrRejestracyjny);
            
            if (pojazdDoUsuniecia == null)
            {
                Console.WriteLine($"BŁĄD: Pojazd o numerze {nrRejestracyjny} nie został znaleziony na parkingu.");
                return false;            
            }
        

        foreach (var pole in pojazdDoUsuniecia.WspolrzedneZajete)
        {
            SiatkaMiejsc[pole.Wiersz, pole.Kolumna] = null;
        }
        
        PojazdyNaParkingu.Remove(pojazdDoUsuniecia);

        _dbManager.ZapiszTransakcje(pojazdDoUsuniecia.NrRejestracyjny, DateTime.Now, "Odjazd");

        Console.WriteLine($"POWODZENIE: Usunięto {pojazdDoUsuniecia.WyswietlTypPojazdu()} ({nrRejestracyjny}). Miejsca zwolnione.");
        return true;
        }
    }
}


