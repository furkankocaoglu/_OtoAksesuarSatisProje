﻿using OtoAksesuarSatis.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.IO;

namespace OtoAksesuarSatis
{
    public partial class UrunDuzenle : Form
    {
        private int urunID;
        public UrunDuzenle(int urunID)
        {
            InitializeComponent();
            this.urunID = urunID;

        }

        private void UrunDuzenle_Load(object sender, EventArgs e)
        {

            kategoriYukle();
            markaYukle();
            urunDetayi();

        }
        private void kategoriYukle()
        {
            using (SqlConnection conn = new SqlConnection(Baglanti.baglantiYolu))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT KategoriID, KategoriAdi FROM Kategoriler WHERE Durum = 1", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                var categories = new List<dynamic>();
                while (reader.Read())
                {
                    categories.Add(new { Text = reader["KategoriAdi"].ToString(), Value = reader["KategoriID"] });
                }

                comboBox2.DataSource = categories;
                comboBox2.DisplayMember = "Text";
                comboBox2.ValueMember = "Value";
            }
        }
        private void markaYukle()
        {
            using (SqlConnection conn = new SqlConnection(Baglanti.baglantiYolu))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT MarkaID, MarkaAdi FROM Markalar WHERE Durum = 1", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                var brands = new List<dynamic>();
                while (reader.Read())
                {
                    brands.Add(new { Text = reader["MarkaAdi"].ToString(), Value = reader["MarkaID"] });
                }

                comboBox1.DataSource = brands;
                comboBox1.DisplayMember = "Text";
                comboBox1.ValueMember = "Value";
            }
        }
        private void urunDetayi()
        {
            using (SqlConnection conn = new SqlConnection(Baglanti.baglantiYolu))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Urunler WHERE UrunID = @UrunID", conn);
                cmd.Parameters.AddWithValue("@UrunID", urunID);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    
                    textBox1.Text = reader["UrunAdi"].ToString();
                    textBox4.Text = reader["BronzFiyat"].ToString();
                    textBox5.Text = reader["SilverFiyat"].ToString();
                    textBox6.Text = reader["GoldFiyat"].ToString();
                    textBox2.Text = reader["StokMiktari"].ToString();
                    textBox3.Text = reader["Aciklama"].ToString();

                   
                    string resimYolu = reader["ResimYolu"].ToString();
                    if (!string.IsNullOrEmpty(resimYolu))
                    {
                        pictureBox1.ImageLocation = resimYolu; 
                    }
                    else
                    {
                        pictureBox1.Image = null; 
                    }

                    
                    comboBox2.SelectedValue = reader["KategoriID"];
                    comboBox1.SelectedValue = reader["MarkaID"];
                }
            }
        }
        private string mevcutResimYolu = "";
        private void button1_Click(object sender, EventArgs e)
        {
            string urunAdi = textBox1.Text;
            int kategoriID = (int)comboBox2.SelectedValue;
            int markaID = (int)comboBox1.SelectedValue;
            decimal bronzFiyat = Convert.ToDecimal(textBox4.Text);
            decimal silverFiyat = Convert.ToDecimal(textBox5.Text);
            decimal goldFiyat = Convert.ToDecimal(textBox6.Text);
            int stokMiktari = Convert.ToInt32(textBox2.Text);
            string aciklama = textBox3.Text;
            string yeniResimYolu = pictureBox1.ImageLocation;
            string kullanilacakResimYolu = !string.IsNullOrEmpty(yeniResimYolu)
                ? yeniResimYolu
                : (!string.IsNullOrEmpty(mevcutResimYolu) ? mevcutResimYolu : "");

            
            using (SqlConnection conn = new SqlConnection(Baglanti.baglantiYolu))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(@"
UPDATE Urunler
SET UrunAdi = @UrunAdi,
    MarkaID = @MarkaID,
    KategoriID = @KategoriID,
    BronzFiyat = @BronzFiyat,
    SilverFiyat = @SilverFiyat,
    GoldFiyat = @GoldFiyat,
    StokMiktari = @StokMiktari,
    Aciklama = @Aciklama,
    ResimYolu = @ResimYolu
WHERE UrunID = @UrunID", conn);

                cmd.Parameters.AddWithValue("@UrunAdi", urunAdi);
                cmd.Parameters.AddWithValue("@MarkaID", markaID);
                cmd.Parameters.AddWithValue("@KategoriID", kategoriID);
                cmd.Parameters.AddWithValue("@BronzFiyat", bronzFiyat);
                cmd.Parameters.AddWithValue("@SilverFiyat", silverFiyat);
                cmd.Parameters.AddWithValue("@GoldFiyat", goldFiyat);
                cmd.Parameters.AddWithValue("@StokMiktari", stokMiktari);
                cmd.Parameters.AddWithValue("@Aciklama", aciklama);
                cmd.Parameters.AddWithValue("@ResimYolu", kullanilacakResimYolu);
                cmd.Parameters.AddWithValue("@UrunID", urunID);

                cmd.ExecuteNonQuery();
                conn.Close();
            }

           
            string xmlYolu = @"C:\BayilikXML\Urunler.xml";
            XDocument doc = File.Exists(xmlYolu)
                ? XDocument.Load(xmlYolu)
                : new XDocument(new XElement("urunler"));

            
            var mevcutUrun = doc.Root.Elements("urun")
                .FirstOrDefault(x => x.Element("UrunID")?.Value == urunID.ToString());

            if (mevcutUrun != null)
            {
                mevcutUrun.SetElementValue("UrunAdi", urunAdi);
                mevcutUrun.SetElementValue("Kategori", comboBox2.Text);
                mevcutUrun.SetElementValue("Marka", comboBox1.Text);
                mevcutUrun.SetElementValue("BronzFiyat", bronzFiyat.ToString("F2", CultureInfo.InvariantCulture));
                mevcutUrun.SetElementValue("SilverFiyat", silverFiyat.ToString("F2", CultureInfo.InvariantCulture));
                mevcutUrun.SetElementValue("GoldFiyat", goldFiyat.ToString("F2", CultureInfo.InvariantCulture));
                mevcutUrun.SetElementValue("Stok", stokMiktari.ToString());
                mevcutUrun.SetElementValue("Aciklama", aciklama);
                mevcutUrun.SetElementValue("Resim", kullanilacakResimYolu);
                mevcutUrun.SetElementValue("GuncellenmeZamani", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            else
            {
              
                XElement yeniUrun = new XElement("urun",
                    new XElement("UrunID", urunID),
                    new XElement("UrunAdi", urunAdi),
                    new XElement("Kategori", comboBox2.Text),
                    new XElement("Marka", comboBox1.Text),
                    new XElement("BronzFiyat", bronzFiyat.ToString("F2", CultureInfo.InvariantCulture)),
                    new XElement("SilverFiyat", silverFiyat.ToString("F2", CultureInfo.InvariantCulture)),
                    new XElement("GoldFiyat", goldFiyat.ToString("F2", CultureInfo.InvariantCulture)),
                    new XElement("Stok", stokMiktari.ToString()),
                    new XElement("Aciklama", aciklama),
                    new XElement("Resim", kullanilacakResimYolu),
                    new XElement("EklenmeZamani", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                );
                doc.Root.Add(yeniUrun);
            }

            
            doc.Save(xmlYolu);

            MessageBox.Show("Ürün başarıyla güncellendi ve XML dosyasına yansıdı!");
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.ImageLocation = openFileDialog1.FileName;
            }
        }
    }

}

