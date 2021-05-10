using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    internal static class DBConnection
    {
        private static string connectionString = @"Data Source=localhost\sqlexpress;Initial Catalog=movie_manager_db;Integrated Security=True";

        public static SqlConnection GetDBConnection()
        {
            var conn = new SqlConnection(connectionString);
            return conn;
        }
    }
}
