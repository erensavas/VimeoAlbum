using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VimeoAlbum.Models
{
    public class UrunGelenJsonModel
    {
        public bool durum { get; set; }
        public int kayitsayisi { get; set; }
        public string mesaj { get; set; }
        public List<UrunModel> veri { get; set; }
    }
}
