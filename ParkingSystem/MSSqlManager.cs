using System.Data.SqlClient;

namespace ParkingSystem.Services 
{
    public class MSSqlManager 
    {
        private const string ConnectionString = "Server=127.0.0.1,1433;Database=ParkingDB;User Id=sa;Password=ProjektParking2026!;Encrypt=false;";
        
        public void ZapiszTransakcje(string nrRejestracyjny, DateTime dataCzas, string typOperacji)
        { 
            string query = "INSERT INTO Transakcje (NrRejestracyjny, DataCzas, TypOperacji) VALUES (@nr, @data, @typ)";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nr", nrRejestracyjny);
                    cmd.Parameters.AddWithValue("@data", dataCzas);
                    cmd.Parameters.AddWithValue("@typ", typOperacji);
                    
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}