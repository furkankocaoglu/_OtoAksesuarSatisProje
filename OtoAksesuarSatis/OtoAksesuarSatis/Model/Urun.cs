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
        public decimal Fiyat { get; set; }

        public override string ToString()
        {
            return $"{UrunAdi} - {Marka} - {KategoriAdi} - {Fiyat:C2}";
        }
    }
}
