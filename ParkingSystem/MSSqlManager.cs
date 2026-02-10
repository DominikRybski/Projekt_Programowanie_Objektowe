using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ParkingSystem.Services 
{
    public class MSSqlManager
    {
        private readonly string _connectionString;

        public MSSqlManager()
        {
            // BaseDirectory kieruje do bin/Debug/net9.0/ (tam gdzie ląduje skopiowany plik)
            var basePath = AppDomain.CurrentDomain.BaseDirectory;

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                               ?? throw new InvalidOperationException("Brak ConnectionString");
        }
        public void ZapiszTransakcje(string nrRejestracyjny, DateTime dataCzas, string typOperacji)
        { 
            string query = "INSERT INTO Transakcje (NrRejestracyjny, DataCzas, TypOperacji) VALUES (@nr, @data, @typ)";

            using SqlConnection conn = new SqlConnection(_connectionString);
            conn.Open();
            using SqlCommand cmd = new SqlCommand(query, conn);
                {
                    cmd.Parameters.AddWithValue("@nr", nrRejestracyjny);
                    cmd.Parameters.AddWithValue("@data", dataCzas);
                    cmd.Parameters.AddWithValue("@typ", typOperacji);
                    
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }