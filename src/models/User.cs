using System;

namespace Models
{
    public class User(string nik, string nama, string tempatLahir, DateTime tanggalLahir, string jenisKelamin, string golonganDarah, string alamat, string agama, string statusPerkawinan, string pekerjaan, string kewarganegaraan)
    {
        // Attributes
        private readonly string nik = nik;
        private readonly string nama = nama;
        private readonly string tempatLahir = tempatLahir;
        private readonly DateTime tanggalLahir = tanggalLahir;
        private readonly string jenisKelamin = jenisKelamin;
        private readonly string golonganDarah = golonganDarah;
        private readonly string alamat = alamat;
        private readonly string agama = agama;
        private readonly string statusPerkawinan = statusPerkawinan;
        private readonly string pekerjaan = pekerjaan;
        private readonly string kewarganegaraan = kewarganegaraan;

        // Getters
        public string GetNik()
        {
            return nik;
        }

        public string GetNama()
        {
            return nama;
        }

        public string GetTempatLahir()
        {
            return tempatLahir;
        }

        public DateTime GetTanggalLahir()
        {
            return tanggalLahir;
        }

        public string GetJenisKelamin()
        {
            return jenisKelamin;
        }

        public string GetGolonganDarah()
        {
            return golonganDarah;
        }

        public string GetAlamat()
        {
            return alamat;
        }

        public string GetAgama()
        {
            return agama;
        }

        public string GetStatusPerkawinan()
        {
            return statusPerkawinan;
        }

        public string GetPekerjaan()
        {
            return pekerjaan;
        }

        public string GetKewarganegaraan()
        {
            return kewarganegaraan;
        }

        public void Print()
        {
            Console.WriteLine($"NIK: {nik}");
            Console.WriteLine($"Nama: {nama}");
            Console.WriteLine($"Tempat Lahir: {tempatLahir}");
            Console.WriteLine($"Tanggal Lahir: {tanggalLahir}");
            Console.WriteLine($"Jenis Kelamin: {jenisKelamin}");
            Console.WriteLine($"Golongan Darah: {golonganDarah}");
            Console.WriteLine($"Alamat: {alamat}");
            Console.WriteLine($"Agama: {agama}");
            Console.WriteLine($"Status Perkawinan: {statusPerkawinan}");
            Console.WriteLine($"Pekerjaan: {pekerjaan}");
            Console.WriteLine($"Kewarganegaraan: {kewarganegaraan}");
        }
    }
}