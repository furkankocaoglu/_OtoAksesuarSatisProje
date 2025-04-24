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
    public partial class MarkaEkle : Form
    {
        public MarkaEkle()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=OtoAksesuarsatis_db;Integrated Security=true");
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "INSERT INTO Markalar(MarkaAdi) VALUES(@n)";
                cmd.Parameters.AddWithValue("@n", textBox1.Text);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Marka başarıyla eklendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                catch
                {
                    MessageBox.Show("Marka eklenirken hata oluştu", "Hata");
                }
                finally
                {
                    con.Close();
                }
            }
            else
            {
                MessageBox.Show("Lütfen marka adını giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
    
}
