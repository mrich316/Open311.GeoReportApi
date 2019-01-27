using System;
using System.Data.Common;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace Open311.GeoReportApi.OracleEam
{
    public static class EamOptionsExtensions
    {
        internal static async Task<DbConnection> CreateConnection(this EamOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            if (string.IsNullOrWhiteSpace(options.ConnectionString))
            {
                throw new ArgumentException($"{nameof(options.ConnectionString)} must be defined.");
            }

            var connection = new OracleConnection(options.ConnectionString);

            await connection.OpenAsync();

            return connection;
        }
    }
}
