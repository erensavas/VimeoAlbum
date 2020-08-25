using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using VimeoAlbum.Models;
using VimeoAlbum.Repository;
using VimeoDotNet.Models;

namespace VimeoAlbum.Services
{
    public class ServiceAlbum:IServiceAlbum
    {
        public IVimeoManager vimeoManager;

      
        public async Task<Album> AlbumOlustur()
        {
            vimeoManager = new VimeoManager();
            AlbumOlusturModel model = new AlbumOlusturModel()
            {
                Name = "YeniAlbumOlustur",
                Description = "AlbumDeneme"
            };
            var result = await vimeoManager.AlbumOlustur(model);


            return result;
        }

        public async Task<long> AlbumIdOlustur(string AlbumAdi)
        {
            vimeoManager = new VimeoManager();
            long albumId = 0;
            AlbumOlusturModel model = new AlbumOlusturModel()
            {
                Name = AlbumAdi,
                Description = AlbumAdi
            };
            var result = await vimeoManager.AlbumOlustur(model);
            if (result != null)
            {
                albumId = result.GetAlbumId().Value;
                return albumId;
            }
            return 0;
           
        }


        public async Task<bool> AlbumeVideoEkleSevices(long AlbumId,List<ModelView> model)
        {
            vimeoManager = new VimeoManager();
            string joinle = String.Join(",", model.Select(v => v.VideoId).ToArray());
            var result = await vimeoManager.AlbumeVideoEkle(AlbumId, joinle);
            return result;
        }

        async Task<List<ModelView>> IServiceAlbum.GetVideoGertir(string VideoAdi)
        {
            vimeoManager = new VimeoManager();

            List<ModelView> AllVideos = new List<ModelView>();
            VimeoDotNet.Models.Paginated<VimeoDotNet.Models.Video> videolarTek = await vimeoManager.VideolariGetirBirAdetAsync(1, VideoAdi);

            
            int totalVideo = videolarTek.Total;
            int perPage = 50;
            var totalPages = (int)Math.Ceiling((decimal)totalVideo / (decimal)perPage);


            for (int i = 1; i <= totalPages; i++)
            {
                Paginated<Video> videolar = await vimeoManager.VideolariGetirAsync(i, VideoAdi);

               


               videolar.Data= videolar.Data.OrderBy(x => x.CreatedTime).ToList();


                foreach (var item in videolar.Data)
                {
                   
                    // eğer videnamenin sonunda (1) gibi parantez varsa ayıkla
                    int parantez = item.Name.IndexOf('(');
                    if (parantez != -1)
                    {
                        item.Name = item.Name.Substring(0, parantez);
                    }

                    var videoNumber = item.Name.Split('-');
                    // eğer aynı numaralı 2 video varsa resmi olmayanı sil


                    foreach (var item1 in AllVideos)
                    {
                        if (item.Name==item1.VideoName)
                        {
                            AllVideos.Remove(item1);
                            break;
                        }
                    }

                    ModelView model = new ModelView
                    {
                        VideoId = item.Id.Value,
                        VideoName = item.Name,
                        Picture = item.Pictures,
                        ModifiedDate=  item.CreatedTime,
                        VideoNumber=Convert.ToInt32(videoNumber.Last())  //videonameyi parse yap ve sondaki sayı numarasını al
                    };

                    AllVideos.Add(model);
                }
            }

            AllVideos = AllVideos.OrderBy(X => X.VideoName).ToList();
           
            return AllVideos;
        }

        public async Task<long> GetAlbumNo(string albumAdi)
        {
            vimeoManager = new VimeoManager();
            string search = String.Format("{0}", albumAdi);
            var albumler = await vimeoManager.AlbumleriGetirAsync(1, search);
            long albumNo = 0;
            int count = albumler.Total;
            if (count > 0)
            {
                var album = albumler.Data.Select(a => new
                {
                    id = a.GetAlbumId(),
                    text = a.Name
                }).ToList();


                albumNo = album.Select(x => x.id.Value).FirstOrDefault();

            }
            else
                return 1;

            return albumNo;
        }

        public async Task<VimeoVideo> GetVideoVarMi(long VideoId)
        {
            string url = "https://api.vimeo.com/me/videos";

         
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + TokenKey.Token);
                    client.DefaultRequestHeaders.Add("Accept", "application/vnd.vimeo.*+json;version=3.2");
                   // var content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");

                   
                    var response = await client.GetAsync($"{url}/{VideoId}");
                    if (response != null)
                    {

                        var jsonString = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<VimeoVideo>(jsonString);
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

      public async Task<List<AlbumOlusturModel>> GetAlbumAdi(string albumAdi)
        {
            vimeoManager = new VimeoManager();
            string search = String.Format("{0}", albumAdi);
            var albumler = await vimeoManager.AlbumleriGetirHizliAsync(1, search);
            var albumAl = new List<AlbumOlusturModel>();

            int count = albumler.Total;
            if (count > 0)
            {
                var album = albumler.Data.Select(a => new
                {
                    id = a.GetAlbumId(),
                    text = a.Name
                }).ToList();


                foreach (var item in album)
                {
                    albumAl.Add(
                        new AlbumOlusturModel
                        {
                            id = item.id.Value,
                            Name = item.text
                        });
                }

                //albumNo = album.Select(x => x.id.Value).FirstOrDefault();

            }
            else
                return null;

            return albumAl;
        }

        public async Task<Album> GetAlbumName(long AlbumId)
        {
            vimeoManager = new VimeoManager();
            var result = await vimeoManager.AlbumGetir(AlbumId);
            return result;
        }

    }
}
