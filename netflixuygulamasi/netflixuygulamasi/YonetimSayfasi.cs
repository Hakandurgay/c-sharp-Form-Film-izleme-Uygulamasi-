using System;

using System.Windows.Forms;

namespace netflixuygulamasi
{
    public partial class YonetimSayfasi : Form
    {
        private Database ara = Database.DatabaseOlustur();
        
        public YonetimSayfasi()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Database ara = Database.DatabaseOlustur();
            if (txtAdAra.Text != "")
            {
                ara.ProgramAraIsim(this);
            }
            else
            {
               ara.ProgramAraTur(this);

            }

        }

        private void YonetimSayfasi_Load(object sender, EventArgs e)
        {
            ara.ComboboboxTuradlariSirala(this);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            BolumIzle(ara.BolumUzunluguGetir(this,e)); //veritabanından bölüm uzunluğu getiriliyor ve timera atanıyor
        }

        private void BolumIzle(int bolumUzunlugu)
        {
            IzlemeEkrani izle=new IzlemeEkrani();
            izle.Show();
            izle.progressBar1.Maximum = bolumUzunlugu; //Bölümün uzunluğu kadar kapasite ayarlanıyor
            izle.BolumUzunluguAl(bolumUzunlugu);
        }

        private void txtAdAra_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
