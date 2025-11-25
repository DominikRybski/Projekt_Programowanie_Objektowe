namespace ParkingSystem.Models
{
    public class Bus : Pojazd
    {
        public override int RozmiarWymagany => 4;
    
    public Bus(string nrRejestracyjny) : base(nrRejestracyjny)
        {
            
        }
        public override string WyswietlTypPojazdu()
        {
            return "Autobus";
        }
}
}