using ParkingSystem.Interfaces;
using ParkingSystem.Helpers;

namespace ParkingSystem.Models
{
    public class Transakcja : ITransakcja
    {
        public string NrRejestracyjny{get; set;}
        public DateTime DataCzas {get; set;}

        public string TypOperacji {get; set;}

        public Transakcja()
        {
            NrRejestracyjny = string.Empty;
            TypOperacji = string.Empty;
        }

        public Transakcja(string nrRejestracyjny, DateTime dataCzas, string typOperacji)
        {
            if (!ValidationHelper.WalidujNumerRejestracyjny(nrRejestracyjny, out string? blad))
            {
                throw new ArgumentException(blad ?? "Nieprawidłowy numer rejestracyjny.");
            }

            if (!ValidationHelper.WalidujTypOperacji(typOperacji, out string? bladTypu))
            {
                throw new ArgumentException(bladTypu ?? "Nieprawidłowy typ operacji.");
            }

            NrRejestracyjny = nrRejestracyjny;
            DataCzas = dataCzas;
            TypOperacji = typOperacji;
        }

        public string PobierzInformacje()
        {
            return $"[{DataCzas:yyyy-MM-dd HH:mm:ss}] {TypOperacji}: {NrRejestracyjny}";
        }
    }
}