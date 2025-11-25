namespace ParkingSystem.Models
{
    public class Motorcycle : Pojazd
    {
        public override int RozmiarWymagany => 1;

        public Motorcycle(string nrRejestracyjny) : base(nrRejestracyjny)
        {
            
        }

        public override string WyswietlTypPojazdu()
        {
            return "Motocykl";
        }
    }
}