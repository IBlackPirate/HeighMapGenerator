using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Data;

namespace HeighMapGeneratorBot
{
    static class DataBaseReaderWriter
    {
        // Строка подключения
        private static readonly string connectionString = "Data Source=(local);Initial Catalog=Maps;"
                + "Integrated Security=true";

        /// <summary>
        /// Добавление карты пользователю
        /// </summary>
        /// <param name="map"></param>
        /// <param name="personId"></param>
        public static void AddMap(Map map, long personId)
        {
            TryAddUser(personId);

            var queryString = $"UPDATE Map " +
                    $"SET heightMap=@heightMap, colorMap = @colorMap, " +
                    $"sizeX=@sizeX, sizeY=@sizeY " +
                    $"WHERE idUser=@idUser";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    connection.Open();
                    SqlParameter heightMap = command.Parameters.Add("@heightMap", SqlDbType.VarBinary);
                    SqlParameter colorMap = command.Parameters.Add("@colorMap", SqlDbType.VarBinary);
                    SqlParameter sizeX = command.Parameters.Add("@sizeX", SqlDbType.Int);
                    SqlParameter sizeY = command.Parameters.Add("@sizeY", SqlDbType.Int);
                    SqlParameter idUser = command.Parameters.Add("@idUser", SqlDbType.Int);

                    heightMap.Value = map.HeightMap.ToArray();
                    colorMap.Value = map.ColorMap.ToArray().ToByte();
                    sizeX.Value = map.SizeX;
                    sizeY.Value = map.SizeY;
                    idUser.Value = personId;

                    command.ExecuteNonQuery();
                }
            }
        }

        private static void TryAddUser(long personId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    var queryString = $"INSERT INTO Map(idUser) VALUES (@idUser)";
                    SqlCommand command = new SqlCommand(queryString, connection);
                    connection.Open();
                    var param = new SqlParameter("@idUser", personId);
                    command.Parameters.Add(param);
                    var reader = command.ExecuteNonQuery();
                }
            }
            catch { }
        }

        public static bool TryGetMap(long personId, out Map map)
        {
            byte[] heightMap = null;
            byte[] colorMap = null;
            int sizeX = 0;
            int sizeY = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    var queryString = $"SELECT heightMap, colorMap, sizeX, sizeY " +
                        $"FROM Map " +
                        $"WHERE idUser={personId}";
                    SqlCommand command = new SqlCommand(queryString, connection);
                    connection.Open();
                    var reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        heightMap = (byte[])reader["heightMap"];
                        colorMap = (byte[])reader["colorMap"];
                        sizeX = (int)reader["sizeX"];
                        sizeY = (int)reader["sizeY"];
                    }
                }
                map = new Map(heightMap.ToMatrix(sizeX, sizeY), colorMap.ToPixels(sizeX, sizeY).ToMatrix(sizeX, sizeY), sizeX, sizeY);
            }
            catch
            {
                map = null;
                return false;
            }
            return true;
        }
    }
}

