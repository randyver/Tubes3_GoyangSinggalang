using System;

namespace Models
{
    public class Fingerprint(string nama, string path)
    {
        // Attributes
        private readonly string nama = nama;

        private readonly string path = path;

        // Getters
        public string GetNama()
        {
            return nama;
        }

        public string GetPath()
        {
            return path;
        }

        public void Print()
        {
            Console.WriteLine($"Nama: {nama}");
            Console.WriteLine($"Path: {path}");
        }
    }

}