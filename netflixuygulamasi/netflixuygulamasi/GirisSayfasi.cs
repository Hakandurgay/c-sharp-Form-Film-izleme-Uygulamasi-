using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace netflixuygulamasi
{
    public partial class formGiris : Form
    {
        public formGiris()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }



        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnGiris_Click(object sender, EventArgs e)
        {

        }

        private void KullaniciKayitEt()
        {
            KayitSayfasi kullaniciKayitSayfasi = new KayitSayfasi();
            this.Hide();
            kullaniciKayitSayfasi.Show();
        }

        private void YonetimSayfasiAc()
        {
            YonetimSayfasi yonetimSayfasi = new YonetimSayfasi();
            this.Hide();
            yonetimSayfasi.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Database kontrol = Database.DatabaseOlustur();
            if (kontrol.KullaniciKontrolEt(this))
            {
                YonetimSayfasiAc();
            }
            else
            {
                if (txtMail.Text == "" || txtSifre.Text == "")
                    MessageBox.Show("Bütün Alanları Doldurunuz");
                else
                    KullaniciKayitEt();
            }
        }
    }
}
