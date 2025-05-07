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
using System.Xml.Linq;
using System.IO;


namespace OtoAksesuarSatis
{
    public partial class UrunIslemleri : Form
    {
        private int rowindex = -1;
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

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rowindex != -1)
            {
                int id = Convert.ToInt32(dataGridView1.Rows[rowindex].Cells[0].Value);

                if (MessageBox.Show($"{id} id'li ürün silinecektir.\nOnaylıyor musunuz?", "Ürün Sil", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    
                    SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS; Initial Catalog=OtoAksesuarsatis_db; Integrated Security=True");
                    SqlCommand cmd = con.CreateCommand();

                    cmd.CommandText = "UPDATE Urunler SET Silinmis = 1, AktifMi = 0 WHERE UrunID = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Ürün başarıyla silindi.", "Silme İşlemi");

                        
                        string xmlYolu = @"C:\BayilikXML\Urunler.xml"; 

                        if (File.Exists(xmlYolu))
                        {
                            XDocument doc = XDocument.Load(xmlYolu);
                            var urunElement = doc.Root.Elements("urun")
                                                      .FirstOrDefault(x => x.Element("UrunID")?.Value == id.ToString());

                            if (urunElement != null)
                            {
                                urunElement.Remove();
                                doc.Save(xmlYolu); 
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ürün silinirken bir hata oluştu: " + ex.Message, "Hata");
                    }
                    finally
                    {
                        con.Close();
                    }

                    
                    UrunleriYukle();
                }
            }
        }

        private void düzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rowindex != -1)  
            {
               
                int urunID = Convert.ToInt32(dataGridView1.Rows[rowindex].Cells[0].Value);


                UrunDuzenle urunDuzenleForm = new UrunDuzenle(urunID);  
                urunDuzenleForm.ShowDialog();  
            }
        }
    }

}
