namespace ParkingSystem.Interfaces
{
    public interface IPojazd
    {
        string NrRejestracyjny { get; }
        int RozmiarWymagany { get; }
        string WyswietlTypPojazdu();
        void WyswietlInfo();
        List<(int Wiersz, int Kolumna)> WspolrzedneZajete { get; }
    }
}
