using System;
using System.Collections.Generic;
using System.Threading;

class Motor
{
    public int Id { get; set; }
    public double Sicaklik { get; set; }
    public double Vibrasyon { get; set; }
    public bool Arizali { get; set; }

    public Motor(int id, double sicaklik, double vibrasyon)
    {
        Id = id;
        Sicaklik = sicaklik;
        Vibrasyon = vibrasyon;
        Arizali = false;
    }

    public void SicaklikArtir(double disOrtamSicakligi, double motorYuku)
    {
        double sicaklikArtisi = (new Random().NextDouble() * 2) + (disOrtamSicakligi * 0.1) + (motorYuku * 0.2);
        Sicaklik += sicaklikArtisi;

        if (Sicaklik > 55)
        {
            Arizali = true;
            Console.WriteLine($"Motor {Id} sıcaklığı 55°C'nin üzerinde! Sıcaklık: {Sicaklik:0.00}°C");
            Thread.Sleep(10000); // 10 saniye bekle
        }
    }

    public void DurumYazdir()
    {
        if (Arizali)
        {
            Console.WriteLine($"Motor {Id} arızalı! Sıcaklık: {Sicaklik:0.00}°C, Vibrasyon: {Vibrasyon:0.00}");
        }
        else
        {
            Console.WriteLine($"Motor {Id} sorunsuz. Sıcaklık: {Sicaklik:0.00}°C, Vibrasyon: {Vibrasyon:0.00}");
        }
    }
}

class MotorSicaklikKontrolu
{
    static void Main()
    {
        int motorSayisi = 5;
        double[] baslangicSicakliklari = new double[motorSayisi];
        double[] baslangicVibrasyonlari = new double[motorSayisi];

        Random rand = new Random();
        for (int i = 0; i < motorSayisi; i++)
        {
            baslangicSicakliklari[i] = 20.0 + rand.NextDouble() * 5; // Başlangıç sıcaklığı 20 ile 25 derece arasında
            baslangicVibrasyonlari[i] = rand.NextDouble() * 5; // Başlangıç vibrasyon değeri 0 ile 5 arasında
        }

        List<Motor> motorlar = new List<Motor>();
        for (int i = 0; i < motorSayisi; i++)
        {
            motorlar.Add(new Motor(i + 1, baslangicSicakliklari[i], baslangicVibrasyonlari[i]));
        }

        double disOrtamSicakligi = 25.0; // Sabit dış ortam sıcaklığı
        List<int> arizaliMotorlar = new List<int>();

        Console.WriteLine("Motor sıcaklıklarını ve vibrasyonlarını manuel olarak girmek ister misiniz? (E/H)");
        string kullaniciCevap = Console.ReadLine();

        if (kullaniciCevap.ToUpper() == "E")
        {
            double[] girilenSicakliklar = new double[motorSayisi];
            double[] girilenVibrasyonlar = new double[motorSayisi];

            for (int i = 0; i < motorSayisi; i++)
            {
                Console.WriteLine($"Motor {i + 1} için sıcaklık girin: ");
                string sicaklikInput = Console.ReadLine();
                if (double.TryParse(sicaklikInput, out double sicaklik))
                {
                    girilenSicakliklar[i] = sicaklik;
                }
                else
                {
                    Console.WriteLine("Geçersiz sıcaklık değeri girildi. Rastgele değer kullanılacak.");
                    girilenSicakliklar[i] = baslangicSicakliklari[i];
                }

                Console.WriteLine($"Motor {i + 1} için vibrasyon girin: ");
                string vibrasyonInput = Console.ReadLine();
                if (double.TryParse(vibrasyonInput, out double vibrasyon))
                {
                    girilenVibrasyonlar[i] = vibrasyon;
                }
                else
                {
                    Console.WriteLine("Geçersiz vibrasyon değeri girildi. Rastgele değer kullanılacak.");
                    girilenVibrasyonlar[i] = baslangicVibrasyonlari[i];
                }
            }

            for (int i = 0; i < motorSayisi; i++)
            {
                motorlar[i].Sicaklik = girilenSicakliklar[i];
                motorlar[i].Vibrasyon = girilenVibrasyonlar[i];
            }
        }

        bool devamEtsin = true;
        while (devamEtsin)
        {
            Console.WriteLine("Yeni bir ölçüm yapmak istiyor musunuz? (E/H)");
            string devamCevabi = Console.ReadLine();

            if (devamCevabi.ToUpper() != "E")
            {
                devamEtsin = false;
                continue;
            }

            Console.WriteLine("Ölçüm başlıyor...");
            for (int t = 0; t < 10; t++) // 10 ölçüm için döngü
            {
                Console.WriteLine($"\n{t + 1}. Ölçüm:");
                arizaliMotorlar.Clear();

                foreach (var motor in motorlar)
                {
                    double motorYuku = rand.NextDouble(); // 0 ile 1 arasında rastgele motor yükü
                    motor.SicaklikArtir(disOrtamSicakligi, motorYuku);
                    motor.DurumYazdir();

                    if (motor.Arizali && !arizaliMotorlar.Contains(motor.Id))
                    {
                        arizaliMotorlar.Add(motor.Id);
                    }
                }

                // 1 saniye bekle (gerçek zamanlı ölçüm simülasyonu için)
                Thread.Sleep(1000);
            }

            // Analiz sonuçlarını kullanıcıya sun
            if (arizaliMotorlar.Count > 0)
            {
                Console.WriteLine("\nArıza tespit edilen motorlar:");
                foreach (int motorNo in arizaliMotorlar)
                {
                    Console.WriteLine($"Motor {motorNo}");
                }
            }
            else
            {
                Console.WriteLine("\nHiçbir motor arızalı değil.");
            }
        }

        Console.WriteLine("Program sonlandı.");
    }
}
