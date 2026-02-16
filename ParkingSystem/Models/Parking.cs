using ParkingSystem.Models;
using System.Linq;
using ParkingSystem.Services;
using ParkingSystem.Helpers;

namespace ParkingSystem
{
    public class Parking
    {
        public readonly int LiczbaWierszy;
        public readonly int LiczbaKolumn;

        private string?[,] SiatkaMiejsc;
        private List<Pojazd> PojazdyNaParkingu;
        
        private MSSqlManager? _dbManager;
        
        public Parking(int wiersze, int kolumny)
        {
            if (wiersze <= 0 || kolumny <= 0)
            {
                throw new ArgumentException("Liczba wierszy i kolumn musi być większa od 0.");
            }

            LiczbaWierszy = wiersze;
            LiczbaKolumn = kolumny;

            SiatkaMiejsc = new string[wiersze, kolumny];
            PojazdyNaParkingu = new List<Pojazd>();
            
            try
            {
                _dbManager = new MSSqlManager();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OSTRZEŻENIE: Nie udało się połączyć z bazą danych: {ex.Message}");
                Console.WriteLine("System będzie działał bez zapisywania transakcji do bazy.");
                _dbManager = null;
            }
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
            try
            {
                if (nowyPojazd == null)
                {
                    throw new ArgumentNullException(nameof(nowyPojazd), "Pojazd nie może być null.");
                }

                if (!ValidationHelper.WalidujWspolrzedne(startWiersz, startKolumna, LiczbaWierszy, LiczbaKolumn, out string? bladWalidacji))
                {
                    Console.WriteLine($"Błąd: {bladWalidacji}");
                    return false;
                }

                if (startWiersz % 3 == 2)
                {
                    Console.WriteLine($"Błąd: Nie można parkować na przejezdzie."); 
                    return false;
                }

                if (PojazdyNaParkingu.Any(p => p.NrRejestracyjny.Equals(nowyPojazd.NrRejestracyjny, StringComparison.OrdinalIgnoreCase)))
                {
                    Console.WriteLine($"Błąd: Pojazd o numerze rejestracyjnym: {nowyPojazd.NrRejestracyjny} jest już na parkingu.");
                    return false;
                }
                
                List<(int r, int k)> polaDoZajecia = new List<(int r, int k)>();

                if (nowyPojazd.RozmiarWymagany == 1 || nowyPojazd.RozmiarWymagany == 2 || nowyPojazd.RozmiarWymagany == 3)
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
                            Console.WriteLine($"Błąd: Miejsce w rzędzie {startWiersz} i kolumnie {startKolumna + k} jest już zajęte."); 
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
                
                try
                {
                    _dbManager?.ZapiszTransakcje(nowyPojazd.NrRejestracyjny, DateTime.Now, "Przyjazd");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"OSTRZEŻENIE: Nie udało się zapisać transakcji do bazy: {ex.Message}");
                }

                Console.WriteLine($"POWODZENIE: Dodano {nowyPojazd.WyswietlTypPojazdu()} ({nowyPojazd.NrRejestracyjny}).");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"BŁĄD: Nie udało się dodać pojazdu: {ex.Message}");
                return false;
            }
        }

        public bool UsunPojazd(string nrRejestracyjny)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nrRejestracyjny))
                {
                    throw new ArgumentException("Numer rejestracyjny nie może być pusty.");
                }

                Pojazd? pojazdDoUsuniecia = PojazdyNaParkingu.FirstOrDefault(p => 
                    p.NrRejestracyjny.Equals(nrRejestracyjny, StringComparison.OrdinalIgnoreCase));
                
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

                try
                {
                    _dbManager?.ZapiszTransakcje(pojazdDoUsuniecia.NrRejestracyjny, DateTime.Now, "Odjazd");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"OSTRZEŻENIE: Nie udało się zapisać transakcji do bazy: {ex.Message}");
                }

                Console.WriteLine($"POWODZENIE: Usunięto {pojazdDoUsuniecia.WyswietlTypPojazdu()} ({nrRejestracyjny}). Miejsca zwolnione.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"BŁĄD: Nie udało się usunąć pojazdu: {ex.Message}");
                return false;
            }
        }

        public List<string> PobierzHistorieTransakcji(int limit = 50)
        {
            if (limit <= 0)
            {
                throw new ArgumentException("Limit musi być większy od 0.", nameof(limit));
            }

            try
            {
                return _dbManager?.PobierzHistorieTransakcji(limit) ?? new List<string>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OSTRZEŻENIE: Nie udało się pobrać historii transakcji: {ex.Message}");
                return new List<string>();
            }
        }
    }
}


