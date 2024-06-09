using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Bogus;
using Controllers;
using ZstdSharp.Unsafe;


namespace Cli
{
    public class Cli
    {
        public static void RunQuery()
        {
            // Test query
            List<Models.User> users = Controllers.User.GetUsers();
            List<Models.Fingerprint> fingerprints = Controllers.Fingerprint.GetFingerprints();

            // Print result
            for (int i = 0; i < users.Count; i++)
            {
                users[i].Print();
                Console.WriteLine();
            }

            // // Print result
            // for (int i = 0; i < fingerprints.Count; i++)
            // {
            //     fingerprints[i].Print();
            //     Console.WriteLine();
            // }
        }
        public static void RunAes()
        {
            // Test AES
            string plaintext = "Dewantoro Triatmojo LOLLLLLLLLLLLLLLLLLLLLLLLLLL";
            Lib.AES aes = new();

            string encrypted = aes.Encrypt(plaintext);
            Console.WriteLine("Encrypted: " + encrypted);
            Console.WriteLine();

            string decrypted = aes.Decrypt(encrypted);
            Console.WriteLine("Decrypted: " + decrypted);

        }
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
            Controllers.Solver solver = new("../test/altered-easy/1__M_Left_index_finger_CR.BMP", true);

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

        public static void RunNyoba()
        {
            string text = "aaabbb";
            string pattern = "aaa";

            Boolean isMatch = Controllers.Solver.BmSolver(text, pattern);
            if (isMatch)
            {
                Console.WriteLine("Match");
            }
            else
            {
                Console.WriteLine("Not Match");
            }
        }

        public static void RunStress()
        {
            // Test directory path
            string directoryPath = "../test/real/";

            // Get all image in the directory
            string[] files = System.IO.Directory.GetFiles(directoryPath);
            int count = 0, correct = 0, nodatafound = 0;

            // Solve
            foreach (string file in files)
            {
                Console.WriteLine($"File: {file}");
                Controllers.Solver solver = new(file, false);
                solver.Solve();
                Models.User? user = solver.GetUserData();
                Models.Fingerprint? fingerprint = solver.GetFingerPrintData();
                double? duration = solver.GetDuration();
                double? similarity = solver.GetSimilarity();

                if (user != null && fingerprint != null && duration != null && similarity != null)
                {
                    Console.WriteLine("Matched with " + solver.GetFingerPrintData()?.GetPath());
                    string filenik = file.Split("/")[3].Split("__")[0];
                    if (user.GetNik() == filenik)
                    {
                        Console.WriteLine("Matched & CORRECT");
                        correct++;
                        count++;
                    }
                    else
                    {
                        Console.WriteLine("Matched but INCORRECT");
                        count++;

                    }
                }
                else
                {
                    Console.WriteLine("No data found");
                    count++;
                    nodatafound++;
                }

                Console.WriteLine($"Match & CORRECT: {correct}/{count}");
                Console.WriteLine($"Match but INCORRECT: {count - correct - nodatafound}/{count}");
                Console.WriteLine($"No data found: {nodatafound}/{count}\n\n");
            }
        }
    }

}