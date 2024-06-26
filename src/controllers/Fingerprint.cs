using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;


namespace Controllers
{
    public class Fingerprint
    {
        // GET ALL FINGERPRINTS (HANDLE ENCRYPTION)
        public static List<Models.Fingerprint> GetFingerprints()
        {
            // Initialize aes decryptor
            Lib.AES aes = new();

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
                    // Get encrypted data
                    string encryptedNama = reader.GetString("nama");
                    string encryptedBerkasCitra = reader.GetString("berkas_citra");

                    // Get decrypted data
                    string decryptedNama = aes.Decrypt(encryptedNama);
                    string decryptedBerkasCitra = aes.Decrypt(encryptedBerkasCitra);

                    Models.Fingerprint fingerprint = new(
                        decryptedNama,
                        decryptedBerkasCitra
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

        // GET ALL FINGERPRINTS (NOT HANDLE ENCRYPTION, RAW)
        public static List<Models.Fingerprint> GetRawFingerprints()
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
                    // Get encrypted data
                    string nama = reader.GetString("nama");
                    string berkasCitra = reader.GetString("berkas_citra");

                    Models.Fingerprint fingerprint = new(
                        nama,
                        berkasCitra
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


        // Get a fingerprint by name (could be more than one & HANDLE ENCRYPTION)
        public static List<Models.Fingerprint> GetFingerprint(string nama)
        {
            // Initialize 
            // Data in the database is encrypted, so must select * from db then filter it here
            List<Models.Fingerprint> fingerprints = GetFingerprints().FindAll(fingerprint => fingerprint.GetNama() == nama);

            // Return
            return fingerprints;
        }
    }
}