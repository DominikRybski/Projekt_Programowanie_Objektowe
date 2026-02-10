namespace ParkingSystem.Models
{
    public class Truck : Pojazd
    {
        public override int RozmiarWymagany => 3;

        public Truck(string nrRejestracyjny) : base(nrRejestracyjny)
        {
            
        }

        public override string WyswietlTypPojazdu()
        {
            return "Ciężarówka";
        }
    }
}
