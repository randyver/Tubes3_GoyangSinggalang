using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace Controllers
{
    public class User
    {
        public static List<Models.User> GetUsers()
        {
            // Get connection
            using MySqlConnection connection = Db.Db.GetConnection();

            // Query
            string query = "SELECT * FROM biodata";

            // Execute query
            List<Models.User> users = [];

            try
            {
                // Execute query
                using MySqlCommand command = new(query, connection);
                using MySqlDataReader reader = command.ExecuteReader();

                // Parse
                while (reader.Read())
                {
                    Models.User user = new(
                        reader.GetString("NIK"),
                        reader.GetString("nama"),
                        reader.GetString("tempat_lahir"),
                        reader.GetDateTime("tanggal_lahir"),
                        reader.GetString("jenis_kelamin"),
                        reader.GetString("golongan_darah"),
                        reader.GetString("alamat"),
                        reader.GetString("agama"),
                        reader.GetString("status_perkawinan"),
                        reader.GetString("pekerjaan"),
                        reader.GetString("kewarganegaraan")
                    );

                    users.Add(user);
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Db.Db.CloseConnection();
            }

            // Return
            return users;
        }

        // Get user by name using regex
        public static List<Models.User> GetUser(string nama)
        {
            // Get connection
            using MySqlConnection connection = Db.Db.GetConnection();

            // Query
            string query = "SELECT * FROM biodata WHERE nama REGEXP @nama";
            List<Models.User> users = [];

            // Execute query
            try
            {
                // Execute query
                using MySqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@nama", nama);
                using MySqlDataReader reader = command.ExecuteReader();

                // Parse
                while (reader.Read())
                {
                    Models.User user = new(
                        reader.GetString("NIK"),
                        reader.GetString("nama"),
                        reader.GetString("tempat_lahir"),
                        reader.GetDateTime("tanggal_lahir"),
                        reader.GetString("jenis_kelamin"),
                        reader.GetString("golongan_darah"),
                        reader.GetString("alamat"),
                        reader.GetString("agama"),
                        reader.GetString("status_perkawinan"),
                        reader.GetString("pekerjaan"),
                        reader.GetString("kewarganegaraan")
                    );

                    users.Add(user);
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Db.Db.CloseConnection();
            }

            // Return
            return users;
        }
    }
}