using System;
using System.Collections.Generic;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace controllers
{
    // Uses KMP or BM Algorithm to find perfect match
    // Uses Hamming Distance to find similarity

    public class Solver(string inputImagePath, bool isKmp)
    {
        // Input data
        // finger print
        private string inputImagePath = inputImagePath;

        // is kmp
        private bool isKmp = isKmp;

        // Output data
        // User data
        private Models.User? userData;
        // Finger print data
        private Models.Fingerprint? fingerPrintData;
        // similarity
        private double? similarity;
        // time
        private double? time;

        // Use LCS to find similarity
        private double LcsSolver(string s1, string s2)
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
        private bool KmpSolver(string text, string pattern)
        {
            // Get length of both strings
            int n = text.Length;
            int m = pattern.Length;


            // Create LPS array
            int[] lps = new int[m];
            int i = 0;
            int j = 0;
            // Iterate through the pattern
            while (i < m)
            {
                // If the characters are the same
                if (pattern[i] == pattern[j])
                {
                    lps[i] = j + 1;
                    i++;
                    j++;
                }
                // If the characters are different
                else if (j > 0)
                {
                    j = lps[j - 1];
                }
                // If j is at the beginning of the pattern
                else
                {
                    lps[i] = 0;
                    i++;
                }
            }

            // Do KMP Algorithm
            i = 0;
            j = 0;
            while (i < n)
            {
                // If the characters are the same
                if (text[i] == pattern[j])
                {
                    if (j == m - 1)
                    {
                        // Found pattern
                        return true;
                    }
                    i++;
                    j++;
                }
                // If the characters are different
                else if (j > 0)
                {
                    j = lps[j - 1];
                }
                // If j is at the beginning of the pattern
                else
                {
                    i++;
                }
            }

            // Not found pattern
            return false;
        }

        // BM Solver
        private bool BmSolver(string text, string pattern)
        {
            // Get length of both strings
            int n = text.Length;
            int m = pattern.Length;

            // Create bad character array
            int[] badChar = new int[256];

            // Fill bad character array
            int i;
            for (i = 0; i < 256; i++)
            {
                badChar[i] = -1;
            }

            for (i = 0; i < m; i++)
            {
                badChar[pattern[i]] = i;
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
                    int k = badChar[text[i]];
                    i = i + m - Math.Min(j, 1 + k);
                    j = m - 1;
                }
            } while (i <= n - 1);

            return false;
        }


        // Solver
        public void Solve()
        {
            if (isKmp)
            {
                KmpSolver();
            }
            else
            {
                BmSolver();
            }
        }
    }
}