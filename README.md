# GoyangSinggalang

## Penjelasan Algoritma KMP & BM

Algoritma Knuth-Morris-Pratt (KMP) digunakan untuk mencari sebuah substring (pola) dalam sebuah string (teks) secara efisien. Algoritma ini mempercepat pencarian dengan memanfaatkan informasi dari pola itu sendiri untuk menghindari pemeriksaan ulang karakter yang sudah diperiksa. Pertama, dilakukan preprocessing dengan pembuatan array LPS. Buat array Longest Prefix Suffix (LPS) untuk pola yang menunjukkan panjang prefix terpanjang dari pola yang juga merupakan suffix. Setelah itu, lakukan pencarian. Mulai mencocokkan pola dengan teks. Jika karakter cocok, lanjutkan ke karakter berikutnya. Jika terjadi mismatch, gunakan nilai dari LPS array untuk menggeser pola tanpa harus mencocokkan ulang karakter yang sudah sesuai sebelumnya. Kompleksitas waktu O(n + m), di mana n adalah panjang teks dan m adalah panjang pola.

Algoritma Boyer-Moore (BM) juga digunakan untuk mencari substring dalam string, tetapi lebih efisien dibandingkan KMP pada teks yang besar. Algoritma ini bekerja dengan memulai pencocokan dari akhir pola ke awal, dan menggunakan dua heuristik untuk mempercepat pencarian: Bad Character Heuristic dan Good Suffix Heuristic. Pertama, dilakukan preprocessing buat tabel yang mencatat posisi terakhir dari setiap karakter dalam pola. Setelah itu, lakukan pencarian. Mulai mencocokkan pola dengan teks dari kanan ke kiri. Jika terjadi mismatch, gunakan Bad Character atau Good Suffix untuk menggeser pola ke kanan. Kompleksitas waktu rata-rata mendekati O(n/m), di mana n adalah panjang teks dan m adalah panjang pola, karena pergeseran yang lebih besar dibandingkan KMP.

## Requirement program dan instalasi tertentu bila ada

- Dotnet
- Linux / Windows
- MySQL

## How To Run

### Setup Database

1. Buka MySQL dengan user root lalu buat database bernama "tubes3_stima".

```sql
CREATE DATABASE tubes3_stima;
```

2. Buat user dengan username "college" dan password "12345" di local database.

```sql
CREATE USER 'college'@'localhost' IDENTIFIED BY '12345';
```

3. Berikan semua privilege pada user tersebut pada database tubes3_stima

```sql
GRANT ALL PRIVILEGES ON tubes3_stima.* TO 'college'@'localhost';
```

4. Load hasil seeding encrypted (dump pada src/db/seeded.sql)

```bash
make load-dump
```

### Jalankan Program

Jalankan script shell gui.sh

```bash
make run
```

### Beberapa command tambahan

1. Migrate / import schema database dengan encryption

```bash
make migrate
```

2. Generate seeding baru dengan encryption AES

```bash
make seed
```

3. Migrate / import schema database raw (tanpa encryption)

```bash
make raw-migrate
```

4. Generate seed baru tanpa encryption sama sekali

```bash
make raw-seed
```

5. Convert hasil dump test case asisten ke schema enkripsi serta menenkripsi data-datanya juga. (Pastikan data sudah diload terlebih dahulu, bagian ini hanya mengkonversikan saja.)

```bash
make convert-dump
```

6. Run real stress test

```bash
make stress-real
```

7. Run altered easy stress test

```bash
make stress-easy
```

8. Run altered medium stress test

```bash
make stress-medium
```

9. Run altered hard stress test

```bash
make stress-hard
```

## Author (Identitas Pembuat)

|   NIM    |           Nama           |
| :------: | :----------------------: |
| 13522011 |   Dewantoro Triatmojo    |
| 13522066 | Nyoman Ganadipa Narayana |
| 13522067 |      Randy Verdian       |
