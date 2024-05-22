using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;


namespace Controllers
{
    public class Fingerprint
    {
        // Get all fingerprints in a database
        public static List<Models.Fingerprint> GetFingerprints()
        {
            // Get connection
            using MySqlConnection connection = Db.Db.GetConnection();

            // Query
            string query = "SELECT * FROM sidik_jari";
            List<Models.Fingerprint> fingerprints = [];

            // Execute query
            try
            {
                // Execute query
                using MySqlCommand command = new(query, connection);
                using MySqlDataReader reader = command.ExecuteReader();

                // Parse
                while (reader.Read())
                {
                    Models.Fingerprint fingerprint = new(
                        reader.GetString("nama"),
                        reader.GetString("berkas_citra")
                    );

                    fingerprints.Add(fingerprint);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Db.Db.CloseConnection();
            }

            // Return
            return fingerprints;
        }

        // Get a fingerprint by name (could be more than one)
        public static List<Models.Fingerprint> GetFingerprint(string nama)
        {
            // Get connection
            using MySqlConnection connection = Db.Db.GetConnection();

            // Query
            // Note: nama di kolom sidik_jari tidak corrupted
            string query = "SELECT * FROM sidik_jari WHERE nama = @nama";
            List<Models.Fingerprint> fingerprints = [];

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
                    Models.Fingerprint fingerprint = new(
                        reader.GetString("nama"),
                        reader.GetString("berkas_citra")
                    );

                    fingerprints.Add(fingerprint);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Db.Db.CloseConnection();
            }

            // Return
            return fingerprints;
        }
    }
}