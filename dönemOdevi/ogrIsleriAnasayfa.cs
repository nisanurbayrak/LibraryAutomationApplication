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
    public partial class ogrIsleriAnasayfa : Form
    {
        MySqlConnection conn;
        MySqlCommand cmd;
        MySqlDataReader dr;
        public ogrIsleriAnasayfa()
        {
            InitializeComponent();
            conn = new MySqlConnection("server = 172.21.54.3; uid = nistu; password = Okt3571141.; database = nistu");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            cmd = new MySqlCommand();
            conn.Open();
            cmd.Connection = conn;

            cmd.CommandText = "SELECT OkulNumarasi, Ad, Soyad, Fakulte, Bolum, OkulMail FROM ogrenci INNER JOIN cezalilar ON ogrenci.ID = cezalilar.OgrenciID";
            dr = cmd.ExecuteReader();
            dr.Read();
            MySqlDataAdapter ada = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            dr.Close();
            ada.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0].DefaultView;
            conn.Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            girisEkrani gekran = new girisEkrani();
            gekran.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            cmd = new MySqlCommand();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = $"SELECT ogrenci.OkulNumarasi, ogrenci.Ad, ogrenci.Soyad, ogrenci.Fakulte, ogrenci.Bolum, ogrenci.OkulMail FROM ogrenci INNER JOIN cezalilar on ogrenci.ID = cezalilar.OgrenciID WHERE ogrenci.OkulNumarasi LIKE '%{textBox1.Text}%'";
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                MySqlDataAdapter ada = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                dr.Close();
                ada.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0].DefaultView;
            }
            else
            {
                MessageBox.Show("Öğrenci bulunamadı.");
            }
            
            conn.Close();
        }
    }
}
