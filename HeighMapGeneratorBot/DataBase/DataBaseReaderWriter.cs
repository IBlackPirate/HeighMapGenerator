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
        /// Попытка добавления карты пользователю в базу данных
        /// </summary>
        /// <param name="map"></param>
        /// <param name="personId"></param>
        public static bool TryAddMap(Map map, long personId)
        {
            if (!IsUserExists(personId))
            {
                AddUser(personId);
            }

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

                    if (ExequteCommand(command) == null)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Попытка получения карты из базы данных
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public static bool TryGetMap(long personId, out Map map)
        {

            if (!IsUserExists(personId))
            {
                map = null;
                return false;
            }

            byte[] heightMap = null;
            byte[] colorMap = null;
            int sizeX = 0;
            int sizeY = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var queryString = $"SELECT heightMap, colorMap, sizeX, sizeY " +
                    $"FROM Map " +
                    $"WHERE idUser={personId}";
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                var reader = ExequteCommand(command);

                if (reader == null)
                {
                    map = null;
                    return false;
                }

                if (reader.Read())
                {
                    heightMap = (byte[])reader["heightMap"];
                    colorMap = (byte[])reader["colorMap"];
                    sizeX = (int)reader["sizeX"];
                    sizeY = (int)reader["sizeY"];
                }
            }
            map = new Map(heightMap.ToMatrix(sizeX, sizeY), colorMap.ToPixels(sizeX, sizeY).ToMatrix(sizeX, sizeY), sizeX, sizeY);
            return true;
        }

        private static SqlDataReader ExequteCommand(SqlCommand command)
        {
            try
            {
                return command.ExecuteReader();
            }
            catch
            {
                return null;
            }
        }

        private static bool IsUserExists(long personId)
        {
            var users = new List<string>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var queryString = $"SELECT idUser FROM Map";
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                var reader = ExequteCommand(command);

                while (reader.Read())
                {
                    users.Add(reader.GetValue(0).ToString());
                }
            }
            return users.Contains(personId.ToString());
        }

        private static void AddUser(long personId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var queryString = $"INSERT INTO Map(idUser) VALUES (@idUser)";
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                var param = new SqlParameter("@idUser", personId);
                command.Parameters.Add(param);
                ExequteCommand(command);
            }
        }
    }
}

