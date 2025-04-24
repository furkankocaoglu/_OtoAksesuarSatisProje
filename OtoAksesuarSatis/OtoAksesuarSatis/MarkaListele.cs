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

namespace OtoAksesuarSatis
{
    public partial class MarkaListele : Form
    {
        private int rowindex = -1;
        public MarkaListele()
        {
            InitializeComponent();
        }

        private void MarkaListele_Load(object sender, EventArgs e)
        {
            MarkalariListele();
        }
        private void MarkalariListele()
        {
            List<Marka> marks = new List<Marka>();
            SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS; Initial Catalog=OtoAksesuarsatis_db; Integrated Security=True");
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "SELECT * FROM Markalar WHERE Silinmis=0";
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Marka mark = new Marka();
                mark.MarkaID = reader.GetInt32(0);
                mark.MarkaAdi = reader.GetString(1);
                marks.Add(mark);
            }
            dataGridView1.DataSource = marks;
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {

                rowindex = e.RowIndex;


                if (rowindex >= 0)
                {

                    contextMenuStrip1.Show(dataGridView1, e.Location);
                }
            }
        }

        private void sılToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rowindex != -1)
            {
                int id = Convert.ToInt32(dataGridView1.Rows[rowindex].Cells[0].Value);

                if (MessageBox.Show($"{id} id'li marka silinecektir.\nOnaylıyor musunuz?", "Marka Sil", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS; Initial Catalog=OtoAksesuarsatis_db; Integrated Security=True");
                    SqlCommand cmd = con.CreateCommand();

                    // Silinmiş kategoriyi işaretleme
                    cmd.CommandText = "UPDATE Markalar SET Silinmis = 1, Durum = 0 WHERE MarkaID = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Ürün başarıyla silindi.", "Silme İşlemi");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ürün silinirken bir hata oluştu: " + ex.Message, "Hata");
                    }
                    finally
                    {
                        con.Close();
                    }
                    MarkalariListele();
                }
            }
        }
        private void SilinmisMarkalariListele()
        {
            List<Marka> markalar = new List<Marka>();
            SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS; Initial Catalog=OtoAksesuarsatis_db; Integrated Security=True");
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "SELECT MarkaID, MarkaAdi FROM Markalar WHERE Silinmis = 1";
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Marka marka = new Marka();
                marka.MarkaID = reader.GetInt32(0);
                marka.MarkaAdi = reader.GetString(1);
                markalar.Add(marka);
            }
            dataGridView2.DataSource = markalar;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SilinmisMarkalariListele();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView2.DataSource = !true;
        }
    }
}
