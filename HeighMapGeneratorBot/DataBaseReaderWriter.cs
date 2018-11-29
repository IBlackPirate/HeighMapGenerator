using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeighMapGeneratorBot
{
    public static class DataBaseReaderWriter
    {
        private static string connectionString = "Data Source=(local);Initial Catalog=Maps;"
                + "Integrated Security=true";


        public static void AddMap(Map map, long personId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var queryString = $"UPDATE heightMap, colorMap VALUES({map.heightMap})";
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                var reader = command.ExecuteNonQuery();
            }
        }

        public static Map GetMap(long personId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var queryString = "SELECT ";
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {

                }
            }
            return new Map();
        }

        
    }
}

