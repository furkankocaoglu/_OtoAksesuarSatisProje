﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtoAksesuarSatis.Model
{
    public class Kategori
    {
        public int KategoriID { get; set; }
        public string KategoriAdi { get; set; }
        public bool Durum { get; set; }
        public bool Silinmis { get; set; }
    }
}
