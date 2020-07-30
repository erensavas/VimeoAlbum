using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VimeoAlbum.Models
{
    public class UrunModel
    {
        public int id { get; set; }

        public int yayinid { get; set; }

        public string urunadi { get; set; }

        public int usturunid { get; set; }

        public int? sira { get; set; }

        public int barkodno { get; set; }

        public int albumkodu { get; set; }

        public string ilksoruno { get; set; }
        public string link { get; set; }
        public string aciklama { get; set; }
    }
}
