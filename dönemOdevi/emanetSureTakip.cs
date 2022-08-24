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
    public partial class emanetSureTakip : Form
    {
        MySqlDataReader dr;
        MySqlConnection conn;
        MySqlCommand cmd;
        private Ogrenci Ogr { get; set; }
        public emanetSureTakip(Ogrenci ogr)
        {
            this.Ogr = ogr;
            conn = new MySqlConnection("server = 172.21.54.3; uid = nistu; password = Okt3571141.; database = nistu");
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cmd = new MySqlCommand();
            conn.Open();
            cmd.CommandText = $"SELECT * FROM emanet_islem WHERE OgrenciID = {Ogr.OgrenciID}";
            cmd = new MySqlCommand(cmd.CommandText, conn);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                cmd.CommandText = $"SELECT kit.KitapAdi, kit.YazarAdi, emt.EmanetTarih, emt.TeslimTarih FROM kitap as kit LEFT JOIN emanet_islem as emt ON kit.KitapID = emt.KitapID WHERE OgrenciID = {Ogr.OgrenciID}";
                dr.Close();
                MySqlDataAdapter ada = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                ada.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0].DefaultView;
            }          
        }
    }
}
