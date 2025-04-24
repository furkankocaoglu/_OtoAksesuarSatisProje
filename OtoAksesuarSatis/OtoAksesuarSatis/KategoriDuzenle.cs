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
    public partial class KategoriDuzenle : Form
    {
        private int kategoriID;
        public KategoriDuzenle(int kategoriID)
        {
            InitializeComponent();
            this.kategoriID = kategoriID;
            kategoriYukle();
        }
        private void kategoriYukle()
        {
            using (SqlConnection conn = new SqlConnection(Baglanti.baglantiYolu))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Kategoriler WHERE KategoriID = @KategoriID", conn);
                cmd.Parameters.AddWithValue("@KategoriID", this.kategoriID);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {

                    textBox1.Text = reader["KategoriAdi"].ToString();
                }
                conn.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string yeniKategoriAdi = textBox1.Text;


            using (SqlConnection conn = new SqlConnection(Baglanti.baglantiYolu))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Kategoriler SET KategoriAdi = @KategoriAdi WHERE KategoriID = @KategoriID", conn);
                cmd.Parameters.AddWithValue("@KategoriAdi", yeniKategoriAdi);
                cmd.Parameters.AddWithValue("@KategoriID", this.kategoriID);

                cmd.ExecuteNonQuery();
                conn.Close();
            }

            MessageBox.Show("Kategori başarıyla güncellendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
