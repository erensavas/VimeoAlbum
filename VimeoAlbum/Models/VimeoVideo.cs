using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VimeoAlbum.Models
{
    public class VimeoVideo
    {
        public string uri { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public string link { get; set; }
        public string duration { get; set; }
        public string width { get; set; }
        public string height { get; set; }
        public upload upload { get; set; }
        public List<files> files { get; set; }
    }
}
