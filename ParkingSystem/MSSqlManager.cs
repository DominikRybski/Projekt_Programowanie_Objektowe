using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ParkingSystem.Services 
{
    public class MSSqlManager
    {
        private readonly string _connectionString;

        public MSSqlManager()
        {
            try
            {
                var basePath = AppDomain.CurrentDomain.BaseDirectory;

                var configuration = new ConfigurationBuilder()
                    .SetBasePath(basePath)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                _connectionString = configuration.GetConnectionString("DefaultConnection") 
                                   ?? throw new InvalidOperationException("Brak ConnectionString w appsettings.json");
                
                // Test połączenia
                TestConnection();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Błąd inicjalizacji połączenia z bazą danych: {ex.Message}", ex);
            }
        }

        private void TestConnection()
        {
            try
            {
                using SqlConnection conn = new SqlConnection(_connectionString);
                conn.Open();
            }
            catch (SqlException ex)
            {
                throw new InvalidOperationException($"Nie można połączyć się z bazą danych: {ex.Message}", ex);
            }
        }

        public void ZapiszTransakcje(string nrRejestracyjny, DateTime dataCzas, string typOperacji)
        {
            if (string.IsNullOrWhiteSpace(nrRejestracyjny))
            {
                throw new ArgumentException("Numer rejestracyjny nie może być pusty.", nameof(nrRejestracyjny));
            }

            if (string.IsNullOrWhiteSpace(typOperacji))
            {
                throw new ArgumentException("Typ operacji nie może być pusty.", nameof(typOperacji));
            }

            try
            {
                string query = "INSERT INTO Transakcje (NrRejestracyjny, DataCzas, TypOperacji) VALUES (@nr, @data, @typ)";

                using SqlConnection conn = new SqlConnection(_connectionString);
                conn.Open();
                
                using SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nr", nrRejestracyjny);
                cmd.Parameters.AddWithValue("@data", dataCzas);
                cmd.Parameters.AddWithValue("@typ", typOperacji);
                
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new InvalidOperationException($"Błąd zapisu transakcji do bazy danych: {ex.Message}", ex);
            }
        }

        public List<string> PobierzHistorieTransakcji(int limit = 50)
        {
            if (limit <= 0)
            {
                throw new ArgumentException("Limit musi być większy od 0.", nameof(limit));
            }

            try
            {
                List<string> historia = new List<string>();
                string query = "SELECT TOP (@limit) NrRejestracyjny, DataCzas, TypOperacji FROM Transakcje ORDER BY DataCzas DESC";

                using SqlConnection conn = new SqlConnection(_connectionString);
                conn.Open();

                using SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@limit", limit);

                using SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string nrRejestracyjny = reader["NrRejestracyjny"]?.ToString() ?? "BRAK";
                    string typOperacji = reader["TypOperacji"]?.ToString() ?? "Nieznany";
                    DateTime dataCzas = reader["DataCzas"] is DateTime data ? data : DateTime.MinValue;

                    historia.Add($"[{dataCzas:yyyy-MM-dd HH:mm:ss}] {typOperacji}: {nrRejestracyjny}");
                }

                return historia;
            }
            catch (SqlException ex)
            {
                throw new InvalidOperationException($"Błąd pobierania historii transakcji z bazy danych: {ex.Message}", ex);
            }
        }
    }
}