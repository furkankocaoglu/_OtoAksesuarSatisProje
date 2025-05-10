using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OtoAksesuarSatis
{
    public partial class MenuKontrolEkrani : Form
    {
        public MenuKontrolEkrani()
        {
            InitializeComponent();
        }

        private void listeleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            UrunIslemleri listele = new UrunIslemleri();
            listele.MdiParent = this;
            listele.Show();
        }

        private void kaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AnaBayiPanelForm urunEkle = new AnaBayiPanelForm();
            urunEkle.MdiParent = this;
            urunEkle.Show();
        }

        private void çıkışYapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult sonuc = MessageBox.Show(
        "Çıkış yapmak istediğinize emin misiniz?",
        "Çıkış Onayı",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Question);

            if (sonuc == DialogResult.Yes)
            {
                this.Hide(); 
                AnaBayiGirisEkrani cikisForm = new AnaBayiGirisEkrani();
                cikisForm.Show();
            }
        }

        private void ekleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            KategoriEkle kategoriEkle = new KategoriEkle();
            kategoriEkle.MdiParent = this;
            kategoriEkle.Show();
            
        }

        private void listeleToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            
            MarkaListele markaListele = new MarkaListele();
            markaListele.MdiParent = this;
            markaListele.Show();
        }

        private void ekleToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
            MarkaEkle markaEkle = new MarkaEkle();
            markaEkle.MdiParent = this;
            markaEkle.Show();
        }

        private void listeleToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
            KategoriListele kategoriListele = new KategoriListele();
            kategoriListele.MdiParent = this;
            kategoriListele.Show();
        }

       
    }
}
