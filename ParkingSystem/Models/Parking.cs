using ParkingSystem.Models;
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
            LiczbaKolumn =kolumny;

            SiatkaMiejsc = new string[wiersze,kolumny];
            PojazdyNaParkingu = new List<Pojazd>();
            HistoriaTransakcji = new List<Transakcja>();
        }
        public void Wizualizacja()
    {
        Console.WriteLine("\n--- AKTUALNY STAN PARKINGU ---");
        
        for (int r = 0; r < LiczbaWierszy; r++)
        {
            // Wymaganie: Co drugi wiersz (nieparzysty) to przejazd
            if (r % 2 != 0) 
            {
                // Rysujemy linię przejazdu
                Console.WriteLine(new string('=', LiczbaKolumn * 3 + 1) + " PRZEJAZD");
                continue; // Przechodzimy do kolejnego wiersza (pętli)
            }

            for (int k = 0; k < LiczbaKolumn; k++)
            {
                // Sprawdzenie stanu z SiatkiMiejsc (która trzyma string/null)
                string status = string.IsNullOrEmpty(SiatkaMiejsc[r, k]) ? "[ ]" : "[X]"; 
                Console.Write(status);
            }
            Console.WriteLine(); // Nowa linia po każdej kolumnie
        }
    }
    }
}