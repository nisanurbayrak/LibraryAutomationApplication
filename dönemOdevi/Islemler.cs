using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KutuphaneOtomasyonu
{
    public class Islemler : Attribute
    {
        public string AttributeName { get; set; }
        public Islemler(string AttributeName)
        {
            this.AttributeName = AttributeName;
        } 
        public Islemler() { }
    }
    public class BaseClass {
        [Islemler("ID")]
        public string Id { get; set; }
    }
    public class OgrenciBilgi : BaseClass
    {

        [Islemler("OkulNumarasi")]
        public string OgrNo { get; set; }
        [Islemler("OgrenciSifre")]
        public string OgrSifre { get; set; }
        [Islemler("Ad")]
        public string OgrAd { get; set; }
        [Islemler("Soyad")]
        public string OgrSoyad { get; set; }
        [Islemler("Fakulte")]
        public string Fakulte { get; set; }
        [Islemler("Bolum")]
        public string Bolum { get; set; }
        [Islemler("OkulMail")]
        public string Mail { get; set; }
    }
    public class KitapBilgi : BaseClass
    {
        [Islemler("Fakulte")]
        public string KFakulte { get; set; }
        [Islemler("KitapAdi")]
        public string KitapAd { get; set; }
        [Islemler("YazarAdi")]
        public string YazarAd { get; set; }
        [Islemler("ISBNID")]
        public string ISBN { get; set; }
        [Islemler("YayinYili")]
        public DateTime YayinYili { get; set; }
        [Islemler("Cevirmen")]
        public string Cevirmen { get; set; }
        [Islemler("Sayfa")]
        public int Sayfa { get; set; }
        [Islemler("GelisTarih")]
        public DateTime GelisTarih { get; set; }
        [Islemler("Kategori")]
        public string Kategori { get; set; }
        [Islemler("Ozet")]
        public string Ozet { get; set; }
    }
    public class PersonelBilgi : BaseClass
    {
        [Islemler("AdSoyad")]
        public string AdSoyad { get; set; }
        [Islemler("KullaniciAdi")]
        public string KullaniciAdi { get; set; }
        [Islemler("Sifre")]
        public string Sifre { get; set; }
    }
    public class EmanetEkle: BaseClass
    {
        [Islemler("OgrenciID")]
        public string OgrenciID { get; set; }
        [Islemler("KitapID")]
        public string KitapID { get; set; }
        [Islemler("TeslimTarih")]
        public DateTime TeslimTarihi { get; set; }
    }
    public class EmanetAl : BaseClass
    {
        [Islemler("OgrenciID")]
        public string OgrenciID { get; set; }
        [Islemler("KitapID")]
        public string KitapID { get; set; }
        [Islemler("TeslimEdildigiTarih")]
        public DateTime TeslimEdildigiTarih { get; set; }
    }
    public class Cezalilar: BaseClass
    {
        [Islemler("OgrenciID")]
        public string OgrenciID { get; set; }
    }

    public class Islem
    {
        private MySqlCommand cmd;
        private MySqlConnection conn;
        public Islem()
        {
            conn = new MySqlConnection("server = 172.21.54.3; uid = nistu; password = Okt3571141.; database = nistu");
            cmd = conn.CreateCommand();
        }
        public void Ekle<T>(T Obj, string tabloAdi) where T : BaseClass
        {
            conn.Open();

            var qheaders = new List<String>();
            var qvalues = new List<String>();

            foreach (var i in Obj.GetType().GetProperties())
            {
                qheaders.Add($"{i.GetCustomAttribute<Islemler>()?.AttributeName}");
                qvalues.Add($"@{i.GetCustomAttribute<Islemler>()?.AttributeName}");

                cmd.Parameters.AddWithValue(i.GetCustomAttribute<Islemler>().AttributeName, i.GetValue(Obj));
            }
            cmd.CommandText = $"INSERT INTO {tabloAdi} ({string.Join(",", qheaders)}) VALUES ({string.Join(",", qvalues)})";
            cmd.ExecuteNonQuery();

            conn.Close();
        }
        public void Guncelle<T>(T Obj, string tabloAdi) where T : BaseClass
        {
            conn.Open();

            var q = new List<String>();
            foreach (var i in Obj.GetType().GetProperties())
            {
                q.Add($"{i.GetCustomAttribute<Islemler>()?.AttributeName} = @{ i.GetCustomAttribute<Islemler>()?.AttributeName}");
                cmd.Parameters.AddWithValue(i.GetCustomAttribute<Islemler>().AttributeName, i.GetValue(Obj));
            }
            cmd.CommandText = $"UPDATE {tabloAdi} SET {string.Join(",", q)} WHERE ID = {Obj.Id}";
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public void Sil(int id, string TabloAdi)
        {
            conn.Open();
            cmd.CommandText = $"DELETE FROM {TabloAdi} WHERE ID = {id}";
            cmd.ExecuteNonQuery();
            conn.Close();
        }       
    }
}