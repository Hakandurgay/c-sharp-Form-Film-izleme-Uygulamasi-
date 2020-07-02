using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace netflixuygulamasi
{
    public partial class KayitSayfasi : Form
    {
        private Database kayit = Database.DatabaseOlustur();
        private int sayac = 0;
        public KayitSayfasi()
        {
            InitializeComponent();
        }

        private void btnKayit_Click(object sender, EventArgs e)
        {
                 kayit.KullaniciEkle(this);
            this.Hide();
            Thread.Sleep(800);

              MessageBox.Show("Kaydınız Tamamlandı Giriş İçin Giriş Sayfasına Yönlendiriliyorsunuz");
            formGiris giris=new formGiris();
            giris.Show();

        }

        private void chkAksiyon_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAksiyon.Checked == false)
                sayac--;
            else
            {
                sayac++;
                if (sayac <= 3)
                    kayit.FilmListele(this, 1, sayac);
                else
                {
                    MessageBox.Show("Sadece Üç Tane Seçim Yapınız");
                }
            }
         
        }

        private void chkBilimKurgu_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBilimKurgu.Checked == false)
                sayac--;
            else
            {
                sayac++;
                if (sayac <= 3)
                    kayit.FilmListele(this, 3, sayac);
                else
                    MessageBox.Show("Sadece Üç Tane Seçim Yapınız");
            }
            
        }

        private void chkBelgesel_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBelgesel.Checked == false)
                sayac--;
            else
            {
                sayac++;
                if (sayac <= 3)
                    kayit.FilmListele(this, 2, sayac);
                else
                    MessageBox.Show("Sadece Üç Tane Seçim Yapınız");
            }

        }

        private void chkBilimDoga_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBilimDoga.Checked == false)
                sayac--;
            else
            {
                sayac++;
                if (sayac <= 3)
                    kayit.FilmListele(this, 5, sayac);
                else
                    MessageBox.Show("Sadece Üç Tane Seçim Yapınız");
            }
     
        }

        private void chkCocukAile_CheckedChanged(object sender, EventArgs e)
        {
            if(chkCocukAile.Checked==false)
                sayac--;
            else
            {
                sayac++;
                if (sayac <= 3)
                    kayit.FilmListele(this, 6, sayac);
                else
                    MessageBox.Show("Sadece Üç Tane Seçim Yapınız");
            }
     
        }

        private void chkKomedi_CheckedChanged(object sender, EventArgs e)
        {
            if (chkKomedi.Checked == false)
                sayac--;
            else
            {
                sayac++;
                if (sayac <= 3)
                    kayit.FilmListele(this, 9, sayac);
                else
                    MessageBox.Show("Sadece Üç Tane Seçim Yapınız");
            }
       
        }

        private void chkKorku_CheckedChanged(object sender, EventArgs e)
        {
            if (chkKorku.Checked == false)
                sayac--;
            else
            {
                sayac++;
                if (sayac <= 3)
                    kayit.FilmListele(this, 10, sayac);
                else
                    MessageBox.Show("Sadece Üç Tane Seçim Yapınız");
            }
    
        }

        private void chkDrama_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDrama.Checked == false)
                sayac--;
            else
            {
                sayac++;
                if (sayac <= 3)
                    kayit.FilmListele(this, 7, sayac);
                else
                    MessageBox.Show("Sadece Üç Tane Seçim Yapınız");
            }
         
        }

        private void chkRomantizm_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRomantizm.Checked == false)
                sayac--;
            else
            {
                sayac++;
                if (sayac <= 3)
                    kayit.FilmListele(this, 11, sayac);
                else
                    MessageBox.Show("Sadece Üç Tane Seçim Yapınız");

            }
     
        }

        private void chkGerilim_CheckedChanged(object sender, EventArgs e)
        {
            if (chkGerilim.Checked == false)
                sayac--;
            else
            {
                sayac++;
                if (sayac <= 3)
                    kayit.FilmListele(this, 8, sayac);
                else
                    MessageBox.Show("Sadece Üç Tane Seçim Yapınız");
            }
         
        }

        private void DataGridViewSettings(DataGridView data)
        {
            data.BorderStyle = BorderStyle.None;
        }
    }
}
