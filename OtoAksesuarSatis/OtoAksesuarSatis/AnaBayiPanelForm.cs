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
            ComboBoxDoldur("SELECT BayiTipiAdi FROM BayiTipleri", comboBox3);
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;
            UrunleriListele();

        }
        private void UrunleriListele()
        {
            Liste.Items.Clear();

            using (SqlConnection conn = new SqlConnection(Baglanti.baglantiYolu))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"
            SELECT u.UrunID, u.UrunAdi, u.Marka, k.KategoriAdi, u.Fiyat 
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
                        Marka = reader["Marka"].ToString(),
                        KategoriAdi = reader["KategoriAdi"].ToString(),
                        Fiyat = Convert.ToDecimal(reader["Fiyat"])
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

            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) || string.IsNullOrWhiteSpace(comboBox1.Text) ||
                string.IsNullOrWhiteSpace(comboBox3.Text))
            {
                MessageBox.Show("Lütfen tüm zorunlu alanları doldurun.");
                return;
            }

            try
            {
                string urunAdi = textBox1.Text;
                string bayiTipiAdi = comboBox3.Text;
                string kategoriAdi = comboBox1.Text;
                string marka = textBox5.Text;
                decimal fiyat = decimal.Parse(textBox2.Text);
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
                    INSERT INTO Urunler (UrunAdi, Marka, KategoriID, Fiyat, StokMiktari, Aciklama, ResimYolu)
                    VALUES (@UrunAdi, @Marka, 
                        (SELECT KategoriID FROM Kategoriler WHERE KategoriAdi = @KategoriAdi), 
                        @Fiyat, @Stok, @Aciklama, @ResimYolu);
                    SELECT SCOPE_IDENTITY();", conn, tran);

                        cmd.Parameters.Add("@UrunAdi", SqlDbType.NVarChar).Value = urunAdi;
                        cmd.Parameters.Add("@Marka", SqlDbType.NVarChar).Value = marka;
                        cmd.Parameters.Add("@KategoriAdi", SqlDbType.NVarChar).Value = kategoriAdi;
                        cmd.Parameters.Add("@Fiyat", SqlDbType.Decimal).Value = fiyat;
                        cmd.Parameters.Add("@Stok", SqlDbType.Int).Value = stok;
                        cmd.Parameters.Add("@Aciklama", SqlDbType.NVarChar).Value = aciklama;
                        cmd.Parameters.Add("@ResimYolu", SqlDbType.NVarChar).Value = gorselYolu;

                        decimal urunIDDecimal = (decimal)cmd.ExecuteScalar();
                        int urunID = Convert.ToInt32(urunIDDecimal);

                        SqlCommand fiyatCmd = new SqlCommand(@"
                    INSERT INTO BayiFiyatlari (UrunID, BayiTipiID, Fiyat)
                    VALUES (@UrunID, 
                        (SELECT BayiTipiID FROM BayiTipleri WHERE BayiTipiAdi = @BayiTipiAdi), 
                        @Fiyat)", conn, tran);

                        fiyatCmd.Parameters.Add("@UrunID", SqlDbType.Int).Value = urunID;
                        fiyatCmd.Parameters.Add("@BayiTipiAdi", SqlDbType.NVarChar).Value = bayiTipiAdi;
                        fiyatCmd.Parameters.Add("@Fiyat", SqlDbType.Decimal).Value = fiyat;

                        fiyatCmd.ExecuteNonQuery();

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
            Dictionary<string, decimal> fiyatFarklari = new Dictionary<string, decimal>
    {
        { "Bronz", -0.10m }, 
        { "Silver", 0.00m },  
        { "Gold", 0.20m }     
    };

            foreach (string bayiTipi in bayiTipleri)
            {
                using (SqlConnection conn = new SqlConnection(Baglanti.baglantiYolu))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(@"
            SELECT 
                u.UrunAdi, 
                u.Marka, 
                k.KategoriAdi, 
                bf.Fiyat, 
                u.StokMiktari, 
                u.Aciklama, 
                u.ResimYolu
            FROM Urunler u
            JOIN Kategoriler k ON u.KategoriID = k.KategoriID
            JOIN BayiFiyatlari bf ON u.UrunID = bf.UrunID
            JOIN BayiTipleri bt ON bf.BayiTipiID = bt.BayiTipiID
            WHERE bt.BayiTipiAdi = @BayiTipi AND u.UrunID = @UrunID", conn);

                    cmd.Parameters.AddWithValue("@BayiTipi", bayiTipi);
                    cmd.Parameters.AddWithValue("@UrunID", seciliUrun.UrunID);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        decimal baseFiyat = Convert.ToDecimal(reader["Fiyat"]);
                        decimal bayiFiyat = baseFiyat * (1 + fiyatFarklari[bayiTipi]); 

                        XElement urunXml = new XElement("urunler",
                            new XElement("urun",
                                new XElement("UrunAdi", reader["UrunAdi"]),
                                new XElement("Marka", reader["Marka"]),
                                new XElement("Kategori", reader["KategoriAdi"]),
                                new XElement("Fiyat", bayiFiyat.ToString("C2")), 
                                new XElement("Stok", reader["StokMiktari"]),
                                new XElement("Aciklama", reader["Aciklama"]),
                                new XElement("Resim", reader["ResimYolu"])
                            )
                        );

                        string dosyaAdi = $"{bayiTipi}_{seciliUrun.UrunAdi}.xml";
                        string xmlYolu = Path.Combine(xmlKlasorYolu, dosyaAdi);
                        urunXml.Save(xmlYolu);
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
