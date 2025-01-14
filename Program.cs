using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace OgrenciDersYonetimi
{
    // Temel Sınıf
    public abstract class Kisi
    {
        public string Id { get; set; }
        public string Ad { get; set; }
        public string Email { get; set; }

        public abstract void BilgileriGoster();
    }

    // Arayüz
    public interface ILogin
    {
        bool GirisYap(string email, string sifre);
    }

    // Türemiş Sınıf: Ogrenci
    public class Ogrenci : Kisi, ILogin
    {
        public List<string> KayitliDersler { get; set; } = new List<string>();

        public override void BilgileriGoster()
        {
            Console.WriteLine($"Öğrenci ID: {Id}, Ad: {Ad}, Email: {Email}");
            Console.WriteLine("Kayitlı Dersler: " + string.Join(", ", KayitliDersler));
        }

        public bool GirisYap(string email, string sifre)
        {
            // Basit kimlik doğrulama
            return email == Email && sifre == "ogrenci123";
        }
    }

    // Türemiş Sınıf: OgretimUyesi
    public class OgretimUyesi : Kisi, ILogin
    {
        public List<string> VerilenDersler { get; set; } = new List<string>();

        public override void BilgileriGoster()
        {
            Console.WriteLine($"Öğretim Üyesi ID: {Id}, Ad: {Ad}, Email: {Email}");
            Console.WriteLine("Verilen Dersler: " + string.Join(", ", VerilenDersler));
        }

        public bool GirisYap(string email, string sifre)
        {
            // Basit kimlik doğrulama
            return email == Email && sifre == "ogretim123";
        }
    }

    // Ders Sınıfı
    public class Ders
    {
        public string Ad { get; set; }
        public int Kredi { get; set; }
        public string OgretimUyesiId { get; set; }
        public List<string> KayitliOgrenciIds { get; set; } = new List<string>();

        public void DersBilgileriniGoster()
        {
            Console.WriteLine($"Ders Adı: {Ad}, Kredi: {Kredi}, Öğretim Üyesi ID: {OgretimUyesiId}");
            Console.WriteLine("Kayitli Öğrenciler: " + string.Join(", ", KayitliOgrenciIds));
        }
    }

    class Program
    {
        static List<Ogrenci> ogrenciler = new List<Ogrenci>();
        static List<OgretimUyesi> ogretimUyeleri = new List<OgretimUyesi>();
        static List<Ders> dersler = new List<Ders>();

        static void Main(string[] args)
        {
            VerileriYukle();

            Console.WriteLine("Öğrenci ve Ders Yönetim Sistemi");
            Console.WriteLine("1. Öğrenci Ekle\n2. Öğretim Üyesi Ekle\n3. Ders Ekle\n4. Öğrenci Kaydet\n5. Ders Bilgilerini Göster\n6. Mevcut Öğrencileri Göster\n7. Çık");

            while (true)
            {
                Console.Write("Bir seçenek girin: ");
                var secenek = Console.ReadLine();

                switch (secenek)
                {
                    case "1":
                        OgrenciEkle();
                        break;
                    case "2":
                        OgretimUyesiEkle();
                        break;
                    case "3":
                        DersEkle();
                        break;
                    case "4":
                        OgrenciKaydet();
                        break;
                    case "5":
                        DersBilgileriniGoster();
                        break;
                    case "6":
                        MevcutOgrencileriGoster();
                        break;
                    case "7":
                        VerileriKaydet();
                        return;
                    default:
                        Console.WriteLine("Geçersiz seçenek.");
                        break;
                }
            }
        }

        static void OgrenciEkle()
        {
            Console.Write("Öğrenci ID girin: ");
            string id = Console.ReadLine();
            Console.Write("Ad girin: ");
            string ad = Console.ReadLine();
            Console.Write("Email girin: ");
            string email = Console.ReadLine();

            ogrenciler.Add(new Ogrenci { Id = id, Ad = ad, Email = email });
            Console.WriteLine("Öğrenci başarıyla eklendi.");
        }

        static void OgretimUyesiEkle()
        {
            Console.Write("Öğretim Üyesi ID girin: ");
            string id = Console.ReadLine();
            Console.Write("Ad girin: ");
            string ad = Console.ReadLine();
            Console.Write("Email girin: ");
            string email = Console.ReadLine();

            ogretimUyeleri.Add(new OgretimUyesi { Id = id, Ad = ad, Email = email });
            Console.WriteLine("Öğretim üyesi başarıyla eklendi.");
        }

        static void DersEkle()
        {
            Console.Write("Ders Adı girin: ");
            string ad = Console.ReadLine();
            Console.Write("Kredi girin: ");
            int kredi = int.Parse(Console.ReadLine());
            Console.Write("Öğretim Üyesi ID girin: ");
            string ogretimUyesiId = Console.ReadLine();

            if (ogretimUyeleri.Exists(o => o.Id == ogretimUyesiId))
            {
                dersler.Add(new Ders { Ad = ad, Kredi = kredi, OgretimUyesiId = ogretimUyesiId });
                Console.WriteLine("Ders başarıyla eklendi.");
            }
            else
            {
                Console.WriteLine("Öğretim üyesi bulunamadı.");
            }
        }

        static void OgrenciKaydet()
        {
            Console.Write("Ders Adı girin: ");
            string dersAd = Console.ReadLine();
            Console.Write("Öğrenci ID girin: ");
            string ogrenciId = Console.ReadLine();

            var ders = dersler.Find(d => d.Ad == dersAd);
            var ogrenci = ogrenciler.Find(o => o.Id == ogrenciId);

            if (ders != null && ogrenci != null)
            {
                ders.KayitliOgrenciIds.Add(ogrenciId);
                ogrenci.KayitliDersler.Add(dersAd);
                Console.WriteLine("Öğrenci başarıyla kaydedildi.");
            }
            else
            {
                Console.WriteLine("Ders veya öğrenci bulunamadı.");
            }
        }

        static void DersBilgileriniGoster()
        {
            Console.Write("Ders Adı girin: ");
            string dersAd = Console.ReadLine();

            var ders = dersler.Find(d => d.Ad == dersAd);
            if (ders != null)
            {
                ders.DersBilgileriniGoster();
            }
            else
            {
                Console.WriteLine("Ders bulunamadı.");
            }
        }

        static void MevcutOgrencileriGoster()
        {
            Console.WriteLine("Mevcut Öğrenciler:");
            foreach (var ogrenci in ogrenciler)
            {
                ogrenci.BilgileriGoster();
                Console.WriteLine();  // Öğrenci bilgileri arasında boşluk bırakmak için
            }
        }

        static void VerileriYukle()
        {
            if (File.Exists("ogrenciler.json"))
            {
                try
                {
                    var ogrenciJson = File.ReadAllText("ogrenciler.json");
                    ogrenciler = JsonSerializer.Deserialize<List<Ogrenci>>(ogrenciJson);
                }
                catch (JsonException e)
                {
                    Console.WriteLine("Öğrenciler verisi yüklenirken hata oluştu: " + e.Message);
                }
            }

            if (File.Exists("ogretimUyeleri.json"))
            {
                try
                {
                    var ogretimUyesiJson = File.ReadAllText("ogretimUyeleri.json");
                    ogretimUyeleri = JsonSerializer.Deserialize<List<OgretimUyesi>>(ogretimUyesiJson);
                }
                catch (JsonException e)
                {
                    Console.WriteLine("Öğretim Üyeleri verisi yüklenirken hata oluştu: " + e.Message);
                }
            }

            if (File.Exists("dersler.json"))
            {
                try
                {
                    var dersJson = File.ReadAllText("dersler.json");
                    dersler = JsonSerializer.Deserialize<List<Ders>>(dersJson);
                }
                catch (JsonException e)
                {
                    Console.WriteLine("Dersler verisi yüklenirken hata oluştu: " + e.Message);
                }
            }
        }

        static void VerileriKaydet()
        {
            try
            {
                File.WriteAllText("ogrenciler.json", JsonSerializer.Serialize(ogrenciler));
                File.WriteAllText("ogretimUyeleri.json", JsonSerializer.Serialize(ogretimUyeleri));
                File.WriteAllText("dersler.json", JsonSerializer.Serialize(dersler));
            }
            catch (Exception e)
            {
                Console.WriteLine("Veriler kaydedilirken hata oluştu: " + e.Message);
            }
        }
    }
}
