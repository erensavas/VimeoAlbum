using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VimeoAlbum.Models;
using VimeoDotNet.Models;
using VimeoDotNet.Parameters;

namespace VimeoAlbum.Repository
{

    public class VimeoManager : IVimeoManager
    {
        public async Task<JObject> GetUserIdAndAccessToken(string code)
        {
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("client_id", "97e3a880b63f0332a79455957bfeea7f96391adf");
            parameters.Add("client_secret", "9X6JwrJ0kenKwhvrp1ubiYXxBT4V23p8+iH8FgVv1Y922U4zuD9ibQJDCy2eR/rl3ne7VbJXJOJWFyCKWLLfzGU7V06xDCXUi+E/FuEW3zsbnTgc0zTbEd7vraNWRVQh");
            parameters.Add("grant_type", "authorization_code");
            parameters.Add("redirect_uri", "https://localhost:44339/");
            parameters.Add("code", code);

            WebClient client = new WebClient();
            var result = await client.UploadValuesTaskAsync("https://api.vimeo.com/oauth/access_token", "POST", parameters);
            var response = System.Text.Encoding.Default.GetString(result);

            var returnContent = (JObject)JsonConvert.DeserializeObject(response);
            //string accessToken = (string)returnContent["access_token"];
            //string id = returnContent["user"]["id"].ToString();




            return returnContent;

        }

        public async Task<VimeoDotNet.Models.Paginated<VimeoDotNet.Models.Album>> AlbumleriGetirHizliAsync(int Page, string Query)
        {
            VimeoDotNet.VimeoClient client = new VimeoDotNet.VimeoClient(TokenKey.Token);
            VimeoDotNet.Parameters.GetAlbumsParameters parametreler = new VimeoDotNet.Parameters.GetAlbumsParameters();
            parametreler.Page = Page;
            parametreler.PerPage = 50;
            parametreler.Query = Query;
            parametreler.Sort = GetAlbumsSortOption.Date;
            parametreler.Direction = GetAlbumsSortDirectionOption.Desc;
            parametreler.Fields = "name,uri";


            try
            {

                VimeoDotNet.Models.Paginated<VimeoDotNet.Models.Album> albumler = await client.GetAlbumsAsync(VimeoDotNet.Models.UserId.Me, parametreler);
                return albumler;
            }
            catch (Exception ex)
            {
                if (client.RateLimitRemaining == 0)
                {
                    throw new Exception("Çok fazla istek nedeniyle api erişimi engellendi." + (client.RateLimitReset.AddHours(1) - System.DateTime.Now).TotalMinutes + " dakika sonra tekrar deneyiniz.");
                }
                else
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<VimeoDotNet.Models.Paginated<VimeoDotNet.Models.Album>> AlbumleriGetirAsync(int Page, string Query)
        {
            VimeoDotNet.VimeoClient client = new VimeoDotNet.VimeoClient(TokenKey.Token);
            string[] fields = { "name", "uri", "link", "created_time", "metadata.connections" };

            GetAlbumsParameters parametreler = new GetAlbumsParameters();
            parametreler.Page = Page;
            parametreler.PerPage = 50;
            parametreler.Query = Query;
            parametreler.Sort = GetAlbumsSortOption.Date;
            parametreler.Direction = GetAlbumsSortDirectionOption.Desc;

            
            VimeoDotNet.Models.Paginated<VimeoDotNet.Models.Album> albumler = await client.GetAlbumsAsync(VimeoDotNet.Models.UserId.Me, fields, parametreler);
            return albumler;
        }

        public async Task<VimeoDotNet.Models.Paginated<VimeoDotNet.Models.Album>> AlbumleriGetirAsync()
        {
            VimeoDotNet.VimeoClient client = new VimeoDotNet.VimeoClient(TokenKey.Token);
            string[] fields = { "name", "uri", "link", "created_time", "metadata.connections" };

            VimeoDotNet.Models.Paginated<VimeoDotNet.Models.Album> albumler = await client.GetAlbumsAsync(VimeoDotNet.Models.UserId.Me, fields);
            return albumler;
        }

        public async Task<VimeoDotNet.Models.Paginated<VimeoDotNet.Models.Video>> VideolariGetirAsync(int? Page, string query)
        {

            VimeoDotNet.VimeoClient client1 = new VimeoDotNet.VimeoClient(TokenKey.Token);



            VimeoDotNet.Models.Paginated<VimeoDotNet.Models.Video> videolar = await client1.GetVideosAsync(Page, 50, query, new string[4] { "uri", "name", "created_time", "pictures" });
            return videolar;
        }

        public async Task<VimeoDotNet.Models.Video> VideolariGetirDenemeAsync()
        {

            //VimeoDotNet.VimeoClient client1 = new VimeoDotNet.VimeoClient("885d14518da612dac4bd9f06a877b7e1");
            VimeoDotNet.VimeoClient client1 = new VimeoDotNet.VimeoClient(TokenKey.Token);//kendim


           var result= await client1.GetVideoAsync(440624542, new string[4] { "uri", "name", "created_time", "pictures" });
            return result;
        }


        // videoları getir 1 adet video için
        public async Task<VimeoDotNet.Models.Paginated<VimeoDotNet.Models.Video>> VideolariGetirBirAdetAsync(int? Page, string query)
        {

            //VimeoDotNet.VimeoClient client1 = new VimeoDotNet.VimeoClient("885d14518da612dac4bd9f06a877b7e1");
            VimeoDotNet.VimeoClient client1 = new VimeoDotNet.VimeoClient(TokenKey.Token);//kendim


            VimeoDotNet.Models.Paginated<VimeoDotNet.Models.Video> videolar = await client1.GetVideosAsync(Page, 1, query, new string[4] { "uri", "name", "created_time", "pictures" });
            return videolar;
        }


        public async Task<bool> AlbumeVideoEkle(long albumId, string videoId)
        {

            VimeoDotNet.VimeoClient client1 = new VimeoDotNet.VimeoClient(TokenKey.Token);

            VimeoDotNet.Parameters.AddToAlbumParameters parametreler = new VimeoDotNet.Parameters.AddToAlbumParameters();
            parametreler.videos = videoId;


            var result = await client1.AddToAlbumAsync(albumId, parametreler);

            return result;
        }

        public async Task<bool> AlbumdenVideoSil(int albumId, int videoId)
        {





            VimeoDotNet.VimeoClient client1 = new VimeoDotNet.VimeoClient(TokenKey.Token);



            var result = await client1.RemoveFromAlbumAsync(albumId, videoId);

            return result;
        }

        public async Task<Album> AlbumOlustur(AlbumOlusturModel model)
        {

            VimeoDotNet.Parameters.EditAlbumParameters parametreler = new VimeoDotNet.Parameters.EditAlbumParameters();
            parametreler.Name = model.Name;
            parametreler.Description = model.Description;
            parametreler.Privacy = VimeoDotNet.Parameters.EditAlbumPrivacyOption.Anybody;





            VimeoDotNet.VimeoClient client1 = new VimeoDotNet.VimeoClient(TokenKey.Token);



            var result = await client1.CreateAlbumAsync(VimeoDotNet.Models.UserId.Me, parametreler);

            return result;
        }

        public async Task<Album> AlbumDuzenle(VimeoDotNet.Models.Album model, Int64 albumId)
        {

            VimeoDotNet.Parameters.EditAlbumParameters parametreler = new VimeoDotNet.Parameters.EditAlbumParameters();
            parametreler.Name = model.Name;
            parametreler.Description = model.Description;
            parametreler.Privacy = VimeoDotNet.Parameters.EditAlbumPrivacyOption.Anybody;






            VimeoDotNet.VimeoClient client1 = new VimeoDotNet.VimeoClient(TokenKey.Token);

            try
            {


                var result = await client1.UpdateAlbumAsync(VimeoDotNet.Models.UserId.Me, albumId, parametreler);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }

        }

        public async Task<Album> AlbumGetir(long albumId)
        {

            VimeoDotNet.VimeoClient client1 = new VimeoDotNet.VimeoClient(TokenKey.Token);

            var result = await client1.GetAlbumAsync(VimeoDotNet.Models.UserId.Me, albumId);

            return result;
        }

        public async Task<bool> AlbumSil(long albumId)
        {







            VimeoDotNet.VimeoClient client1 = new VimeoDotNet.VimeoClient(TokenKey.Token);



            var result = await client1.DeleteAlbumAsync(VimeoDotNet.Models.UserId.Me, albumId);

            return result;
        }

        public async Task<Paginated<Video>> AlbumVideolariGetir(int? Page, string query, Int64 albumId)
        {

            VimeoDotNet.VimeoClient client1 = new VimeoDotNet.VimeoClient(TokenKey.Token);

            var result = await client1.GetAlbumVideosAsync(VimeoDotNet.Models.UserId.Me, Convert.ToInt32(albumId), Page, 20, "date", "desc", query, new string[3] { "uri", "name", "created_time" });

            return result;
        }
        public async Task<UploadTicket> GetUploadTicket()
        {







            VimeoDotNet.VimeoClient client1 = new VimeoDotNet.VimeoClient(TokenKey.Token);


            try
            {

                return await client1.GetUploadTicketAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }

        }

        public async Task<VimeoDotNet.Net.IUploadRequest> VideoYukle(VimeoDotNet.Net.IBinaryContent content, int size)
        {







            VimeoDotNet.VimeoClient client1 = new VimeoDotNet.VimeoClient(TokenKey.Token);


            try
            {

                return await client1.UploadEntireFileAsync(content, size);
            }
            catch (Exception ex)
            {
                return null;
                //throw new Exception(ex.Message);

            }

        }

        public async Task VideoMetadataGuncelle(long videoId, VideoUpdateMetadata metadata)
        {







            VimeoDotNet.VimeoClient client1 = new VimeoDotNet.VimeoClient(TokenKey.Token);


            try
            {

                await client1.UpdateVideoMetadataAsync(videoId, metadata);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);

            }

        }

        public async Task<bool> VideoDomainIzniEkle(long? videoId, string domain)
        {







            VimeoDotNet.VimeoClient client1 = new VimeoDotNet.VimeoClient(TokenKey.Token);


            try
            {
                await client1.UpdateVideoAllowedDomainAsync(videoId, domain, "put");
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }

        }

        public async Task<bool> VideoDomainIzniSil(long videoId, string domain)
        {







            VimeoDotNet.VimeoClient client1 = new VimeoDotNet.VimeoClient(TokenKey.Token);


            try
            {
                await client1.UpdateVideoAllowedDomainAsync(videoId, domain, "delete");
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }

        }

        public async Task<bool> VideoSil(long videoId)
        {







            VimeoDotNet.VimeoClient client1 = new VimeoDotNet.VimeoClient(TokenKey.Token);


            try
            {
                await client1.DeleteVideoAsync(videoId);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }

        }

        public async Task<Paginated<VideoAllowedDomain>> VideoIzinliDomainleriGetir(int? Page, long videoId)
        {







            VimeoDotNet.VimeoClient client1 = new VimeoDotNet.VimeoClient(TokenKey.Token);


            try
            {

                return await client1.GetAllowedDomains(Page, 20, videoId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }

        }

       

    }
}
