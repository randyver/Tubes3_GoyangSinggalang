using System;
using Bogus;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Cli
{
    public class Cli
    {
        public static void RunRegex()
        {
            // Test regex
            string text = "Dewantoro Triatmojo";
            string bahasaAlay = Lib.Utils.GetBahasaAlay(text);
            string regexAlay = Lib.Utils.GetRegex(bahasaAlay);

            // Print result
            Console.WriteLine($"Text: {text}");
            Console.WriteLine($"Bahasa Alay: {bahasaAlay}");
            Console.WriteLine($"Regex Alay: {regexAlay}");

            // Test regex
            string test1 = "Dewantoro Triatmojo";
            string test2 = Lib.Utils.GetBahasaAlay(test1);

            // Print result
            bool r1 = System.Text.RegularExpressions.Regex.IsMatch(test1, regexAlay);
            bool r2 = System.Text.RegularExpressions.Regex.IsMatch(test2, regexAlay);

            Console.WriteLine(r1);
            Console.WriteLine(r2);
        }
        public static void RunSolve()
        {
            // Test controller
            // Controllers.Solver solver = new("../temp/315650.jpg", false);
            // Controllers.Solver solver = new("../test/4__M_Left_index_finger.BMP", true);
            Controllers.Solver solver = new("../temp/1__M_Left_index_finger_CR.BMP", true);

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
            else
            {
                Console.WriteLine("No data found");
            }
        }
    }

}