using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Models {
    public class Utils
    {
        public static string GetBahasaAlay(string inputString)
        {
            // Generate Kombinasi tiga variasi bahasa alay
            /*
                Kata orisinil | Bintang Dwi Marthen
                Kombinasi huruf besar-kecil | bintanG DwI mArthen
                Penggunaan angka | B1nt4n6 Dw1 M4rthen
                Penyingkatan | Bntng Dw Mrthen
                Kombinasi ketiganya | b1ntN6 Dw mrthn
            */

            // Kombinasi huruf besar-kecil
            string res = "";
            for (int i = 0; i < inputString.Length; i++)
            {
                // Randomize huruf besar atau kecil
                Random random = new();
                bool isUpperCase = random.Next(0, 2) == 0;

                // Tambahkan huruf ke kombinasi
                res += isUpperCase ? inputString[i].ToString().ToUpper() : inputString[i].ToString().ToLower();
            }

            // Penggunaan angka (edit from res)
            Dictionary<string, string> alay2 = new()
            {
                { "a", "4" },
                { "i", "1" },
                { "e", "3" },
                { "o", "0" },
                { "s", "5" },
                { "g", "6" },
            };
            for (int i = 0; i < res.Length; i++)
            {
                Random random = new();
                bool shouldReplace = random.Next(0, 2) == 0;
                bool replacable = alay2.ContainsKey(res[i].ToString().ToLower());
                if (shouldReplace && replacable)
                {
                    res = res.Substring(0, i) + alay2[res[i].ToString().ToLower()] + res.Substring(i + 1);
                }
            }

            // Penyingkatan (edit from res)
            List<string> alay3 =
            [
                "a", "i", "u", "e", "o"
            ];
            for (int i = 0; i < res.Length; i++)
            {
                Random random = new();
                bool shouldReplace = random.Next(0, 2) == 0;
                bool replacable = alay3.Contains(res[i].ToString().ToLower());
                if (shouldReplace && replacable)
                {
                    res = res.Substring(0, i) + res.Substring(i + 1);
                }
            }

            return res;
        }
    }
}