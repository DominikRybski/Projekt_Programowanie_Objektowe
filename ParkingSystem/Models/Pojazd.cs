using ParkingSystem.Interfaces;
using ParkingSystem.Helpers;

namespace ParkingSystem.Models
{
    public abstract class Pojazd : IPojazd
    {
        public string NrRejestracyjny {get; private set;} = string.Empty;
    

        public abstract int RozmiarWymagany {get;}

       public List<(int Wiersz, int Kolumna)> WspolrzedneZajete { get; private set; } = new List<(int Wiersz, int Kolumna)>();

        public Pojazd(string nrRejestracyjny)
        {
            if (!ValidationHelper.WalidujNumerRejestracyjny(nrRejestracyjny, out string? blad))
            {
                throw new ArgumentException(blad ?? "Nieprawidłowy numer rejestracyjny.");
            }
            NrRejestracyjny = nrRejestracyjny.ToUpper();
        }

        public abstract string WyswietlTypPojazdu();

        public virtual void WyswietlInfo()
        {
            Console.WriteLine($"--- INFORMACJE O POJEZDZIE ---");
            Console.WriteLine($"Numer Rejestracyjny: {NrRejestracyjny}");
            Console.WriteLine($"Typ: {WyswietlTypPojazdu()}");
            Console.WriteLine($"Zajmuje: {RozmiarWymagany} miejsc(a)");
        }

    }
}