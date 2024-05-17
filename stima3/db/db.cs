using System;
using MySql.Data.MySqlClient;
using Bogus;

class Db
{
    private static MySqlConnection? connection;
    private static readonly string connectionString = "server=localhost;user=tubes3_stima;password=12345;database=tubes3_stima";

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

    public static void Seed()
    {
        // Generate
        string testImagesPath = "../test/";

        // Get all files in test/ folder
        // Contains 6000 images where each person has 10 fingerprint images (from each fingers)
        string[] filesDirectories = Directory.GetFiles(testImagesPath);
        string[] fileNames = [.. filesDirectories.Select(file => file.Split("/test/").Last()).OrderBy(filename => filename.Split("__")[0])];

        // Generate 600 names
        List<string> names = [];
        for (int i = 0; i < 600; i++)
        {
            names.Add(new Faker().Name.FullName());
        }

        // Generate 6000 fingerprints
        List<Fingerprint> fingerprints = [];
        for (int i = 0; i < fileNames.Length; i++)
        {
            string nama = names[i / 10].Replace("'", @"\'");
            string path = fileNames[i].Replace("'", @"\'");
            fingerprints.Add(new Fingerprint(nama, path));
        }

        // Generate user data
        List<User> users = [];
        List<string> golonganDarahEnum = new List<string> { "A", "B", "AB", "O" };
        List<string> agamaEnum = new List<string> { "Islam", "Kristen", "Katolik", "Hindu", "Buddha", "Konghucu" };
        List<string> statusPerkawinanEnum = new List<string> { "Belum Menikah", "Menikah", "Cerai" };

        for (int i = 0; i < names.Count; i++)
        {
            // 50 50 chance data to be corrupted in biodata (alay name)
            bool shouldAlay = new Random().Next(0, 2) == 0;
            string nama = shouldAlay ? Utils.GetBahasaAlay(names[i]).Replace("'", @"\'") : names[i].Replace("'", @"\'");

            string nik = (i + 1).ToString();
            string tempatLahir = new Faker().Address.City().Replace("'", @"\'");
            string tanggalLahir = new Faker().Person.DateOfBirth.ToString("yyyy-MM-dd");
            string jenisKelamin = new Random().Next(0, 2) == 0 ? "Laki-Laki" : "Perempuan";
            string golonganDarah = golonganDarahEnum[new Random().Next(0, 4)];
            string alamat = new Faker().Address.FullAddress().Replace("'", @"\'");
            string agama = agamaEnum[new Random().Next(0, 6)];
            string statusPerkawinan = statusPerkawinanEnum[new Random().Next(0, 3)];
            string pekerjaan = new Faker().Person.Company.Name.Replace("'", @"\'");
            string kewarganegaraan = new Faker().Address.CountryCode().Replace("'", @"\'");

            users.Add(new User(nik, nama, tempatLahir, tanggalLahir, jenisKelamin, golonganDarah, alamat, agama, statusPerkawinan, pekerjaan, kewarganegaraan));
        }

        // Delete all data in database
        using (MySqlConnection connection = GetConnection())
        {
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
                foreach (User user in users)
                {
                    Console.WriteLine("---------------");
                    Console.WriteLine(user.GetNik());
                    Console.WriteLine(user.GetNama());
                    Console.WriteLine(user.GetTempatLahir());
                    Console.WriteLine(user.GetTanggalLahir());
                    Console.WriteLine(user.GetJenisKelamin());
                    Console.WriteLine(user.GetGolonganDarah());
                    Console.WriteLine(user.GetAlamat());
                    Console.WriteLine(user.GetAgama());
                    Console.WriteLine(user.GetStatusPerkawinan());
                    Console.WriteLine(user.GetPekerjaan());
                    Console.WriteLine(user.GetKewarganegaraan());
                    Console.WriteLine("---------------");

                    command.CommandText = $"INSERT INTO biodata (nik, nama, tempat_lahir, tanggal_lahir, jenis_kelamin, golongan_darah, alamat, agama, status_perkawinan, pekerjaan, kewarganegaraan) VALUES ('{user.GetNik()}', '{user.GetNama()}', '{user.GetTempatLahir()}', '{user.GetTanggalLahir()}', '{user.GetJenisKelamin()}', '{user.GetGolonganDarah()}', '{user.GetAlamat()}', '{user.GetAgama()}', '{user.GetStatusPerkawinan()}', '{user.GetPekerjaan()}', '{user.GetKewarganegaraan()}')";
                    command.ExecuteNonQuery();
                }

                // Insert new sidik jari
                foreach (Fingerprint fingerprint in fingerprints)
                {
                    command.CommandText = $"INSERT INTO sidik_jari (nama, berkas_citra) VALUES ('{fingerprint.GetNama()}', '{fingerprint.GetPath()}')";
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