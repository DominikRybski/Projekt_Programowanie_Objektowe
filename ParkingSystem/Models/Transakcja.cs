namespace ParkingSystem.Models
{
    public class Transakcja
    {
        public string NrRejestracyjny{get; set;}
        public DateTime DataCzas {get; set;}

        public string TypOperacji {get; set;}

        public Transakcja()
        {
            NrRejestracyjny = string.Empty;
            TypOperacji = string.Empty;
        }
    }
}