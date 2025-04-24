using OtoAksesuarSatis.Model;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace OtoAksesuarSatis
{
    public partial class MarkaDuzenle : Form
    {
        private int markaID;
        public MarkaDuzenle(int markaID)
        {
            InitializeComponent();
            this.markaID = markaID;  
            markaYukle();  
        }
        private void markaYukle()
        {
            using (SqlConnection conn = new SqlConnection(Baglanti.baglantiYolu))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Markalar WHERE MarkaID = @MarkaID", conn);
                cmd.Parameters.AddWithValue("@MarkaID", this.markaID);   

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    
                    textBox1.Text = reader["MarkaAdi"].ToString();
                }
                conn.Close();
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string yeniMarkaAdi = textBox1.Text;

            
            using (SqlConnection conn = new SqlConnection(Baglanti.baglantiYolu))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Markalar SET MarkaAdi = @MarkaAdi WHERE MarkaID = @MarkaID", conn);
                cmd.Parameters.AddWithValue("@MarkaAdi", yeniMarkaAdi);
                cmd.Parameters.AddWithValue("@MarkaID", this.markaID);

                cmd.ExecuteNonQuery();
                conn.Close();
            }

            MessageBox.Show("Marka başarıyla güncellendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
            this.Close(); 
        }
        
    }
}
