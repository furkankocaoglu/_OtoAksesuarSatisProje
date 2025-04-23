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
            this.Hide(); // Mevcut (ana) formu gizle
            AnaBayiGirisEkrani cikisForm = new AnaBayiGirisEkrani();
            cikisForm.Show(); 
        }
    }
}
