using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtoAksesuarSatis.Model
{
    public class Urun
    {
        public int UrunID { get; set; }
        public string UrunAdi { get; set; }
        public string Marka { get; set; }
        public string KategoriAdi { get; set; }
        public decimal BronzFiyat { get; set; }
        public decimal SilverFiyat { get; set; }
        public decimal GoldFiyat { get; set; }
        public int StokMiktari { get; set; }
        public string Aciklama { get; set; }
        public string ResimYolu { get; set; }
    }
}
