using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VimeoAlbum.Models;
using VimeoDotNet.Models;

namespace VimeoAlbum.Repository
{
    public interface IVimeoManager
    {
        Task<JObject> GetUserIdAndAccessToken(string code);
        Task<VimeoDotNet.Models.Paginated<VimeoDotNet.Models.Album>> AlbumleriGetirAsync(int Page, string Query);
        Task<VimeoDotNet.Models.Paginated<VimeoDotNet.Models.Album>> AlbumleriGetirAsync();
        Task<VimeoDotNet.Models.Paginated<VimeoDotNet.Models.Video>> VideolariGetirAsync(int? Page, string query);
        Task<bool> AlbumeVideoEkle(long albumId, string clipId);
        Task<Album> AlbumOlustur(AlbumOlusturModel model);
        Task<Album> AlbumGetir(long albumId);
        Task<Album> AlbumDuzenle(VimeoDotNet.Models.Album model, long albumId);
        Task<bool> AlbumSil(long albumId);
        Task<Paginated<Video>> AlbumVideolariGetir(int? Page, string query, Int64 albumId);
        Task<bool> VideoDomainIzniEkle(long? videoId, string domain);
        Task<bool> VideoDomainIzniSil(long videoId, string domain);
        Task<Paginated<VideoAllowedDomain>> VideoIzinliDomainleriGetir(int? Page, long videoId);
        Task<bool> AlbumdenVideoSil(int albumId, int videoId);
        Task<UploadTicket> GetUploadTicket();
        Task<VimeoDotNet.Net.IUploadRequest> VideoYukle(VimeoDotNet.Net.IBinaryContent content, int size);
        Task VideoMetadataGuncelle(long videoId, VideoUpdateMetadata metadata);
        Task<bool> VideoSil(long videoId);
        Task<Paginated<Video>> VideolariGetirBirAdetAsync(int? Page, string query);
        Task<Video> VideolariGetirDenemeAsync();
    }
}
