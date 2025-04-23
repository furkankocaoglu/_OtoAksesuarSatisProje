using OtoAksesuarSatis.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace OtoAksesuarSatis
{
    public partial class AnaBayiPanelForm : Form
    {

        public AnaBayiPanelForm()
        {
            InitializeComponent();
        }

        private void AnaBayiPanelForm_Load(object sender, EventArgs e)
        {
            ComboBoxDoldur("SELECT KategoriAdi FROM Kategoriler WHERE Silinmis=0 AND Durum=1", comboBox1);
            ComboBoxDoldur("SELECT MarkaAdi FROM Markalar", comboBox2);
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            UrunleriListele();

        }
        private void UrunleriListele()
        {
            Liste.Items.Clear();

            using (SqlConnection conn = new SqlConnection(Baglanti.baglantiYolu))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"
SELECT u.UrunID, u.UrunAdi, k.KategoriAdi, u.BronzFiyat, u.SilverFiyat, u.GoldFiyat 
FROM Urunler u
INNER JOIN Kategoriler k ON u.KategoriID = k.KategoriID
WHERE u.Silinmis = 0", conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Urun urun = new Urun
                    {
                        UrunID = Convert.ToInt32(reader["UrunID"]),
                        UrunAdi = reader["UrunAdi"].ToString(),
                        KategoriAdi = reader["KategoriAdi"].ToString(),
                        BronzFiyat = Convert.ToDecimal(reader["BronzFiyat"]),
                        SilverFiyat = Convert.ToDecimal(reader["SilverFiyat"]),
                        GoldFiyat = Convert.ToDecimal(reader["GoldFiyat"])
                    };

                    Liste.Items.Add(urun);
                }
            }
        }
        private void ComboBoxDoldur(string query, ComboBox cmb)
        {
            cmb.Items.Clear();
            using (SqlConnection conn = new SqlConnection(Baglanti.baglantiYolu))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cmb.Items.Add(reader[0].ToString());
                }
                conn.Close();
            }

            if (cmb.Items.Count > 0)
                cmb.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(gold.Text) ||
        string.IsNullOrWhiteSpace(textBox3.Text) || string.IsNullOrWhiteSpace(comboBox1.Text) ||
        string.IsNullOrWhiteSpace(comboBox2.Text))
            {
                MessageBox.Show("Lütfen tüm zorunlu alanları doldurun.");
                return;
            }

            try
            {
                string urunAdi = textBox1.Text;
                string MarkaAdi = comboBox2.Text;
                string kategoriAdi = comboBox1.Text;
                decimal bronzfiyat = decimal.Parse(bronz.Text);
                decimal silverfiyat = decimal.Parse(silver.Text);
                decimal goldfiyat = decimal.Parse(gold.Text);
                int stok = int.Parse(textBox3.Text);
                string aciklama = textBox4.Text;
                string gorselYolu = pictureBox1.ImageLocation ?? "";

                using (SqlConnection conn = new SqlConnection(Baglanti.baglantiYolu))
                {
                    conn.Open();
                    SqlTransaction tran = conn.BeginTransaction();

                    try
                    {

                        SqlCommand cmd = new SqlCommand(@"
                INSERT INTO Urunler (UrunAdi, MarkaID, KategoriID, BronzFiyat, SilverFiyat, GoldFiyat, StokMiktari, Aciklama, ResimYolu)
                VALUES (@UrunAdi, 
                        (SELECT MarkaID FROM Markalar WHERE MarkaAdi = @MarkaAdi),
                        (SELECT KategoriID FROM Kategoriler WHERE KategoriAdi = @KategoriAdi),
                        @BronzFiyat, @SilverFiyat, @GoldFiyat, @Stok, @Aciklama, @ResimYolu);
                SELECT SCOPE_IDENTITY();", conn, tran);

                        cmd.Parameters.Add("@UrunAdi", SqlDbType.NVarChar).Value = urunAdi;
                        cmd.Parameters.Add("@MarkaAdi", SqlDbType.NVarChar).Value = MarkaAdi;
                        cmd.Parameters.Add("@KategoriAdi", SqlDbType.NVarChar).Value = kategoriAdi;
                        cmd.Parameters.Add("@BronzFiyat", SqlDbType.Decimal).Value = bronzfiyat;
                        cmd.Parameters.Add("@SilverFiyat", SqlDbType.Decimal).Value = silverfiyat;
                        cmd.Parameters.Add("@GoldFiyat", SqlDbType.Decimal).Value = goldfiyat;
                        cmd.Parameters.Add("@Stok", SqlDbType.Int).Value = stok;
                        cmd.Parameters.Add("@Aciklama", SqlDbType.NVarChar).Value = aciklama;
                        cmd.Parameters.Add("@ResimYolu", SqlDbType.NVarChar).Value = gorselYolu;

                        decimal urunIDDecimal = (decimal)cmd.ExecuteScalar();
                        int urunID = Convert.ToInt32(urunIDDecimal);


                        tran.Commit();
                        MessageBox.Show("Ürün başarıyla eklendi!");
                        UrunleriListele();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        MessageBox.Show("Hata oluştu: " + ex.Message);
                    }
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Fiyat veya stok alanları hatalı girildi. Lütfen sayı giriniz.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Beklenmeyen bir hata oluştu: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (Liste.SelectedItem == null)
            {
                MessageBox.Show("Lütfen bir ürün seçin!");
                return;
            }

            Urun seciliUrun = (Urun)Liste.SelectedItem;

            string xmlKlasorYolu = @"C:\BayilikXML\";
            if (!Directory.Exists(xmlKlasorYolu))
                Directory.CreateDirectory(xmlKlasorYolu);

            List<string> bayiTipleri = new List<string> { "Bronz", "Silver", "Gold" };

           
            foreach (string bayiTipi in bayiTipleri)
            {
                using (SqlConnection conn = new SqlConnection(Baglanti.baglantiYolu))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(@"
SELECT 
    u.UrunAdi, 
    k.KategoriAdi, 
    u.BronzFiyat, 
    u.SilverFiyat, 
    u.GoldFiyat, 
    u.StokMiktari, 
    u.Aciklama, 
    u.ResimYolu
FROM Urunler u
JOIN Kategoriler k ON u.KategoriID = k.KategoriID
WHERE u.UrunID = @UrunID", conn);

                    cmd.Parameters.AddWithValue("@UrunID", seciliUrun.UrunID);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        decimal fiyat = 0;

                        
                        if (bayiTipi == "Bronz")
                        {
                            fiyat = Convert.ToDecimal(reader["BronzFiyat"]);
                            MessageBox.Show("Bronz Fiyat: " + fiyat.ToString("C2"));
                        }
                        else if (bayiTipi == "Silver")
                        {
                            fiyat = Convert.ToDecimal(reader["SilverFiyat"]);
                            MessageBox.Show("Silver Fiyat: " + fiyat.ToString("C2"));
                        }
                        else if (bayiTipi == "Gold")
                        {
                            fiyat = Convert.ToDecimal(reader["GoldFiyat"]);
                            MessageBox.Show("Gold Fiyat: " + fiyat.ToString("C2"));
                        }

                       
                        XElement urunXml = new XElement("urunler",
                            new XElement("urun",
                                new XElement("UrunAdi", reader["UrunAdi"]),
                                new XElement("Kategori", reader["KategoriAdi"]),
                                new XElement("Fiyat", fiyat.ToString("C2")),
                                new XElement("Stok", reader["StokMiktari"]),
                                new XElement("Aciklama", reader["Aciklama"]),
                                new XElement("Resim", reader["ResimYolu"])
                            )
                        );

                       
                        string dosyaAdi = $"{bayiTipi}_{seciliUrun.UrunAdi}.xml";
                        string xmlYolu = Path.Combine(xmlKlasorYolu, dosyaAdi);
                        urunXml.Save(xmlYolu);
                    }
                    else
                    {
                        MessageBox.Show("Ürün verisi alınamadı.");
                    }

                    reader.Close();
                }
            }

            MessageBox.Show("XML dosyaları başarıyla oluşturuldu!");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.ImageLocation = openFileDialog1.FileName;
            }
        }
    }

}
