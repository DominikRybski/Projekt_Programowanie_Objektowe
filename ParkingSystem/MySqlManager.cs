using MySql.Data.MySqlClient;

namespace ParkingSystem.Services 
{
    public class MySqlManager 
    {
        private const string ConnectionString = "Server=127.0.0.1;Port=3306;Database=ParkingDB;Uid=root;Pwd=2102;";
        
        public void ZapiszTransakcje(string nrRejestracyjny, DateTime dataCzas, string typOperacji)
        { 
            string query = "INSERT INTO Transakcje (NrRejestracyjny, DataCzas, TypOperacji) VALUES (@nr, @data, @typ)";

            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
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