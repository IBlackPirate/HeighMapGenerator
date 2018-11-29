using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

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
                var queryString = $"UPDATE Map" +
                    $"SET heightMap={(object)map.HeightMap.ToArray()}, colorMap = {(object)map.ColorMap.ToArray()}" +
                    $"sizeX={map.SizeX}, sizeY={map.SizeY}" +
                    $"WHERE idUser={personId}";
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                var reader = command.ExecuteNonQuery();
            }
        }

        public static Map GetMap(long personId)
        {
            byte[] heightMap = null;
            byte[] colorMap = null;
            int sizeX = 0;
            int sizeY = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var queryString = $"SELECT heightMap, colorMap, sizeX, sizeY" +
                    $"FROM Map " +
                    $"WHERE idUser={personId}";
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    heightMap = (byte[])reader.GetValue(1);
                    colorMap = (byte[])reader.GetValue(2);
                    sizeX = (int)reader.GetValue(3);
                    sizeY = (int)reader.GetValue(4);
                }
            }
            return new Map(heightMap.ToMatrix(sizeX, sizeY), colorMap.ToMatrix(sizeX, sizeY), sizeX, sizeY);
        }
    }
}

