using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KutuphaneOtomasyonu
{
    public partial class ogrenciAnasayfa : Form
    {
        MySqlDataReader dr;
        MySqlConnection conn;
        MySqlCommand cmd;
        private Ogrenci Ogr { get; set; }

        public ogrenciAnasayfa(Ogrenci ogr)
        {
            this.Ogr = ogr;
            InitializeComponent();
            conn = new MySqlConnection("server = 172.21.54.3; uid = nistu; password = Okt3571141.; database = nistu");           
            panelKapaOgr();
        }
        private void panelKapaOgr()
        {
            kitapAramaPanel.Visible = false;
        }
        private void emanetTakipBttn_Click(object sender, EventArgs e)
        {
            
            //var db = new SqlOperations();
            //db.Sil(new BaseEntity(), "ogrenci");
            MySqlConnection mConnection = new MySqlConnection();
            cmd = new MySqlCommand();
            conn.Open();
            cmd = conn.CreateCommand();
            cmd.CommandText = $"SELECT * FROM emanet_islem WHERE OgrenciID={Ogr.OgrenciID}";
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                MessageBox.Show("Emanet kitap bilginiz bulunmaktadır.");
                var ogr = new Ogrenci { OgrenciID = dr.GetInt16("OgrenciID") };
                emanetSureTakip etakio = new emanetSureTakip(Ogr);
                etakio.Show();
            }
            else
            {          
                MessageBox.Show("Emanet kitap bilginiz bulunmamaktadır.");
            }
            conn.Close();
        }
        private void alınanKitapBttn_Click(object sender, EventArgs e)
        {
        }
        private void ogrenciKitapAraBttn_Click(object sender, EventArgs e)
        {
            panelKapaOgr();
            kitapAramaPanel.Visible = true;
        }
        private void cikisBttn_Click(object sender, EventArgs e)
        {
            this.Close();
            girisEkrani girisEkrani = new girisEkrani();
            girisEkrani.Show();
        }
        private void cezaKbttn_Click(object sender, EventArgs e)
        {
            MySqlConnection mConnection = new MySqlConnection();
            cmd = new MySqlCommand();
            conn.Open();
            var ogrno = Ogr.OgrNu;
            cmd = conn.CreateCommand();
            cmd.CommandText = $"SELECT * FROM cezalilar WHERE OgrenciID={Ogr.OgrenciID}";
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                MessageBox.Show("Cezanız bulunmaktadır!");
            }
            else
            {
                MessageBox.Show("Cezanız bulunmamaktadır!");
            }
            conn.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            cmd = new MySqlCommand();
            conn.Open();
            cmd.CommandText = "SELECT * FROM kitap";
            cmd = new MySqlCommand(cmd.CommandText, conn);
            MySqlDataAdapter ada = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            ada.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0].DefaultView;
            conn.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            cmd = new MySqlCommand();
            conn.Open();
            string kitapAraAd = araTboxOgr.Text.ToUpper();            
            cmd = new MySqlCommand(cmd.CommandText, conn);
            cmd.CommandText = $"SELECT * FROM kitap WHERE KitapAdi LIKE '%{kitapAraAd}%' OR Kategori LIKE '%{kitapAraAd}%' OR YazarAdi LIKE '%{kitapAraAd}%'";
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                dr.Close();
                MySqlDataAdapter ada = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ada.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0].DefaultView;
            }
            else
            {
                MessageBox.Show("Kitap bulunamadı!");
            }
            conn.Close();
        }
    }
}
