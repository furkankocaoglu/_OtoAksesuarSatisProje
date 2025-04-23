using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OtoAksesuarSatis
{
    public partial class UrunIslemleri : Form
    {
        public UrunIslemleri()
        {
            InitializeComponent();
        }

        private void UrunIslemleri_Load(object sender, EventArgs e)
        {
            UrunleriYukle();
        }
        private void UrunleriYukle()
        {
            using (SqlConnection conn = new SqlConnection(Baglanti.baglantiYolu))
            {
                conn.Open();
                string query = @"
                    SELECT 
                        U.UrunID,
                        U.UrunAdi,
                        K.KategoriAdi,
                        U.StokMiktari,
                        U.EklenmeTarihi,
                        U.AktifMi
                    FROM Urunler U
                    LEFT JOIN Kategoriler K ON U.KategoriID = K.KategoriID
                    WHERE U.Silinmis = 0
                ";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;

                
                dataGridView1.Columns["UrunID"].HeaderText = "ID";
                dataGridView1.Columns["UrunAdi"].HeaderText = "Ürün Adı";
                dataGridView1.Columns["KategoriAdi"].HeaderText = "Kategori";
                dataGridView1.Columns["StokMiktari"].HeaderText = "Stok";
                dataGridView1.Columns["EklenmeTarihi"].HeaderText = "Tarih";
                dataGridView1.Columns["AktifMi"].HeaderText = "Aktif?";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UrunleriYukle();
        }
        
    }

}
