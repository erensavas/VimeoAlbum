using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VimeoAlbum.Models
{
    public class files
    {
        public string quality { get; set; }
        public string type { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string link { get; set; }
        public string created_time { get; set; }
        public int fps { get; set; }
        public long size { get; set; }
        public string md5 { get; set; }
        public string public_name { get; set; }
        public string size_short { get; set; }
        public string link_secure { get; set; }
    }
}
