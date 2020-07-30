using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VimeoDotNet.Models;

namespace VimeoAlbum.Models
{
    public class ModelView
    {
        public string VideoName { get; set; }
        public long VideoId { get; set; }
        public int VideoNumber { get; set; }
        public string VideoNameFull { get; set; }
        public Pictures Picture { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
