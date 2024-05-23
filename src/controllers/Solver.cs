using System;
using System.Collections.Generic;
using Bogus;
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

        // KMP Solver
        private static bool KmpSolver(string text, string pattern)
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

            // Do KMP Algorithm
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
        private static bool BmSolver(string text, string pattern)
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
            Image<Rgba32> inputCroppedImage = Lib.ImageConverter.GetCenteredImage(inputImage, 32);

            // Convert to ascii 8 bit
            string inputCroppedImageAsciiString = Lib.ImageConverter.ConvertToAscii8Bits(inputCroppedImage);

            // Iterate through all fingerprints
            bool isMatch = false;
            Models.Fingerprint? bestFingerprint = null;
            double bestSimilarity = 0;
            // Solve for KMP
            if (isKmp)
            {
                foreach (Models.Fingerprint fingerprint in allFingerprints)
                {
                    // Get fingerprint image
                    Image<Rgba32> fingerPrintImage = Image.Load<Rgba32>(fingerprint.GetPath());

                    // Convert to ascii 8 bit
                    string fingerPrintImageAsciiString = Lib.ImageConverter.ConvertToAscii8Bits(fingerPrintImage);

                    // Input string as pattern
                    // Finger print string as text
                    isMatch = KmpSolver(fingerPrintImageAsciiString, inputCroppedImageAsciiString);

                    if (isMatch)
                    {
                        // Save fingerprint (contains name & path)
                        bestFingerprint = fingerprint;

                        // Also calculate bestSimilarity
                        Image<Rgba32> fingerPrintCroppedImage = Lib.ImageConverter.GetCenteredImage(fingerPrintImage, 32);
                        string fingerPrintCroppedImageAsciiString = Lib.ImageConverter.ConvertToAscii8Bits(fingerPrintCroppedImage);
                        bestSimilarity = LcsSolver(fingerPrintCroppedImageAsciiString, inputCroppedImageAsciiString);

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

                    // Convert to ascii 8 bit
                    string fingerPrintImageAsciiString = Lib.ImageConverter.ConvertToAscii8Bits(fingerPrintImage);

                    // Input string as pattern
                    // Finger print string as text
                    isMatch = BmSolver(fingerPrintImageAsciiString, inputCroppedImageAsciiString);
                    if (isMatch)
                    {
                        // Save fingerprint (contains name & path)
                        bestFingerprint = fingerprint;

                        // Also calculate bestSimilarity
                        Image<Rgba32> fingerPrintCroppedImage = Lib.ImageConverter.GetCenteredImage(fingerPrintImage, 32);
                        string fingerPrintCroppedImageAsciiString = Lib.ImageConverter.ConvertToAscii8Bits(fingerPrintCroppedImage);
                        bestSimilarity = LcsSolver(fingerPrintCroppedImageAsciiString, inputCroppedImageAsciiString);

                        break;
                    }
                }

            }


            // If no match is found using KMP or BM, use LCS to get best match
            double SIMILARITY_LIMIT = 0.6;
            if (!isMatch)
            {
                foreach (Models.Fingerprint fingerprint in allFingerprints)
                {
                    // Get fingerprint image
                    Image<Rgba32> fingerPrintImage = Image.Load<Rgba32>(fingerprint.GetPath());
                    Image<Rgba32> fingerPrintCroppedImage = Lib.ImageConverter.GetCenteredImage(fingerPrintImage, 32);

                    // Convert to ascii 8 bit
                    string fingerPrintCroppedImageAsciiString = Lib.ImageConverter.ConvertToAscii8Bits(fingerPrintCroppedImage);

                    // Get similarity
                    double tempSimilarity = LcsSolver(fingerPrintCroppedImageAsciiString, inputCroppedImageAsciiString);
                    if (tempSimilarity > bestSimilarity && tempSimilarity > SIMILARITY_LIMIT)
                    {
                        // Save name & similarity
                        isMatch = true;
                        bestSimilarity = tempSimilarity;
                        bestFingerprint = fingerprint;
                    }
                }
            }

            // No solution found (KMP/BM or LCM)
            if (!isMatch || bestFingerprint == null)
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

            // Fingerprint data
            fingerPrintData = bestFingerprint;

            // Get all user data
            List<Models.User> allUsers = User.GetUsers();

            // Get user data
            foreach (Models.User user in allUsers)
            {
                // Handle bahasa alay
                string regexName = Lib.Utils.GetRegex(user.GetNama());

                // Check if name is in fingerprint name
                if (System.Text.RegularExpressions.Regex.IsMatch(bestFingerprint.GetNama(), regexName))
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