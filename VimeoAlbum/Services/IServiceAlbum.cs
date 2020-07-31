using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VimeoAlbum.Models;
using VimeoDotNet.Models;

namespace VimeoAlbum.Services
{
    public interface IServiceAlbum
    {
        Task<bool> AlbumeVideoEkleSevices(long AlbumId, List<ModelView> model);
        Task<long> AlbumIdOlustur(string AlbumAdi);
        Task<Album> AlbumOlustur();
        Task<List<ModelView>> GetVideoGertir(string VideoAdi);
        Task<long> GetAlbumNo(string albumAdi);
        Task<VimeoVideo> GetVideoVarMi(long VideoId);
      
    }
}
