using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VimeoAlbum.Models
{
    public class UrunPostModel
    {
        public int id { get; set; }

        public int yid { get; set; }

        public string urunadi { get; set; }

        public int uid { get; set; }

        public int? sira { get; set; }

        public long barkodno { get; set; }

        public long albumkodu { get; set; }

        public string ilksoruno { get; set; }
        public string link { get; set; }
        public string aciklama { get; set; }
    }
}
