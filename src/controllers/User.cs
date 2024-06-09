using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;
using System.Globalization;

namespace Controllers
{
    public class User
    {
        // HANDLE ENCRYPTED DATA
        public static List<Models.User> GetUsers()
        {
            // Aes decryptor
            Lib.AES aes = new();

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
                    // Get encrypted data
                    string encryptedNik = reader.GetString("NIK");
                    string encryptedNama = reader.GetString("nama");
                    string encryptedTempatLahir = reader.GetString("tempat_lahir");
                    string encryptedTanggalLahir = reader.GetString("tanggal_lahir");
                    string encryptedJenisKelamin = reader.GetString("jenis_kelamin");
                    string encryptedGolonganDarah = reader.GetString("golongan_darah");
                    string encryptedAlamat = reader.GetString("alamat");
                    string encryptedAgama = reader.GetString("agama");
                    string encryptedStatusPerkawinan = reader.GetString("status_perkawinan");
                    string encryptedPekerjaan = reader.GetString("pekerjaan");
                    string encryptedKewarganegaraan = reader.GetString("kewarganegaraan");

                    // Get decrypted data
                    string decryptedNik = aes.Decrypt(encryptedNik);
                    string decryptedNama = aes.Decrypt(encryptedNama);
                    string decryptedTempatLahir = aes.Decrypt(encryptedTempatLahir);
                    // Console.WriteLine(aes.Decrypt(encryptedTanggalLahir));
                    DateTime decryptedTanggalLahir = DateTime.ParseExact(aes.Decrypt(encryptedTanggalLahir), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None);
                    // Console.WriteLine(decryptedTanggalLahir.ToString());
                    string decryptedJenisKelamin = aes.Decrypt(encryptedJenisKelamin);
                    string decryptedGolonganDarah = aes.Decrypt(encryptedGolonganDarah);
                    string decryptedAlamat = aes.Decrypt(encryptedAlamat);
                    string decryptedAgama = aes.Decrypt(encryptedAgama);
                    string decryptedStatusPerkawinan = aes.Decrypt(encryptedStatusPerkawinan);
                    string decryptedPekerjaan = aes.Decrypt(encryptedPekerjaan);
                    string decryptedKewarganegaraan = aes.Decrypt(encryptedKewarganegaraan);

                    // Add user
                    Models.User user = new(
                        decryptedNik,
                        decryptedNama,
                        decryptedTempatLahir,
                        decryptedTanggalLahir,
                        decryptedJenisKelamin,
                        decryptedGolonganDarah,
                        decryptedAlamat,
                        decryptedAgama,
                        decryptedStatusPerkawinan,
                        decryptedPekerjaan,
                        decryptedKewarganegaraan
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

        // GET ALL USERS WITHOUT HANDLING ENCRYPTION DATA, RAW.
        public static List<Models.User> GetRawUsers()
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
                    // Get encrypted data
                    string nik = reader.GetString("NIK");
                    string nama = reader.GetString("nama");
                    string tempatLahir = reader.GetString("tempat_lahir");
                    DateTime tanggalLahir = reader.GetDateTime("tanggal_lahir").Date;
                    string jenisKelamin = reader.GetString("jenis_kelamin");
                    string golonganDarah = reader.GetString("golongan_darah");
                    string alamat = reader.GetString("alamat");
                    string agama = reader.GetString("agama");
                    string statusPerkawinan = reader.GetString("status_perkawinan");
                    string pekerjaan = reader.GetString("pekerjaan");
                    string kewarganegaraan = reader.GetString("kewarganegaraan");

                    // Add user
                    Models.User user = new(
                        nik,
                        nama,
                        tempatLahir,
                        tanggalLahir,
                        jenisKelamin,
                        golonganDarah,
                        alamat,
                        agama,
                        statusPerkawinan,
                        pekerjaan,
                        kewarganegaraan
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


        // HANDLE ENCRYPTED DATA
        // Get user from nama (not handled if nama is alay but already handle encrypted data in the database)
        public static Models.User? GetUser(string nama)
        {
            // Data in the database is encrypted, so must select * from db then filter it here.
            List<Models.User> users = GetUsers();

            Models.User? user = GetUsers().Find(user => user.GetNama() == nama);

            // Return
            return user;
        }
    }
}