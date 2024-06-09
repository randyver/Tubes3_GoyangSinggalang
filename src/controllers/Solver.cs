using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using Bogus.Extensions.Extras;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Controllers
{
    // Uses KMP or BM Algorithm to find perfect match
    // Uses Hamming Distance to find similarity

    public class Solver(string inputImagePath, bool isKmp)
    {
        // Input data
        // finger print
        private readonly string inputImagePath = inputImagePath;

        // is kmp
        private readonly bool isKmp = isKmp;

        // Output data
        // User data
        private Models.User? userData;
        // Finger print data
        private Models.Fingerprint? fingerPrintData;
        // similarity
        private double? similarity;
        // duration
        private double? duration;

        // Use LCS to find similarity
        private static double LcsSolver(string s1, string s2)
        {
            // Get length of both strings
            int n = s1.Length;
            int m = s2.Length;

            // Create dp array
            double[,] dp = new double[n + 1, m + 1];

            // Base case
            for (int i = 0; i <= n; i++)
            {
                dp[i, 0] = 0;
            }
            for (int i = 0; i <= m; i++)
            {
                dp[0, i] = 0;
            }

            // Fill dp array
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    // If the characters are the same
                    if (s1[i - 1] == s2[j - 1])
                    {
                        // Add 1 to the previous diagonal value
                        dp[i, j] = dp[i - 1, j - 1] + 1;
                    }
                    // If the characters are different
                    else
                    {
                        // Get the maximum value from the top and left
                        dp[i, j] = Math.Max(dp[i - 1, j], dp[i, j - 1]);
                    }
                }
            }

            // Return similarity
            return dp[n, m] / Math.Max(n, m);
        }

        private static double HammingDistanceSolver(char[,] rectangle1, char[,] rectangle2) {
            // rectangle2 is the larger image here

            // print rectangle 1
            for (int i = 0; i < rectangle1.GetLength(0); i++) {
                for (int j = 0; j < rectangle1.GetLength(1); j++) {
                }
            }

            // print rectangle 2
            for (int i = 0; i < rectangle2.GetLength(0); i++) {
                for (int j = 0; j < rectangle2.GetLength(1); j++) {
                }
            }


            // Calculate the hamming distance
            int distance = (int) 2e9;
            int maxtop = rectangle2.GetLength(0) - rectangle1.GetLength(0);
            int maxleft = rectangle2.GetLength(1) - rectangle1.GetLength(1);
            // ("Maxleft: " + maxleft);
            for (int top = 0; top < maxtop + 1; top++) {
                for (int left = 0; left < maxleft + 1; left++) {
                    int tempDistance = 0;
                    for (int i = 0; i < rectangle1.GetLength(0); i++) {
                        for (int j = 0; j < rectangle1.GetLength(1); j++) {
                            if (rectangle1[i, j] != rectangle2[top + i, left + j]) {
                                tempDistance++;
                            }
                        }
                    }
                    distance = Math.Min(distance, tempDistance);
                }
            }

            // Return the similarity
            return 1 - (double)distance / (rectangle1.GetLength(0) * rectangle1.GetLength(1));
        }
        public static double LevenshteinDistance(string s1, string s2)
        {
            // Get length of both strings
            int n = s1.Length;
            int m = s2.Length;

            // Create dp array
            double[,] dp = new double[n + 1, m + 1];

            // Base case
            for (int i = 0; i <= n; i++)
            {
                dp[i, 0] = i;
            }
            for (int i = 0; i <= m; i++)
            {
                dp[0, i] = i;
            }

            // Fill dp array
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    // If the characters are the same
                    if (s1[i - 1] == s2[j - 1])
                    {
                        // Add 0 to the previous diagonal value
                        dp[i, j] = dp[i - 1, j - 1];
                    }
                    // If the characters are different
                    else
                    {
                        // Get the minimum value from the top, left, and diagonal
                        dp[i, j] = Math.Min(dp[i - 1, j - 1], Math.Min(dp[i - 1, j], dp[i, j - 1])) + 1;
                    }
                }
            }

            // Return similarity
            return 1 - dp[n, m] / Math.Max(n, m);
        }

        // KMP Solver
        public static bool KmpSolver(string text, string pattern)
        {
            // Get length of both strings
            int n = text.Length;
            int m = pattern.Length;

            // Create LPS array
            int[] lps = new int[m];
            lps[0] = 0;
            int i = 1;
            int j = 0;

            while (i < m)
            {
                if (pattern[i] == pattern[j])
                {
                    lps[i] = j + 1;
                    i++;
                    j++;
                }
                else if (j > 0)
                {
                    j = lps[j - 1];
                }
                else
                {
                    lps[i] = 0;
                    i++;
                }
            }

            i = 0;
            j = 0;
            // Iterate through the text
            while (i < n)
            {
                // If the characters are the same
                if (pattern[j] == text[i])
                {
                    // End of pattern (found pattern)
                    if (j == m - 1)
                    {
                        return true;
                    }
                    // Move to the right
                    else
                    {
                        i++;
                        j++;
                    }
                }
                // If the characters are different
                else if (j > 0)
                {
                    j = lps[j - 1];
                }
                else
                {
                    i++;
                }
            }
        

            // Not found pattern
            return false;
        }

        // BM Solver
        public static bool BmSolver(string text, string pattern)
        {
            // Get length of both strings
            int n = text.Length;
            int m = pattern.Length;

            // Create bad character array
            int[] last = new int[256];

            // Fill bad character array
            int i;
            for (i = 0; i < 256; i++)
            {
                last[i] = -1;
            }

            for (i = 0; i < m; i++)
            {
                last[pattern[i]] = i;
            }

            // Start from the right
            i = m - 1;
            // Not found if pattern is longer than text
            if (i > n - 1)
            {
                return false;
            }

            int j = m - 1;
            do
            {
                // If the characters are the same
                if (pattern[j] == text[i])
                {
                    // End of pattern (found pattern)
                    if (j == 0)
                    {
                        return true;
                    }
                    // Move to the left
                    else
                    {
                        i--;
                        j--;
                    }
                }
                // If the characters are different
                else
                {
                    int k = last[text[i]];
                    i = i + m - Math.Min(j, 1 + k);
                    j = m - 1;
                }
            } while (i <= n - 1);
        

            return false;
        }

        // Solver
        public void Solve()
        {

            // Start calculation duration
            var watch = System.Diagnostics.Stopwatch.StartNew();

            // Get all fingerprints
            List<Models.Fingerprint> allFingerprints = Controllers.Fingerprint.GetFingerprints();

            // Get input image
            Image<Rgba32> inputImage = Image.Load<Rgba32>(inputImagePath);

            // // print the grayscale image
            // for (int y = 0; y < inputImage.Height; y++) {
            //     for (int x = 0; x < inputImage.Width; x++) {
            //         // grayscale
            //         int gray = (int)(inputImage[x, y].R * 0.3 + inputImage[x, y].G * 0.59 + inputImage[x, y].B * 0.11);
            //         int binary = gray > 127 ? 0 : 1;
            //         Console.Write(binary);
            //     }
            //     Console.WriteLine();
            // }

            // Console.WriteLine("\n\n");

            inputImage = Lib.ImageConverter.OmitWhiteSpace(inputImage);

            // print the grayscale image
            // for (int y = 0; y < inputImage.Height; y++) {
            //     for (int x = 0; x < inputImage.Width; x++) {
            //         // grayscale
            //         int gray = (int)(inputImage[x, y].R * 0.3 + inputImage[x, y].G * 0.59 + inputImage[x, y].B * 0.11);
            //         int binary = gray > 127 ? 0 : 1;
            //         Console.Write(binary);
            //     }
            //     Console.WriteLine();
            // }


            string inputImageAsciiString = Lib.ImageConverter.ConvertToAscii8Bits(inputImage);
            string[] inputImageCropped = Lib.ImageConverter.GetCenteredArrays(inputImageAsciiString);

            // Iterate through all fingerprints
            bool isMatch = false;
            Models.Fingerprint? bestFingerprint = null;
            double bestSimilarity = 0;

            // Solve for KMP
            if (isKmp)
            {
                foreach (Models.Fingerprint fingerprint in allFingerprints)
                {
                    foreach (string pattern in inputImageCropped)
                    {
                        // Get fingerprint image
                        Image<Rgba32> fingerPrintImage = Image.Load<Rgba32>(fingerprint.GetPath());
                        fingerPrintImage = Lib.ImageConverter.OmitWhiteSpace(fingerPrintImage);

                        // Convert to ascii 8 bit
                        string fingerPrintImageAsciiString = Lib.ImageConverter.ConvertToAscii8Bits(fingerPrintImage);

                        // Input string as pattern
                        // Finger print string as text
                        isMatch = KmpSolver(fingerPrintImageAsciiString, pattern);

                        if (isMatch)
                        {
                            // Save fingerprint (contains name & path)
                            bestFingerprint = fingerprint;
                            bestSimilarity = LevenshteinDistance(fingerPrintImageAsciiString, inputImageAsciiString);
                            break;
                        }
                    }

                    if (isMatch) {
                        break;
                    }
                }
            }
            // Solve for BM
            else
            {
                foreach (Models.Fingerprint fingerprint in allFingerprints)
                {

                    // Get fingerprint image
                    Image<Rgba32> fingerPrintImage = Image.Load<Rgba32>(fingerprint.GetPath());
                    fingerPrintImage = Lib.ImageConverter.OmitWhiteSpace(fingerPrintImage);

                    // Convert to ascii 8 bit
                    string fingerPrintImageAsciiString = Lib.ImageConverter.ConvertToAscii8Bits(fingerPrintImage);
                    int count = 0;
                    foreach (string pattern in inputImageCropped)
                    {
                        count++;
                        isMatch = BmSolver(fingerPrintImageAsciiString, pattern);
                        

                        if (isMatch)
                        {

                            // print pattern
                            Console.WriteLine("Pattern: " + pattern);
                            bestFingerprint = fingerprint;
                            bestSimilarity = LevenshteinDistance(fingerPrintImageAsciiString, inputImageAsciiString);
                            break;
                        }
                    }

                    if (isMatch) {
                        break;
                    }
                }

            }


            // If not match, use Levenstein Distance
            double SIMILARITY_LIMIT = 0.3;
            if (!isMatch)
            {
                Console.WriteLine("No data found so we use Levenshtein Distance");
                foreach (Models.Fingerprint fingerprint in allFingerprints)
                {
                    // Get fingerprint image
                    Image<Rgba32> fingerPrintImage = Image.Load<Rgba32>(fingerprint.GetPath());
                    fingerPrintImage = Lib.ImageConverter.OmitWhiteSpace(fingerPrintImage);

                    // Convert to ascii 8 bit
                    string fingerPrintImageAsciiString = Lib.ImageConverter.ConvertToAscii8Bits(fingerPrintImage);
                    string[] fingerprintCropped = Lib.ImageConverter.GetCenteredArrays(fingerPrintImageAsciiString);

                    int length = fingerprintCropped.Length;

                    double current_similiarity = 0;
                    for (int i = 0; i < length; i++) {
                        current_similiarity += LevenshteinDistance(fingerprintCropped[i], inputImageCropped[i]);
                    }

                    current_similiarity /= length;
                    if (current_similiarity > bestSimilarity && current_similiarity > SIMILARITY_LIMIT) {
                        bestSimilarity = current_similiarity;
                        bestFingerprint = fingerprint;
                    }
                }
            }



            // No solution found (KMP/BM or LCM)
            if (bestFingerprint == null)
            {
                userData = null;
                fingerPrintData = null;
                similarity = 0;
                duration = 0;
                return;
            }

            // Solution found
            // Time
            watch.Stop();
            duration = watch.ElapsedMilliseconds;

            // similarity
            similarity = bestSimilarity;
            Console.WriteLine(similarity);


            // Fingerprint data
            fingerPrintData = bestFingerprint;

            // Get all user data
            List<Models.User> allUsers = User.GetUsers();

            // Get user data
            foreach (Models.User user in allUsers)
            {
                // Get regex
                string regex = Lib.Utils.GetRegex(user.GetNama());

                // Check if name is in fingerprint name
                if (
                    user.GetNama() == bestFingerprint.GetNama() || // Handle exact match
                    System.Text.RegularExpressions.Regex.IsMatch(bestFingerprint.GetNama(), regex) // Handle bahasa alay regex
                )
                {
                    userData = user;
                    break;
                }
            }
        }

        // Getters
        public double? GetDuration()
        {
            return duration;
        }

        public double? GetSimilarity()
        {
            return similarity;
        }

        public Models.User? GetUserData()
        {
            return userData;
        }

        public Models.Fingerprint? GetFingerPrintData()
        {
            return fingerPrintData;
        }
    }
}