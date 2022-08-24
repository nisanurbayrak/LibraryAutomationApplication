using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Engines;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace KutuphaneOtomasyonu
{
    public partial class personelAnasayfa : Form
    {
        MySqlDataReader dr;
        MySqlConnection conn;
        MySqlCommand cmd;
        BaseEntity entity = new BaseEntity();
        public personelAnasayfa()
        {
            conn = new MySqlConnection("server = 172.21.54.3; uid = nistu; password = Okt3571141.; database = nistu");
            InitializeComponent();
            panelKapa();
        }
            Islem islem = new Islem();
        #region PANEL
        private void panelKapa()
        {
            ogrenciKayitPanel.Visible = false;
            kitapEklePanel.Visible = false;
            emanetIslemPanel.Visible = false;
            kitapAraPanel.Visible = false;
            cezaPanel.Visible = false;
            panel1.Visible = false;
        }
        private void kitapEmanetIslemBttn_Click(object sender, EventArgs e)
        {
            panelKapa();
            emanetIslemPanel.Visible = true;
        }
        private void kitapAraBttn_Click(object sender, EventArgs e)
        {
            panelKapa();
            kitapAraPanel.Visible = true;
        }
        private void cezalılarBttn_Click(object sender, EventArgs e)
        {
            panelKapa();
            cezaPanel.Visible = true;
        }
        private void ogrKayitBttn_Click(object sender, EventArgs e)
        {
            panelKapa();
            ogrenciKayitPanel.Visible = true;
        }
        private void kitapEkleBttn_Click_1(object sender, EventArgs e)
        {
            panelKapa();
            kitapEklePanel.Visible = true;
        }
        private void cikisBttn_Click(object sender, EventArgs e)
        {
            girisEkrani girisEkrani = new girisEkrani();
            girisEkrani.Show();
            this.Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            panelKapa();
            kitapAraPanel.Visible = true;
        }
        private void button9_Click(object sender, EventArgs e)
        {
            panelKapa();
            panel1.Visible = true;
        }
        #endregion

        #region OGRENCI
        private void kayıtBttn_Click(object sender, EventArgs e)
        {
            cmd = new MySqlCommand();
            conn.Open();
            cmd.CommandText = $"SELECT * FROM ogrenci WHERE OkulNumarasi= {OkulNo.Text}";
            cmd = new MySqlCommand(cmd.CommandText, conn);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                MessageBox.Show("Numarasını girdiğiniz öğrencinin kaydı bulunmaktadır.");
            }
            else
            {
                conn.Close();
                islem.Ekle(new OgrenciBilgi
                {
                    OgrNo = OkulNo.Text,
                    OgrSifre = OgrenciSifre.Text,
                    OgrAd = adTbox.Text,
                    OgrSoyad = sAdTbox.Text,
                    Fakulte = fakulteTbox.Text,
                    Bolum = bAdTbox.Text,
                    Mail = mailTbox.Text
                },"ogrenci");               
                dr.Close();
            }
            conn.Close();
        }
        private void btnSil_Click(object sender, EventArgs e)
        {
            int selected = int.Parse(dataGridView5.CurrentRow.Cells[0].Value.ToString());
            int i;
            i = dataGridView5.SelectedCells[0].RowIndex;
            if (dataGridView5.Rows.Count >= 1 && i != dataGridView5.Rows.Count - 1)
            {
                islem.Sil(selected, "ogrenci");
                MessageBox.Show("Silme işlemi tamamlandı.");
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            cmd = new MySqlCommand();
            conn.Open();
            cmd.CommandText = "SELECT ID, OkulNumarasi, Ad, Soyad, Fakulte, Bolum, OkulMail, OgrenciSifre FROM ogrenci";
            cmd = new MySqlCommand(cmd.CommandText, conn);
            MySqlDataAdapter ada = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ada.Fill(ds);        
            dataGridView5.DataSource = ds.Tables[0].DefaultView;
            dataGridView5.Columns[0].Visible = false;
            dataGridView5.Columns[7].Visible = false;
            conn.Close();
        }
        private void btnDuzenle_Click(object sender, EventArgs e)
        {
            islem.Guncelle(new OgrenciBilgi
            {
                Id = dataGridView5.CurrentRow.Cells["ID"].Value.ToString(),
                OgrNo = ogrOkulNoTbox.Text,
                OgrSifre = OgrenciSifre.Text,
                OgrAd = adTbox.Text,
                OgrSoyad = sAdTbox.Text,
                Fakulte = fakulteTbox.Text,
                Bolum = bAdTbox.Text,
                Mail = mailTbox.Text

            }, "ogrenci");       
        }
        private void dataGridView5_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView5.Rows[e.RowIndex];
                OkulNo.Text = row.Cells[1].Value.ToString();
                adTbox.Text = row.Cells[2].Value.ToString();
                sAdTbox.Text = row.Cells[3].Value.ToString();      
                mailTbox.Text = row.Cells[6].Value.ToString();            
                fakulteTbox.Text = row.Cells[4].Value.ToString();
                bAdTbox.Text = row.Cells[5].Value.ToString();
                OgrenciSifre.Text = row.Cells[7].Value.ToString();
            }
        }
        #endregion

        #region KİTAP
        private void kitapKaydetBttn_Click(object sender, EventArgs e)
        {
            if (sayfaTbox.Text == "")
            {
                sayfaTbox.Text = "0";
            }
            islem.Ekle(new KitapBilgi
            {
                KFakulte = kitapFakulte.Text.ToUpper(),
                KitapAd = kAdTbox.Text.ToUpper(),
                YazarAd = kYazarTbox.Text.ToUpper(),
                ISBN = isbnTbox.Text,
                YayinYili = yayınDateTime.Value,
                Cevirmen = cevirmenTbox.Text.ToUpper(),
                Sayfa = int.Parse(sayfaTbox.Text),
                GelisTarih = gelisDateTime.Value,
                Kategori = kategoriTbox.Text.ToUpper(),
                Ozet = ozetTbox.Text.ToUpper()
            }, "kitap");
        }
        private void btnksil_Click_1(object sender, EventArgs e)
        {
            conn.Open();
            int selected = int.Parse(dataGridView7.CurrentRow.Cells[0].Value.ToString());
           
            cmd.CommandText = $"SELECT * FROM emanet_islem WHERE KitapID= {selected} ORDER BY TeslimEdildigiTarih = NULL ";
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                MessageBox.Show("Kitap emanette olduğu için silemezsiniz!");
                dr.Close();
                conn.Close();
            }
            else
            {
                    conn.Close();
                    dr.Close();
                    islem.Sil(selected, "kitap");
                    MessageBox.Show("Silme işlemi tamamlandı.");
            }
        }
        private void btnkduzen_Click(object sender, EventArgs e)
        {
            if (sayfaTbox.Text == "")
            {
                sayfaTbox.Text = "0";
            }
            islem.Guncelle(new KitapBilgi
            {
                Id = dataGridView7.CurrentRow.Cells["ID"].Value.ToString(),
                KFakulte = kitapFakulte.Text.ToUpper(),
                KitapAd = kAdTbox.Text.ToUpper(),
                YazarAd = kYazarTbox.Text.ToUpper(),
                ISBN = isbnTbox.Text,
                YayinYili = yayınDateTime.Value,
                Cevirmen = cevirmenTbox.Text.ToUpper(),
                Sayfa = int.Parse(sayfaTbox.Text),
                GelisTarih = gelisDateTime.Value,
                Kategori = kategoriTbox.Text.ToUpper(),
                Ozet = ozetTbox.Text.ToUpper()
            }, "kitap") ;
        }
        private void kitapGosterBttn_Click(object sender, EventArgs e)
        {   
            cmd = new MySqlCommand();
            conn.Open();
            cmd = new MySqlCommand(cmd.CommandText, conn);
            cmd.CommandText = "SELECT * FROM kitap";
            MySqlDataAdapter ada = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ada.Fill(ds);
            dataGridView7.DataSource = ds.Tables[0].DefaultView;
            conn.Close();
        }
        private void dataGridView7_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView7.Rows[e.RowIndex];     
                fakulteTbox.Text = row.Cells[1].Value.ToString();
                kAdTbox.Text = row.Cells[2].Value.ToString();
                kYazarTbox.Text = row.Cells[3].Value.ToString();
                isbnTbox.Text = row.Cells[4].Value.ToString();
                cevirmenTbox.Text = row.Cells[6].Value.ToString();
                sayfaTbox.Text = row.Cells[7].Value.ToString();
                kategoriTbox.Text = row.Cells[9].Value.ToString();
                ozetTbox.Text = row.Cells[10].Value.ToString();
            }
        }
        #endregion

        #region EMANET ISLEM
        private void btnksil_Click(object sender, EventArgs e)
        {
            MySqlConnection mConnection = new MySqlConnection();
            cmd = new MySqlCommand();
            conn.Open();
            cmd = conn.CreateCommand();

            int selectedid = int.Parse(dataGridView5.CurrentRow.Cells[0].Value.ToString());

            cmd.CommandText = $"SELECT * FROM emanet_islem WHERE KitapID= {selectedid} ORDER BY TeslimEdildigiTarih < {DateTime.Now}"; 
            cmd.CommandText = $"SELECT * FROM emanet_islem WHERE KitapID= {selectedid} ORDER BY TeslimEdildigiTarih = NULL"; 
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                conn.Close();
                islem.Sil(selectedid,"kitap");
                dr.Close();
            }
            else
            {
                MessageBox.Show("Kitap emanette olduğu için silemezsiniz!");
                dr.Close();
                conn.Close();
            }  
        }
        private void emanetEtBttn_Click(object sender, EventArgs e)
        {
            cmd = new MySqlCommand();
            conn.Open();
            cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM emanet_islem WHERE TeslimEdildigiTarih";
            dr = cmd.ExecuteReader();
            dr.Read();
            DateTime dt = dr.GetDateTime("TeslimEdildigiTarih");
            dr.Close();
            if (DateTime.Now < dt)
            {
                MessageBox.Show("Kitap başka birisine emanet edilmiş.");
            }
            else
            {
                string selected1 = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                string selected2 = dataGridView2.CurrentRow.Cells[0].Value.ToString();
                DateTime date = DateTime.Parse(dateTimePicker1.Value.ToString("dd.MM.yyyy"));
                islem.Ekle(new EmanetEkle
                {
                    OgrenciID = selected1,
                    KitapID = selected2,    
                    TeslimTarihi = date
                }, "emanet_islem");
                MessageBox.Show("Emanet işlemi başarılı!");
            }
            conn.Close();
        }
        private void emanetTabloBttn_Click(object sender, EventArgs e)
        {
            cmd = new MySqlCommand();
            conn.Open();
            cmd = new MySqlCommand(cmd.CommandText, conn);
            cmd.CommandText = "SELECT ogr.ID, ogr.OkulNumarasi, ogr.Ad, ogr.Soyad, ogr.Bolum, kit.ID, kit.KitapAdi, kit.YazarAdi, emt.EmanetTarih, emt.TeslimEdildigiTarih, emt.ID " +
                "FROM ogrenci as ogr " +
                "LEFT JOIN emanet_islem as emt ON ogr.ID = emt.OgrenciID " +
                "LEFT JOIN kitap as kit on kit.ID = emt.KitapID";

            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                dr.Close();
                MySqlDataAdapter ada = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ada.Fill(ds);
                dataGridView3.DataSource = ds.Tables[0].DefaultView;
                dataGridView3.Columns[0].Visible = false;
                dataGridView3.Columns[5].Visible = false;
                dataGridView3.Columns[10].Visible = false;
            }
            conn.Close();
        }
        private void teslimBttn_Click(object sender, EventArgs e)
        {
            MySqlConnection mConnection = new MySqlConnection();
            cmd = new MySqlCommand();
            conn.Open();
            cmd = conn.CreateCommand();

            string selectedid2 = dataGridView3.CurrentRow.Cells[0].Value.ToString();
            string selectedid3 = dataGridView3.CurrentRow.Cells[5].Value.ToString();

            cmd.CommandText = $"SELECT * FROM emanet_islem WHERE KitapID= {selectedid3} ORDER BY TeslimTarih";
            
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                if (dr.GetDateTime("TeslimTarih") < DateTime.Now)
                {
                    islem.Ekle(new Cezalilar
                    {
                        OgrenciID = selectedid2
                    }, "cezalilar");
                    MessageBox.Show("Öğrenci kitabı geç teslim ettiği için ceza almıştır.");
                }
                dr.Close();
                islem.Guncelle(new EmanetAl
                {
                    Id = dataGridView3.CurrentRow.Cells[10].Value.ToString(),
                    OgrenciID = selectedid2,
                    KitapID = selectedid3,
                    TeslimEdildigiTarih = DateTime.Parse(DateTime.Now.ToString("dd.MM.yyyy"))
                }, "emanet_islem") ;
            }
            conn.Close();
        }
        private void emanetAraBttn_Click(object sender, EventArgs e)
        {
            cmd = new MySqlCommand();
            conn.Open();
            cmd = new MySqlCommand(cmd.CommandText, conn);
            cmd.CommandText = $"SELECT ogr.OgrenciID, ogr.OkulNumarasi, ogr.Ad, ogr.Soyad, ogr.Bolum, kit.KitapID, kit.KitapAdi, kit.YazarAdi, emt.EmanetTarih, emt.TeslimEdildigiTarih FROM ogrenci AS ogr LEFT JOIN emanet_islem AS emt ON ogr.OgrenciID = emt.OgrenciID LEFT JOIN kitap AS kit ON kit.KitapID = emt.KitapID WHERE kit.KitapAdi LIKE '%{emanetİslemAraTxt.Text.ToUpper()}%' OR ogr.OkulNumarasi LIKE '%{emanetİslemAraTxt.Text}%'";
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                dr.Close();
                MySqlDataAdapter ada = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ada.Fill(ds);
                dataGridView3.DataSource = ds.Tables[0].DefaultView;
                dataGridView3.Columns["OgrenciID"].Visible = false;
                dataGridView3.Columns["KitapID"].Visible = false;
            }
            else
            {
                MessageBox.Show("Kayıt bulunamadı!");
            }
            conn.Close();
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            cmd = new MySqlCommand();
            conn.Open();

            cmd.CommandText = $"SELECT * FROM ogrenci WHERE OkulNumarasi LIKE '%{ogrOkulNoTbox.Text}%'";
            cmd = new MySqlCommand(cmd.CommandText, conn);
            dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                ogrOkulNoTbox.Text = dr.GetString("OkulNumarasi");
                dr.Close();
                MySqlDataAdapter ada = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ada.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0].DefaultView;
            }
            else
            {
                MessageBox.Show("Öğrenci bulunamadı!");
            }
            conn.Close();
        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            cmd = new MySqlCommand();
            conn.Open();
            cmd.CommandText = $"SELECT * FROM kitap WHERE (KitapAdi) LIKE '%{kEmanetTbox.Text.ToUpper()}%'";
            cmd = new MySqlCommand(cmd.CommandText, conn);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                kEmanetTbox.Text = dr.GetString("KitapAdi");
                dr.Close();
                MySqlDataAdapter ada = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ada.Fill(ds);
                dataGridView2.DataSource = ds.Tables[0].DefaultView;
            }
            else
            {
                MessageBox.Show("Kayıtlı kitap bulunamadı!");
            }
            conn.Close();
        }
        private void ogrGoster_Click(object sender, EventArgs e)
        {
            cmd = new MySqlCommand();
            conn.Open();
            cmd.CommandText = "SELECT * FROM ogrenci";
            cmd = new MySqlCommand(cmd.CommandText, conn);
            MySqlDataAdapter ada = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ada.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0].DefaultView;
            conn.Close();
        }
        private void kitapGoster_Click(object sender, EventArgs e)
        {
            cmd = new MySqlCommand();
            conn.Open();
            cmd.CommandText = "SELECT * FROM kitap";
            cmd = new MySqlCommand(cmd.CommandText, conn);
            MySqlDataAdapter ada = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ada.Fill(ds);
            dataGridView2.DataSource = ds.Tables[0].DefaultView;
            conn.Close();
        }
        #endregion

        #region CEZALILAR
        private void button4_Click(object sender, EventArgs e)
        {
            cmd = new MySqlCommand();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT OkulNumarasi, Ad, Soyad, Fakulte, Bolum FROM ogrenci INNER JOIN cezalilar ON ogrenci.ID = cezalilar.OgrenciID";
            dr = cmd.ExecuteReader();
            dr.Read();
            MySqlDataAdapter ada = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            dr.Close();
            ada.Fill(ds);
            dataGridView4.DataSource = ds.Tables[0].DefaultView;
            conn.Close();
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            cmd = new MySqlCommand();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = $"SELECT ogrenci.OkulNumarasi, ogrenci.Ad, ogrenci.Soyad, ogrenci.Fakulte, ogrenci.Bolum, ogrenci.OkulMail FROM ogrenci INNER JOIN cezalilar on ogrenci.ID = cezalilar.OgrenciID WHERE ogrenci.OkulNumarasi LIKE '%{textBox7.Text}%'";
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                MySqlDataAdapter ada = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                dr.Close();
                ada.Fill(ds);
                dataGridView4.DataSource = ds.Tables[0].DefaultView;
            }
            else
            {
                MessageBox.Show("Öğrenci bulunamadı.");
            }
            conn.Close();
        }
        #endregion

        #region PERSONEL KAYIT
        private void button10_Click(object sender, EventArgs e)
        {
            var personel = new PersonelBilgi
            {
                AdSoyad = textBox5.Text,
                KullaniciAdi = textBox4.Text,
                Sifre = textBox3.Text
            };
            islem.Ekle(personel, "personel");

            MessageBox.Show("Kayıt başarılı!");
        }
        private void button11_Click(object sender, EventArgs e)
        {
            var personel = new PersonelBilgi
            {
                AdSoyad = textBox5.Text,
                KullaniciAdi = textBox4.Text,
                Sifre = textBox3.Text
            };
            islem.Ekle(personel, "ogrenciIsleri");

            MessageBox.Show("Kayıt başarılı!");
        }
        #endregion

        #region KITAP ARA
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            MySqlConnection mConnection = new MySqlConnection();
            cmd = new MySqlCommand();
            conn.Open();
            cmd = conn.CreateCommand();

            cmd.CommandText = $"SELECT * FROM kitap WHERE KitapAdi LIKE '%{araTbox.Text.ToUpper()}%' OR Kategori LIKE '%{araTbox.Text.ToUpper()}%' OR YazarAdi LIKE '%{araTbox.Text.ToUpper()}%'";
            MySqlDataAdapter ada = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ada.Fill(ds);
            dataGridView6.DataSource = ds.Tables[0].DefaultView;
            conn.Close();
        }
        private void button12_Click(object sender, EventArgs e)
        {
            cmd = new MySqlCommand();
            conn.Open();
            cmd.CommandText = "SELECT * FROM kitap";
            cmd = new MySqlCommand(cmd.CommandText, conn);
            MySqlDataAdapter ada = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ada.Fill(ds);
            dataGridView6.DataSource = ds.Tables[0].DefaultView;
            conn.Close();
        }
        #endregion
    }
}