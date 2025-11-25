namespace ParkingSystem.Models
{
    public class Car : Pojazd
    {

        public override int RozmiarWymagany => 2;

        public Car(string nrRejestracyjny) : base(nrRejestracyjny)
        {
            
        }

        public override string WyswietlTypPojazdu()
        {
            return "Samochód Osobowy";
        }
    }
}