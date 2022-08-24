using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace KutuphaneOtomasyonu
{
    public partial class girisEkrani : Form
    {
        MySqlDataReader dr;
        MySqlConnection conn;
        MySqlCommand cmd;
        public girisEkrani()
        {
            InitializeComponent();
            conn = new MySqlConnection("server = 172.21.54.3; uid = nistu; password = Okt3571141.; database = nistu");
            panelKapa();
            labelyazi();
        }    

        public void labelyazi()
        {
            cmd = new MySqlCommand();
            conn.Open();
            cmd.Connection = conn;

            cmd.CommandText = "SELECT COUNT(ID) FROM kitap";
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            string c = count.ToString();
            label5.Text = $"Toplam \nKitap Sayısı: {c}";

            cmd.CommandText = "SELECT COUNT(ID) FROM ogrenci";
            int count2 = Convert.ToInt32(cmd.ExecuteScalar());
            string c2 = count2.ToString();
            label6.Text = $"Toplam \nÖğrenci Sayısı: {c2} ";

            cmd.CommandText = "SELECT OgrenciID FROM emanet_islem GROUP BY OgrenciID ORDER BY count(*) DESC LIMIT 1";
            dr = cmd.ExecuteReader();
            dr.Read();
                string ogrID = dr.GetString("OgrenciID");
                dr.Close();
                cmd.CommandText = $"SELECT Ad,Soyad FROM ogrenci WHERE ID= {ogrID}";
                dr = cmd.ExecuteReader();
                dr.Read();              
                label7.Text = $"En Çok Kitap \nOkuyan Öğrenci: {dr.GetString("Ad")} {dr.GetString("Soyad")}";
                dr.Close();              
            
            cmd.CommandText = "SELECT KitapID FROM emanet_islem GROUP BY KitapID ORDER BY count(*) DESC LIMIT 1";
            dr = cmd.ExecuteReader();
            dr.Read();
                string kitapID = dr.GetString("KitapID");
                dr.Close();
                cmd.CommandText = $"SELECT KitapAdi, YazarAdi FROM kitap WHERE ID= {kitapID}";
                dr = cmd.ExecuteReader();
                dr.Read();
                label8.Text = $"En Çok \nOkunan Kitap: {dr.GetString("KitapAdi")} - {dr.GetString("YazarAdi")}";
                dr.Close();

            cmd.CommandText = "SELECT KitapID FROM emanet_islem GROUP BY KitapID ORDER BY count(*) DESC LIMIT 1";
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                string kitapid = dr.GetString("KitapID");
                dr.Close();
                cmd.CommandText = $"SELECT YazarAdi FROM kitap WHERE ID= {kitapid}";
                dr = cmd.ExecuteReader();
                dr.Read();
                label9.Text = $"En Çok \nOkunan Yazar: {dr.GetString("YazarAdi")}";
                dr.Close();
            }

            cmd.CommandText = "SELECT COUNT(DISTINCT YazarAdi) FROM kitap";
            int count3 = Convert.ToInt32(cmd.ExecuteScalar());
            string c3 = count3.ToString();
            label10.Text = $"Toplam \nYazar Sayısı: {c3}";
            conn.Close();
        }    
        public void girisBttn_Click(object sender, EventArgs e)
        {
            cmd = new MySqlCommand();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM personel WHERE KullaniciAdi ='" + kullaniciAdiTbox.Text + "' AND Sifre ='" + sifreTbox.Text + "'";
            
            dr = cmd.ExecuteReader();
            
            if (dr.Read())
            {
                personelAnasayfa personelAnasayfa = new personelAnasayfa();
                personelAnasayfa.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Kullanıcı adını ya da şifrenizi yanlış girdiniz.");
            }
            conn.Close();
        }
        public void panelKapa()
        {
            panelOgr.Visible = false;
            panelP.Visible = false;
            panel1.Visible = false;
        }
        private void pGiris_Click(object sender, EventArgs e)
        {
            panelKapa();
            panelP.Visible = true;
        }
        private void ogrGiris_Click(object sender, EventArgs e)
        {
            panelKapa();
            panelOgr.Visible = true;
        }
        public void girisOgr_Click(object sender, EventArgs e)
        {
            cmd = new MySqlCommand();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = $"SELECT * FROM ogrenci WHERE OkulNumarasi = '{okul_numarasi.Text}' AND OgrenciSifre ='{ogrSifre.Text}'";
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                var ogrenci = new Ogrenci{OgrNu = dr.GetString("OkulNumarasi")};
                ogrenciAnasayfa ogr = new ogrenciAnasayfa(ogrenci);
                ogr.Show();
                dr.Close();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Kullanıcı adını ya da şifrenizi yanlış girdiniz.");
            }
            conn.Close();
        }
        private void sifreTbox_TextChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                sifreTbox.PasswordChar = '\0';
            }
            else
            {
                sifreTbox.PasswordChar = '*';
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {

            panelKapa();
            panel1.Visible = true;
        }
        private void button6_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Bu otomasyon tamamen Nisa Nur Bayrak tarafından yapılmıştır.");
        }
        private void button7_Click(object sender, EventArgs e)
        {
            cmd = new MySqlCommand();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = $"SELECT * FROM ogrenciIsleri WHERE KullaniciAdi ='{kullaniciAdiTbox.Text}' AND Sifre ='{sifreTbox.Text}'";

            dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                ogrIsleriAnasayfa ogrIsleri = new ogrIsleriAnasayfa();
                ogrIsleri.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Kullanıcı adını ya da şifrenizi yanlış girdiniz.");
            }
            conn.Close();
        }
        private void button6_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Bu Otomasyon Nisa Nur Bayrak Tarafından Yazılmıştır. Keyifli Okumalar :)");
        }

        private void ogrSifre_TextChanged(object sender, EventArgs e)
        {
            if (cb2.Checked)
            {
                ogrSifre.PasswordChar = '\0';
            }
            else
            {
                ogrSifre.PasswordChar = '*';
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
