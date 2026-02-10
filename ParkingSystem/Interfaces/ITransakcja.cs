namespace ParkingSystem.Interfaces
{
    public interface ITransakcja
    {
        string NrRejestracyjny { get; set; }
        DateTime DataCzas { get; set; }
        string TypOperacji { get; set; }
        string PobierzInformacje();
    }
}
