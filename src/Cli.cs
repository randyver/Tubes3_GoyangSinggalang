using System;
using Bogus;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Cli
{
    public class Cli
    {
        public static void Run()
        {
            // Test controller
            // Controllers.Solver solver = new("../temp/315650.jpg", false);
            Controllers.Solver solver = new("../test/1__M_Left_index_finger.BMP", false);

            // Solve
            solver.Solve();

            // Get result
            Models.User? user = solver.GetUserData();
            Models.Fingerprint? fingerprint = solver.GetFingerPrintData();
            double? duration = solver.GetDuration();
            double? similarity = solver.GetSimilarity();

            // Print result
            if (user != null && fingerprint != null && duration != null && similarity != null)
            {
                Console.WriteLine("User Data:");
                Console.WriteLine($"NIK: {user.GetNik()}");
                Console.WriteLine($"Nama: {user.GetNama()}");
                Console.WriteLine($"Tempat Lahir: {user.GetTempatLahir()}");
                Console.WriteLine($"Tanggal Lahir: {user.GetTanggalLahir()}");
                Console.WriteLine($"Jenis Kelamin: {user.GetJenisKelamin()}");
                Console.WriteLine($"Golongan Darah: {user.GetGolonganDarah()}");
                Console.WriteLine($"Alamat: {user.GetAlamat()}");
                Console.WriteLine($"Agama: {user.GetAgama()}");
                Console.WriteLine($"Status Perkawinan: {user.GetStatusPerkawinan()}");
                Console.WriteLine($"Pekerjaan: {user.GetPekerjaan()}");
                Console.WriteLine($"Kewarganegaraan: {user.GetKewarganegaraan()}");
                Console.WriteLine();

                Console.WriteLine("Fingerprint Data:");
                Console.WriteLine($"Nama: {fingerprint.GetNama()}");
                Console.WriteLine($"Path: {fingerprint.GetPath()}");
                Console.WriteLine();

                Console.WriteLine($"Duration: {duration} ms");
                Console.WriteLine($"Similarity: {similarity}");
            }
        }
    }

}