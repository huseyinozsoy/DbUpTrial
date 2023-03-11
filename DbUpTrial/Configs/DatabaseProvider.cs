using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace DbUpTrial.Configs
{
    public interface IDatabaseProvider
    {
        IDbConnection GetConnection();
    }
    public class DatabaseProvider: IDatabaseProvider
    {
        private readonly IConfiguration _configuration;
        private string _connectionString;

        public DatabaseProvider(
            IConfiguration configuration
            )
        {
            _configuration = configuration;
            _connectionString = _configuration.GetSection(nameof(DatabaseSettings))[nameof(DatabaseSettings.ConnectionString)];
        }

        public IDbConnection GetConnection()
        {
            var conn = new NpgsqlConnection(_connectionString);
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            return conn;
        }
    }
}
