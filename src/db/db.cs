using System;
using System.Collections.Generic;
using System.IO;
using MySql.Data.MySqlClient;
using Bogus;
using System.Linq;

namespace Db
{
    public class Db
    {
        private static MySqlConnection? connection;
        private static readonly string connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ?? throw new Exception("DB_CONNECTION_STRING is not set");

        public static MySqlConnection GetConnection()
        {
            if (connection != null && connection.State == System.Data.ConnectionState.Open)
            {
                return connection;
            }

            connection = new MySqlConnection(connectionString);
            connection.Open();

            return connection;
        }

        public static void CloseConnection()
        {
            if (connection != null)
            {
                connection.Close();
                connection = null;
            }
        }

        public static void Migrate()
        {
            // Create database if not exists & migrate schmea from schema.sql
            using MySqlConnection connection = GetConnection();
            // Use transaction to rollback if error
            MySqlTransaction transaction = connection.BeginTransaction();
            MySqlCommand command = connection.CreateCommand();
            command.Transaction = transaction;

            // Read schema.sql
            string schema = System.IO.File.ReadAllText("db/schema.sql");

            // Execute schema.sql
            try
            {
                // MIGRATE SCHEMA
                command.CommandText = schema;
                command.ExecuteNonQuery();

                // COMMIT
                transaction.Commit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                transaction.Rollback();
            }
            finally
            {
                CloseConnection();
            }
        }

        public static void LoadDump()
        {
            // Create database if not exists & migrate schmea from schema.sql
            using MySqlConnection connection = GetConnection();
            // Use transaction to rollback if error
            MySqlTransaction transaction = connection.BeginTransaction();
            MySqlCommand command = connection.CreateCommand();
            command.Transaction = transaction;

            // Read schema.sql
            string schema = System.IO.File.ReadAllText("db/seeded.sql");

            // Execute schema.sql
            try
            {
                command.CommandText = schema;
                command.ExecuteNonQuery();
                transaction.Commit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                transaction.Rollback();
            }
            finally
            {
                CloseConnection();
            }
        }

        public static void Seed()
        {
            // Generate
            string testImagesPath = "../test/real/";

            // Intialize AES Encryption
            Lib.AES aes = new();

            // Get all files in test/ folder
            // Contains 6000 images where each person has 10 fingerprint images (from each fingers)
            // Save relative path from src (which is ../test/real/<FILENAME>.BMP)
            // Need to sort based on file name to make sure the order is correct
            List<string> filesDirectories = new(Directory.GetFiles(testImagesPath).OrderBy(path => Convert.ToInt32(path.Split("../test/real/")[1].Split("__")[0])));
            // Generate 600 names
            List<string> names = [];
            for (int i = 0; i < 600; i++)
            {
                names.Add(new Faker().Name.FullName());
            }

            // Enums
            List<string> golonganDarahEnum = ["A", "B", "AB", "O"];
            List<string> agamaEnum = ["Islam", "Kristen", "Katolik", "Hindu", "Buddha", "Konghucu"];
            List<string> statusPerkawinanEnum = ["Belum Menikah", "Menikah", "Cerai"];

            // Delete all data in database
            using MySqlConnection connection = GetConnection();
            MySqlCommand command = connection.CreateCommand();
            MySqlTransaction transaction = connection.BeginTransaction();
            command.Transaction = transaction;

            try
            {
                // Delete previous biodata
                command.CommandText = "DELETE FROM biodata";
                command.ExecuteNonQuery();

                // Delete previous sidik jari
                command.CommandText = "DELETE FROM sidik_jari";
                command.ExecuteNonQuery();

                // Insert new biodata
                // Generate 600 biodata
                for (int i = 0; i < names.Count; i++)
                {
                    // Original data
                    // 50 50 chance data to be corrupted in biodata (alay name)
                    bool shouldAlay = new Random().Next(0, 2) == 0;
                    string nama = shouldAlay ? Lib.Utils.GetBahasaAlay(names[i]) : names[i];
                    string nik = (i + 1).ToString();
                    string tempatLahir = new Faker().Address.City();
                    string tanggalLahir = new Faker().Person.DateOfBirth.ToString("yyyy-MM-dd");
                    string jenisKelamin = new Random().Next(0, 2) == 0 ? "Laki-Laki" : "Perempuan";
                    string golonganDarah = golonganDarahEnum[new Random().Next(0, 4)];
                    string alamat = new Faker().Address.FullAddress();
                    string agama = agamaEnum[new Random().Next(0, 6)];
                    string statusPerkawinan = statusPerkawinanEnum[new Random().Next(0, 3)];
                    string pekerjaan = new Faker().Person.Company.Name;
                    string kewarganegaraan = new Faker().Address.CountryCode();

                    // Encrypted data
                    string namaEncrypted = aes.Encrypt(nama).Replace("'", @"\'");
                    string nikEncrypted = aes.Encrypt(nik).Replace("'", @"\'");
                    string tempatLahirEncrypted = aes.Encrypt(tempatLahir).Replace("'", @"\'");
                    string tanggalLahirEncrypted = aes.Encrypt(tanggalLahir).Replace("'", @"\'");
                    string jenisKelaminEncrypted = aes.Encrypt(jenisKelamin).Replace("'", @"\'");
                    string golonganDarahEncrypted = aes.Encrypt(golonganDarah).Replace("'", @"\'");
                    string alamatEncrypted = aes.Encrypt(alamat).Replace("'", @"\'");
                    string agamaEncrypted = aes.Encrypt(agama).Replace("'", @"\'");
                    string statusPerkawinanEncrypted = aes.Encrypt(statusPerkawinan).Replace("'", @"\'");
                    string pekerjaanEncrypted = aes.Encrypt(pekerjaan).Replace("'", @"\'");
                    string kewarganegaraanEncrypted = aes.Encrypt(kewarganegaraan).Replace("'", @"\'");

                    // Insert data
                    command.CommandText = $"INSERT INTO biodata (nik, nama, tempat_lahir, tanggal_lahir, jenis_kelamin, golongan_darah, alamat, agama, status_perkawinan, pekerjaan, kewarganegaraan) VALUES ('{nikEncrypted}', '{namaEncrypted}', '{tempatLahirEncrypted}', '{tanggalLahirEncrypted}', '{jenisKelaminEncrypted}', '{golonganDarahEncrypted}', '{alamatEncrypted}', '{agamaEncrypted}', '{statusPerkawinanEncrypted}', '{pekerjaanEncrypted}', '{kewarganegaraanEncrypted}')";
                    command.ExecuteNonQuery();
                }

                // Generate 6000 fingerprints
                for (int i = 0; i < filesDirectories.Count; i++)
                {
                    // Original data
                    string nama = names[i / 10];
                    string path = filesDirectories[i];

                    // Encrypt data
                    string encryptedNama = aes.Encrypt(nama).Replace("'", @"\'");
                    string encryptedPath = aes.Encrypt(path).Replace("'", @"\'");

                    // Insert
                    command.CommandText = $"INSERT INTO sidik_jari (nama, berkas_citra) VALUES ('{encryptedNama}', '{encryptedPath}')";
                    command.ExecuteNonQuery();
                }

                // Commit transaction
                transaction.Commit();
            }
            catch (Exception e)
            {
                // Rollback transaction if error
                Console.WriteLine(e.Message);
                transaction.Rollback();
            }
            finally
            {
                CloseConnection();
            }
        }
    }

}