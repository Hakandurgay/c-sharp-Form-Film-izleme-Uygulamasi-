using System;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
namespace netflixuygulamasi
{
    class Database
    {
        public static Database nesne;
        private string girisYapanKullaniciMail = "";
        private string izlenecekFilm = "";
        private int[] secim = new int[4];

        public static Database DatabaseOlustur() //diğer classlarda kullanılması için fazladan nesne oluşturmaya gerek olmadığı için sigleton deseni kullanıldı
        {
            if (nesne == null)
                nesne = new Database();
            return nesne;
        }

        private Database()
        {

        }

        public SqlConnection F_Baglanti()
        {
            var conStr = System.Configuration.ConfigurationManager.AppSettings["ConStr"];
            SqlConnection baglan =
                new SqlConnection(conStr);  // bu yolu debug klasöründeki config dosyasından değiştirebilisiniz
            baglan.Open();
            return baglan;
        }

        public bool KullaniciKontrolEt(formGiris girisFormu) //kullanici girişi başarılıysa true döndürecek, değilse false
        {

            string mail = girisFormu.txtMail.Text;
            string sifre = girisFormu.txtSifre.Text;
            SqlCommand command = new SqlCommand();
            command.Connection = F_Baglanti();
            command.CommandText = "select * from Tbl_Kullanici where email='" + girisFormu.txtMail.Text + "'" +
                                  "and sifre='" + girisFormu.txtSifre.Text + "'";
            girisYapanKullaniciMail = girisFormu.txtMail.Text;
            SqlDataReader dt = command.ExecuteReader();
            if (dt.Read())
                return true;
            else
            {
                return false;
            }
        }

        public void KullaniciEkle(KayitSayfasi kayitForm)    //kullanıcı girişi başarısırızsa  kullanici tablosuna kullanıcıyı kaydeder
        {
            SqlCommand komut =
                new SqlCommand(
                    "insert into Tbl_Kullanici (kullaniciAd,email,sifre,dogumTarihi) values (@t1,@t2,@t3,@t4)",
                    F_Baglanti());
            komut.Parameters.AddWithValue("@t1", kayitForm.kTxtAd.Text);
            komut.Parameters.AddWithValue("@t2", kayitForm.kTxtEmail.Text);
            komut.Parameters.AddWithValue("@t3", kayitForm.kTxtSifre.Text);
            komut.Parameters.AddWithValue("@t4", kayitForm.kTxtDogumTarihi.Text);
            komut.ExecuteNonQuery();
            F_Baglanti().Close();

        }

        public void FilmListele(KayitSayfasi kayit, int seciliDeger, int sayac)   //kullanıcı kayıt sayfasında checkboxlarda seçili olan verinin adını alır, inner joinler kullanılarak üç farklı tablodan gerekli veriler alınır
        {

            SqlCommand komut = new SqlCommand(@"SELECT top 2  Tbl_Program.programAd
                FROM  Tbl_ProgramTur INNER JOIN
                Tbl_KullaniciProgram ON Tbl_ProgramTur.programId = Tbl_KullaniciProgram.kullaniciprogramId INNER JOIN
            Tbl_Program ON Tbl_ProgramTur.programId = Tbl_Program.programId
            WHERE(Tbl_ProgramTur.turId IN	(@t1))
            ORDER BY Tbl_KullaniciProgram.kullaniciPuan DESC", F_Baglanti());
            komut.Parameters.AddWithValue("@t1", seciliDeger);

            DataTable data = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(komut);
            adapter.Fill(data);
            if (sayac == 1)
                kayit.dataGridView1.DataSource = data;
            else if (sayac == 2)
                kayit.dataGridView2.DataSource = data;
            else if (sayac == 3)
                kayit.dataGridView3.DataSource = data;
        }

        public void ProgramAraIsim(YonetimSayfasi yonetim)
        {
            SqlCommand komut =
                new SqlCommand(
                    @"SELECT p.programAd FROM Tbl_Program AS p WHERE p.programAd='" + yonetim.txtAdAra.Text + "'",
                    F_Baglanti());
            DataTable data = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(komut);
            adapter.Fill(data);

            yonetim.dataGridView1.DataSource = data;
        } // Giriş yapıldıktan sonra isime göre arama yapar

        public void ComboboboxTuradlariSirala(YonetimSayfasi yonetim)
        {
            SqlCommand komut =
                new SqlCommand(@"SELECT turAd FROM Tbl_Tur ", F_Baglanti());
            DataTable data = new DataTable();
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                yonetim.cmbTur.Items.Add(read["turAd"]);

            }

            yonetim.dataGridView1.DataSource = data;
        }  //tür adlarına göre arama yapılabilmesi için comboboxa tür adlarını yazdırır

        public void ProgramAraTur(YonetimSayfasi yonetim)   //burayı düzelt. kullanıcı program sayfasında olan türleri getiriyor bütün türleri getirmesi lazım
        {
            SqlCommand komut =
                new SqlCommand(
                    @"SELECT  DISTINCT Tbl_Program.programAd
                FROM  Tbl_ProgramTur INNER JOIN
            Tbl_Program ON Tbl_ProgramTur.programId = Tbl_Program.programId
            WHERE(Tbl_ProgramTur.turId =(SELECT turId FROM Tbl_Tur WHERE turAd=(@p1)))", F_Baglanti());

            komut.Parameters.AddWithValue("@p1", Convert.ToString(yonetim.cmbTur.SelectedItem));

            DataTable data = new DataTable();

            SqlDataAdapter adapter = new SqlDataAdapter(komut);
            adapter.Fill(data);

            yonetim.dataGridView1.DataSource = data;
        } //comboboxtaki seçili veriyi kullanarak türe göre arama yapar

        public int BolumUzunluguGetir(YonetimSayfasi yonetim, DataGridViewCellEventArgs e) //bölümün uzunluğunu bulur
        {
            int uzunluk = 0;

            if (e.ColumnIndex == 0)
            {

                SqlCommand komut =
                    new SqlCommand(@"SELECT uzunluk FROM Tbl_Program WHERE programAd =@p1", F_Baglanti());
                if (yonetim.dataGridView1.CurrentRow != null)
                    komut.Parameters.AddWithValue("@p1",
                        yonetim.dataGridView1.CurrentRow.Cells["programAd"].Value.ToString());
                if (yonetim.dataGridView1.CurrentRow != null)
                    izlenecekFilm = yonetim.dataGridView1.CurrentRow.Cells["programAd"].Value.ToString();
                DataTable data = new DataTable();
                SqlDataReader read = komut.ExecuteReader();

                while (read.Read())
                {
                    uzunluk = Convert.ToInt16(read["uzunluk"]);
                }
            }
            return uzunluk;
        }
        public int YarimKalmisFilmVarMi()  //filmi izlemeye başlamadan önce daha önce izlemeye başlanan ve yarım kalmış film var mı diye kontrol eder
        {
            int kalinanYer = 0;
            SqlCommand command = new SqlCommand();
            command.Connection = F_Baglanti();
            command.CommandText =
                "SELECT kullaniciProgramId,kullaniciId,kalinanYer FROM Tbl_KullaniciProgram  WHERE kullaniciprogramId=(SELECT programId FROM Tbl_Program WHERE programAd=@programadi) AND kullaniciId=(SELECT kullaniciId FROM Tbl_Kullanici WHERE email=(@kullanicimaili))";
            command.Parameters.AddWithValue("@programadi", izlenecekFilm);
            command.Parameters.AddWithValue("@kullanicimaili", girisYapanKullaniciMail);

            SqlDataReader dt = command.ExecuteReader();


            while (dt.Read())
                kalinanYer = Convert.ToInt16(dt["kalinanYer"]);
            return kalinanYer;
        }
        public void IzlemeVerileriKaydet(int izlenenZaman, string tarih, int puan)  //eğer daha önce seçili kullanıcıya ait izlenmiş film bulunmuyorsa kulalniciprogram tablosuna yeni kayıt ekler
        {
            SqlCommand komut = new SqlCommand(
                "INSERT INTO Tbl_KullaniciProgram(kullaniciprogramId,kullaniciId,kalinanYer,izlemeTarihi,kullaniciPuan)VALUES((SELECT programId FROM Tbl_Program WHERE programAd = (@programadi)), (SELECT kullaniciId FROM Tbl_Kullanici WHERE email = (@kullanicimaili)),@kalinanYer,@tarih,@puan)",
                F_Baglanti());

            komut.Parameters.AddWithValue("@programadi", izlenecekFilm);
            komut.Parameters.AddWithValue("@kullanicimaili", girisYapanKullaniciMail);
            komut.Parameters.AddWithValue("@kalinanYer", izlenenZaman);
            komut.Parameters.AddWithValue("@tarih", tarih);
            komut.Parameters.AddWithValue("@puan", puan);


            komut.ExecuteNonQuery();
            F_Baglanti().Close();
        }

        public void IzlemeVerileriGuncelle(int izlenenZaman, string tarih, int puan) // kullaniciprogram tablosunda daha önceden kayıt var ise yeni kayıt oluşturmak yerine öncekinin üstüne yazılıyor
        {
            SqlCommand command = new SqlCommand();
            command.Connection = F_Baglanti();
            command.CommandText = "UPDATE Tbl_KullaniciProgram SET kalinanYer = @kalinanYer, izlemeTarihi =@tarih , kullaniciPuan = @puan WHERE kullaniciprogramId = (SELECT programId FROM Tbl_Program WHERE programAd = (@programadi)) and kullaniciId = (SELECT kullaniciId FROM Tbl_Kullanici WHERE email = (@kullanicimaili)) ";
            command.Parameters.AddWithValue("@programadi", izlenecekFilm);
            command.Parameters.AddWithValue("@kullanicimaili", girisYapanKullaniciMail);
            command.Parameters.AddWithValue("@kalinanYer", izlenenZaman);
            command.Parameters.AddWithValue("@tarih", tarih);
            command.Parameters.AddWithValue("@puan", puan);
            command.ExecuteNonQuery();
            F_Baglanti().Close();
        }
        public int DiziFilmBolumSayisiKontrolEt() // dizinin bölüm sayısını kontrol eder eğer film ise dönücek değer hep 1 olacak şekilde kabul edildi
        {
            int bolumSayisi = 1;
            SqlCommand komut = new SqlCommand("SELECT bolumSayisi FROM Tbl_Program WHERE programAd=(@programAdi)",
                F_Baglanti());
            komut.Parameters.AddWithValue("@programAdi", izlenecekFilm);

            SqlDataReader dt = komut.ExecuteReader();

            while (dt.Read())
                bolumSayisi = Convert.ToInt16(dt["bolumSayisi"]);

            return bolumSayisi;
        }

        public bool DiziMi()   //kayıt yapılırken dizi ise kullaniciprogram tablosuna bölüm uzunluğunu, film ise izlenen süreyi yazması için kontrol fonksiyonu
        {
            SqlCommand command = new SqlCommand();
            command.Connection = F_Baglanti();
            command.CommandText = "SELECT programTip FROM Tbl_Program WHERE programAd=(@p1)";
            command.Parameters.AddWithValue("@p1", izlenecekFilm);
            SqlDataReader dt = command.ExecuteReader();
            string bolumTur = "";

            while (dt.Read())
                bolumTur = dt["programTip"].ToString();
            if (bolumTur == "Dizi")
                return true;
            else
                return false;

        }


    }
}
