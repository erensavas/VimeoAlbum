using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VimeoAlbum.Models;

namespace VimeoAlbum.Repository
{
    public class Urunler
    {
        public async Task<YayinGelenJsonModel> YayinlariGetir()
        {
            string url = "http://orbim.in/orbiminapi/yayin.php";
            string postString;

            postString = string.Format("islem={0}", "getir");


            System.Net.ServicePointManager.Expect100Continue = false;
            WebRequest req = WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            StreamWriter requestWriter = new StreamWriter(req.GetRequestStream());
            requestWriter.Write(postString);
            requestWriter.Close();
            var r = await req.GetResponseAsync().ConfigureAwait(false);
            var responseReader = new StreamReader(r.GetResponseStream());
            var responseData = await responseReader.ReadToEndAsync();
            var d = Newtonsoft.Json.JsonConvert.DeserializeObject<YayinGelenJsonModel>(responseData);
            return d;
        }

        public async Task<UrunGelenJsonModel> UrunleriGetir(string id, string uid, string yid)
        {
            string url = "http://orbim.in/orbiminapi/urun.php";
            string postString;
            if (uid == "#")
            {
                postString = string.Format("islem={0}&yid={1}", "getir", yid);
            }
            else
            {
                postString = string.Format("islem={0}&yid={1}&uid={2}", "getir", yid, uid);
            }

            if (!String.IsNullOrEmpty(id))
            {
                postString = string.Format("islem={0}&yid={1}&id={2}&uid={3}", "getir", yid, id, uid);

            }

            System.Net.ServicePointManager.Expect100Continue = false;
            WebRequest req = WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            StreamWriter requestWriter = new StreamWriter(req.GetRequestStream());
            requestWriter.Write(postString);
            requestWriter.Close();
            var r = await req.GetResponseAsync().ConfigureAwait(false);
            var responseReader = new StreamReader(r.GetResponseStream());
            var responseData = await responseReader.ReadToEndAsync();
            var d = Newtonsoft.Json.JsonConvert.DeserializeObject<UrunGelenJsonModel>(responseData);
            return d;
        }

    

        public async Task<UrunPostResponseModel> UrunApiPost(string islem, string queryString)
        {

            string url = "http://orbim.in/orbiminapi/urun.php";


            System.Net.ServicePointManager.Expect100Continue = false;
            WebRequest req = WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            StreamWriter requestWriter = new StreamWriter(req.GetRequestStream());
            requestWriter.Write(queryString + "&islem=" + islem);
            requestWriter.Close();
            var r = await req.GetResponseAsync().ConfigureAwait(false);
            var responseReader = new StreamReader(r.GetResponseStream());
            var responseData = await responseReader.ReadToEndAsync();
            var d = Newtonsoft.Json.JsonConvert.DeserializeObject<UrunPostResponseModel>(responseData);
            return d;
        }

    }
}
