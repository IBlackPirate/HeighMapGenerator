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
                var queryString = $"UPDATE heightMap, colorMap VALUES({(object)map.heightMap.ToArray()}, {(object)map.heightMap.ToArray()})";
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

