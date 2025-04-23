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
    public partial class AnaBayiGirisEkrani : Form
    {
        public AnaBayiGirisEkrani()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            label3.Visible = true;
            label4.Visible = true;
            textBox1.Visible = true;
            textBox2.Visible = true;
            button1.Visible = true;
            label2.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string kullaniciAdi = textBox1.Text.Trim();
            string sifre = textBox2.Text.Trim();
            if (string.IsNullOrEmpty(kullaniciAdi) || string.IsNullOrEmpty(sifre))
            {
                MessageBox.Show("Lütfen kullanıcı adı ve şifre giriniz.");
                return;
            }


            using (SqlConnection conn = new SqlConnection(Baglanti.baglantiYolu))
            {
                string query = "SELECT * FROM AnaBayi WHERE KullaniciAdi = @kullaniciAdi AND Sifre = @sifre AND Durum = 1 AND Silinmis = 0";


                SqlCommand cmd = new SqlCommand(query, conn);
                
                cmd.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);
                cmd.Parameters.AddWithValue("@sifre", sifre);

                
                conn.Open();

               
                SqlDataReader reader = cmd.ExecuteReader();

                
                if (reader.HasRows)
                {
                    MessageBox.Show("Giriş başarılı!");
                    MenuKontrolEkrani panelForm = new MenuKontrolEkrani();
                    panelForm.Show();
                    this.Hide(); 
                }
                else
                {
                    
                    MessageBox.Show("Giriş başarısız! Bilgilerinizi kontrol edin!");
                }
            }
        }
    }
}
