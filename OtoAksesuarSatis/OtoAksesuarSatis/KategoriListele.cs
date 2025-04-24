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
    public partial class KategoriListele : Form
    {
        private int rowindex = -1;
        public KategoriListele()
        {
            InitializeComponent();
        }

        private void KategoriListele_Load(object sender, EventArgs e)
        {
            KategorileriListele();
        }
        private void KategorileriListele()
        {
            List<Kategori> kategoriler = new List<Kategori>();
            SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS; Initial Catalog=OtoAksesuarsatis_db; Integrated Security=True");
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "SELECT * FROM Kategoriler WHERE Silinmis = 0";
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Kategori kategori = new Kategori();
                kategori.KategoriID = reader.GetInt32(0);
                kategori.KategoriAdi = reader.GetString(1);
                kategoriler.Add(kategori);
            }
            dataGridView1.DataSource = kategoriler;
        }
        private void SilinmisKategorileriListele()
        {
            List<Kategori> kategoriler = new List<Kategori>();
            SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS; Initial Catalog=OtoAksesuarsatis_db; Integrated Security=True");
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "SELECT KategoriID, KategoriAdi FROM Kategoriler WHERE Silinmis = 1";  
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Kategori kategori = new Kategori();
                kategori.KategoriID = reader.GetInt32(0);
                kategori.KategoriAdi = reader.GetString(1);
                kategoriler.Add(kategori);
            }
            dataGridView2.DataSource = kategoriler;
        }


        private void sılToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rowindex != -1)
            {
                int id = Convert.ToInt32(dataGridView1.Rows[rowindex].Cells[0].Value);

                if (MessageBox.Show($"{id} ID'li kategori silinecektir.\nOnaylıyor musunuz?", "Kategori Sil", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS; Initial Catalog=OtoAksesuarsatis_db; Integrated Security=True");
                    SqlCommand cmd = con.CreateCommand();

                    
                    cmd.CommandText = "UPDATE Kategoriler SET Silinmis = 1, Durum = 0 WHERE KategoriID = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Kategori başarıyla silindi.", "Silme İşlemi");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Kategori silinirken bir hata oluştu: " + ex.Message, "Hata");
                    }
                    finally
                    {
                        con.Close();
                    }

                    
                    KategorileriListele();
                }
            }
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {

            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                dataGridView1.ClearSelection();
                dataGridView1.Rows[e.RowIndex].Selected = true;
                dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[0];

                rowindex = e.RowIndex;
                contextMenuStrip1.Show(dataGridView1, dataGridView1.PointToClient(Cursor.Position));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SilinmisKategorileriListele();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView2.DataSource = false;
        }

        private void duzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int kategoriID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["KategoriID"].Value);
            KategoriDuzenle duzenleForm = new KategoriDuzenle(kategoriID);


            DialogResult result = duzenleForm.ShowDialog();


            if (result == DialogResult.OK)
            {
                KategorileriListele();
            }

        }
    }
}
