using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VimeoAlbum.Models;

namespace VimeoAlbum.Repository
{
    public class Klasorler
    {
        List<KlasorModel> dosyaYolu = new List<KlasorModel>();
        private int sayac = 0;

        public Klasorler()
        {
            dosyaYolu = new List<KlasorModel>();
        }
        
        public async Task<int> FileCopy(string targetPath)
        {
            try
            {
                sayac = 0;
                targetPath = $"{targetPath}\\TumKlasor";
                if (!File.Exists(targetPath))
                {
                    Directory.CreateDirectory(targetPath);
                }
                int count = dosyaYolu.Count();
                foreach (var item in dosyaYolu)
                {
                    string filename = targetPath + "\\" + item.FileName;
                    if (File.Exists(item.Path))
                    {
                        File.Copy(item.Path, filename);
                        sayac++;
                    }
                }
               
            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.Message);
            }

            return sayac;
        }

        public async Task<List<KlasorModel>> SubFolders(string path)
        {

           
            try
            {
              

                foreach (string folder in Directory.GetDirectories(path))
                {
                    foreach (string f in Directory.GetFiles(folder, "*"))
                    {
                        string extension = Path.GetExtension(f);
                        string getFileName = Path.GetFileName(f);
                        if (extension != null)
                        {
                            
                            dosyaYolu.Add(
                                new KlasorModel
                                {
                                    Path = f,
                                    FileName = getFileName
                                });
                        }
                    }
                    SubFolders(folder);
                }

                return dosyaYolu;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
