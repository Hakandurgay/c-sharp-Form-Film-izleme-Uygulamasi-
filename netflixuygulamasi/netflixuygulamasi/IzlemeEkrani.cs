using System;

using System.Windows.Forms;

namespace netflixuygulamasi
{
    public partial class IzlemeEkrani : Form
    {
        Database database = Database.DatabaseOlustur();

        YonetimSayfasi bolumUzunlugu =new YonetimSayfasi();
        private int uzunluk;

        private int sayac;
        private int bolumSayisi;
        public IzlemeEkrani()
        {
            InitializeComponent();
        }

        ~IzlemeEkrani()
        {
            VerileriKaydet();
        }


        private void YarimKalanVarMi()
        {
            if (database.YarimKalmisFilmVarMi() == 0) //yarım kalan yok
            {
                sayac = 0;
        
                bolumSayisi = database.DiziFilmBolumSayisiKontrolEt();

            }
            else  //yarım kalan var
            {

                if (database.DiziMi())
                {
                    sayac = 0;
                    bolumSayisi = database.YarimKalmisFilmVarMi();

                }
                else
                {
                    sayac = database.YarimKalmisFilmVarMi();

                    bolumSayisi = database.DiziFilmBolumSayisiKontrolEt();

                }
                MessageBox.Show("Kalinan Yerden Devam Ediliyor");
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (bolumSayisi == 1)
            {
                label3.Text = "Film";
                label5.Text = "Film Süresi:";
                label4.Text = uzunluk.ToString();
                if (progressBar1.Value == uzunluk)
                {
                    timer1.Stop();
                    MessageBox.Show("İzlemeyi Bitirdiniz");
                }
                else
                {
                    progressBar1.Value = sayac;
                    sayac++;
                    lblSure.Text = sayac.ToString();
                }
            }
            else if (bolumSayisi > 1)
            {
                string toplamBolumSayisi = database.DiziFilmBolumSayisiKontrolEt().ToString();
                label3.Text = "Dizi";
                label5.Text = "Dizi Toplamda " + toplamBolumSayisi + " bolum. Siz ";
                    label4.Text = bolumSayisi.ToString() + ". Bölümü izliyorsunuz";
                    if (progressBar1.Value == uzunluk)
                    {
                        bolumSayisi--;
                        progressBar1.Value = 0;
                        sayac = 0;
                    }
                    else
                    {
                        progressBar1.Value = sayac;
                        sayac++;
                        lblSure.Text = sayac.ToString();
                    }
            }
        }

        private void VerileriKaydet()
        {
            string tarih = "";
            tarih = DateTime.Now.ToString();
            int puan = Convert.ToInt16(textBox1.Text);
            timer1.Stop();
            if (database.YarimKalmisFilmVarMi() == 0) //yarım kalan, yani daha önceden kaydı olan yok sıfırdan yeni kayıt oluşturuluyot
            {
                if (label3.Text == "Dizi") // dizi ise bolumsayisi, değil ise sayaç kaydediliyor
                    database.IzlemeVerileriKaydet(bolumSayisi, tarih, puan);
                else
                    database.IzlemeVerileriKaydet(sayac, tarih, puan);
            }
            else
            {
                if (label3.Text == "Dizi") // dizi ise bolumsayisi, değil ise sayaç kaydediliyor
                    database.IzlemeVerileriGuncelle(bolumSayisi,tarih,puan);
                else
                    database.IzlemeVerileriGuncelle(sayac, tarih, puan);
            }

            this.Hide();
            bolumUzunlugu.Show();
        }


        public void BolumUzunluguAl(int uzunluk)
        {
            this.uzunluk = uzunluk;
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
      
            if (button1.Text == "Başlat")
            {
                timer1.Start();
                button1.Text = "Duraklat";
            }
            else if (button1.Text == "Duraklat")
            {
                timer1.Stop();
                button1.Text = "Başlat";
            }
        }

        private void kaydet_Click(object sender, EventArgs e)
        {
            VerileriKaydet();
        }
        private void IzlemeEkrani_Load(object sender, EventArgs e)
        {

                YarimKalanVarMi();
        }
    }
}
