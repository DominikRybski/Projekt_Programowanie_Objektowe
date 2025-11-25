namespace ParkingSystem.Models
{
    public abstract class Pojazd
    {
        public string NrRejestracyjny {get; private set;} = string.Empty;
    

        public abstract int RozmiarWymagany {get;}

       public List<(int Wiersz, int Kolumna)> WspolrzedneZajete { get; set; } = new List<(int Wiersz, int Kolumna)>();

        public Pojazd(string nrRejestracyjny)
        {
            NrRejestracyjny = nrRejestracyjny;
        }

        public abstract string WyswietlTypPojazdu();

        public virtual void WyswietlInfo()
        {
            Console.WriteLine($"--- INFORMACJE O POJEZDZIE ---");
            Console.WriteLine($"Numer Rejestracyjny: {NrRejestracyjny}");
            Console.WriteLine($"Typ: {WyswietlTypPojazdu}");
            Console.WriteLine($"Zajmuje: {RozmiarWymagany}");
        }

    }
}