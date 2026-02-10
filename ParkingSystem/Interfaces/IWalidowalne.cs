namespace ParkingSystem.Interfaces
{
    public interface IWalidowalne
    {
        bool Waliduj();
        List<string> PobierzBledy();
    }
}
