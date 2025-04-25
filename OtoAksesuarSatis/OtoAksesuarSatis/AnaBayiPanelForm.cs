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
            ComboBoxDoldur("SELECT MarkaAdi FROM Markalar WHERE Silinmis=0 AND Durum=1", comboBox2);
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
        SELECT u.UrunID, u.UrunAdi, k.KategoriAdi, u.BronzFiyat, u.SilverFiyat, u.GoldFiyat, u.StokMiktari 
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
                        GoldFiyat = Convert.ToDecimal(reader["GoldFiyat"]),
                        StokMiktari = Convert.ToInt32(reader["StokMiktari"])
                    };

                    ListViewItem item = new ListViewItem(urun.UrunAdi);
                    item.SubItems.Add(urun.KategoriAdi);
                    item.SubItems.Add(urun.BronzFiyat.ToString("C2"));
                    item.SubItems.Add(urun.SilverFiyat.ToString("C2"));
                    item.SubItems.Add(urun.GoldFiyat.ToString("C2"));
                    item.SubItems.Add(urun.StokMiktari.ToString());

                    
                    item.Tag = urun;

                    Liste.Items.Add(item);
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

            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(bronz.Text) ||
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
                decimal temelFiyat = decimal.Parse(bronz.Text);
                decimal bronzFiyat = temelFiyat * 1.05m;
                decimal silverFiyat = temelFiyat * 1.07m;
                decimal goldFiyat = temelFiyat * 1.10m;
                int stok = int.Parse(textBox3.Text);
                string aciklama = textBox4.Text;
                string gorselYolu = pictureBox1.ImageLocation ?? "";

                using (SqlConnection conn = new SqlConnection(Baglanti.baglantiYolu))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(@"
        INSERT INTO Urunler (UrunAdi, MarkaID, KategoriID, BronzFiyat, SilverFiyat, GoldFiyat, StokMiktari, Aciklama, ResimYolu)
        VALUES (@UrunAdi, 
                (SELECT MarkaID FROM Markalar WHERE MarkaAdi = @MarkaAdi),
                (SELECT KategoriID FROM Kategoriler WHERE KategoriAdi = @KategoriAdi),
                @BronzFiyat, @SilverFiyat, @GoldFiyat, @Stok, @Aciklama, @ResimYolu)", conn);

                    cmd.Parameters.AddWithValue("@UrunAdi", urunAdi);
                    cmd.Parameters.AddWithValue("@MarkaAdi", MarkaAdi);
                    cmd.Parameters.AddWithValue("@KategoriAdi", kategoriAdi);
                    cmd.Parameters.AddWithValue("@BronzFiyat", bronzFiyat);
                    cmd.Parameters.AddWithValue("@SilverFiyat", silverFiyat);
                    cmd.Parameters.AddWithValue("@GoldFiyat", goldFiyat);
                    cmd.Parameters.AddWithValue("@Stok", stok);
                    cmd.Parameters.AddWithValue("@Aciklama", aciklama);
                    cmd.Parameters.AddWithValue("@ResimYolu", gorselYolu);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Ürün başarıyla eklendi!");
                    UrunleriListele();
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
            DialogResult result = MessageBox.Show("XML dosyasını oluşturmak istediğinize emin misiniz?",
                                         "Onay",
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                return; 
            }

            ListViewItem selectedItem = (ListViewItem)Liste.SelectedItem;
            Urun seciliUrun = (Urun)selectedItem.Tag;

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
                u.UrunID,
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
                        }
                        else if (bayiTipi == "Silver")
                        {
                            fiyat = Convert.ToDecimal(reader["SilverFiyat"]);
                        }
                        else if (bayiTipi == "Gold")
                        {
                            fiyat = Convert.ToDecimal(reader["GoldFiyat"]);
                        }

                        string dosyaAdi = $"{bayiTipi}.xml";
                        string xmlYolu = Path.Combine(xmlKlasorYolu, dosyaAdi);

                        XDocument doc = File.Exists(xmlYolu)
                            ? XDocument.Load(xmlYolu)
                            : new XDocument(new XElement("urunler"));

                        var mevcutUrun = doc.Root.Elements("urun")
                            .FirstOrDefault(x => x.Element("UrunID")?.Value == seciliUrun.UrunID.ToString());

                        if (mevcutUrun != null)
                        {
                            bool degisiklikVar = false;

                            void Guncelle(string elemanAdi, string yeniDeger)
                            {
                                var eleman = mevcutUrun.Element(elemanAdi);
                                if (eleman == null || eleman.Value != yeniDeger)
                                {
                                    mevcutUrun.SetElementValue(elemanAdi, yeniDeger);
                                    degisiklikVar = true;
                                }
                            }

                            Guncelle("UrunAdi", reader["UrunAdi"].ToString());
                            Guncelle("Kategori", reader["KategoriAdi"].ToString());
                            Guncelle("Fiyat", fiyat.ToString("C2"));
                            Guncelle("Stok", reader["StokMiktari"].ToString());
                            Guncelle("Aciklama", reader["Aciklama"].ToString());
                            Guncelle("Resim", reader["ResimYolu"].ToString());

                            if (degisiklikVar)
                            {
                                mevcutUrun.SetElementValue("GuncellenmeZamani", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                doc.Save(xmlYolu);
                            }
                        }
                        else
                        {
                            XElement yeniUrun = new XElement("urun",
                                new XElement("UrunID", seciliUrun.UrunID),
                                new XElement("UrunAdi", reader["UrunAdi"]),
                                new XElement("Kategori", reader["KategoriAdi"]),
                                new XElement("Fiyat", fiyat.ToString("C2")),
                                new XElement("Stok", reader["StokMiktari"]),
                                new XElement("Aciklama", reader["Aciklama"]),
                                new XElement("Resim", reader["ResimYolu"]),
                                new XElement("EklenmeZamani", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                            );

                            doc.Root.Add(yeniUrun);
                            doc.Save(xmlYolu);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Ürün verisi alınamadı.");
                    }

                    reader.Close();
                }
            }

            MessageBox.Show("Ürün Eklendi. Mevcut özelliklerde ise tekrar eklenmeyecektir.");
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
