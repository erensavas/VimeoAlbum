using Newtonsoft.Json;
using Org.BouncyCastle.Math.EC.Rfc7748;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VimeoAlbum.Models;
using VimeoAlbum.Repository;
using VimeoAlbum.Services;
using VimeoDotNet.Models;


namespace VimeoAlbum
{
    public partial class Form1 : Form
    {
        public IServiceAlbum service;
        ListBox lst = new ListBox();
        List<ExcelModel> excelList = new List<ExcelModel>();
        List<ExcelModel> excelExport = new List<ExcelModel>();
        Urunler urun = new Urunler();
        List<ExcelViewModel> excelListOrbim = new List<ExcelViewModel>();
        public string previousSelectedNode = null;
        public string parenId = null;
        private int listIndex = -1;
        public IVimeoManager vimeoManager;


        public Form1()
        {
            InitializeComponent();

            // listBox1.DrawMode = DrawMode.OwnerDrawFixed;

        }



        private async void btnAlbumOlustur_Click(object sender, EventArgs e)
        {
            service = new ServiceAlbum();

            var videoGrup = excelList.Select(x => x.VideoName).Distinct().ToList();

            foreach (var item in videoGrup)
            {
                long albumId = await service.AlbumIdOlustur(item);
                var videoTestSayisi = excelList.Where(x => x.VideoName == item).ToList();

                if (albumId > 0)
                {

                    char start = '"'; char end = '"';
                    string aranan = string.Concat(start, item, end).ToString();
                    var result = await service.GetVideoGertir(aranan);
                    if (result.Count != 0)
                    {
                        if (videoTestSayisi.Count > result.Count)
                        {
                            listBox1.Items.Add($"{item} videoları vimeoda  eksik. Excel : {videoTestSayisi.Count} Vimeo : {result.Count}");
                        }
                        foreach (var item1 in result)
                        {
                            if (item1.Picture == null)
                            {
                                var result1 = await service.GetVideoVarMi(item1.VideoId);  //videonun upload status durumunu alır

                                if (result1 != null)
                                {
                                    if (result1.upload.status == "error")
                                    {
                                        listBox1.Items.Add($"{item1.VideoId}-{item1.VideoName} Video hatalı ----------------------");
                                    }
                                    else if (result1.upload.status == "complete")
                                    {
                                        listBox1.Items.Add($"{item1.VideoId}-{item1.VideoName} isimli videonun resmi yok. Ama videosu var");
                                    }
                                }

                            }
                        }

                        bool result2 = await service.AlbumeVideoEkleSevices(albumId, result);
                        if (result2)
                        {
                            listBox2.Items.Add($"{item} isimli albüm oluturuldu.");
                        }
                    }
                    else
                    {
                        listBox1.Items.Add($"{item} isimli video bulunamadı");
                    }

                }
            }

            MessageBox.Show("Vimeo album oluşturma işlem tamamlandı");

            btnAlbumOlustur.Enabled = false;
        }

        private async void btnVideoSearch_Click(object sender, EventArgs e)
        {
            //service = new ServiceAlbum();
            //var result =await service.GetVideoGertir("cozum-8-turkce-unite01-test01");
            ////listBox();
            //lblTotalCount.Text = $"Toplam : {result.Count} adet video bulundu.";
            ////lblTotalCoun.Text = $"Toplam : {videolarTek.Total} adet video bulundu.";
            //foreach (var item in result)
            //{
            //    lst.Items.Add(item.VideoName);
            //}



        }

        //private void listBox()
        //{
        //    lst.Name = "listbox";
        //    lst.Location = new Point(20, 100);
        //    lst.Width = 350;
        //    lst.Height = 300;
        //    this.Controls.Add(lst);
        //}

        private async void Form1_Load(object sender, EventArgs e)
        {
            progressBar1.MarqueeAnimationSpeed = 0;
            progressBar1.Style = ProgressBarStyle.Marquee;
            progressBar1.Visible = false;

            btnAlbumOlustur.Enabled = false;
            btnEkleOrbim.Enabled = false;
            lblUrunID.Text = "";

            var result = await urun.YayinlariGetir();

            comboBox1.DataSource = result.veri;

            comboBox1.DisplayMember = "yayinadi";
            comboBox1.ValueMember = "id";
            comboBox1.SelectedIndex = 0;
        }

        private async void btnExcelExport_Click(object sender, EventArgs e)
        {
            string result = await ExcelDoldur();
            if (excelExport.Count > 0)
            {
                bool deger = await ExcelExport.excelExportKaydet(result, excelExport);
                if (deger)
                {
                    MessageBox.Show("Excel dosyası oluşturuldu");
                }
            }

        }

        private async Task<string> ExcelDoldur()
        {
            excelExport.Clear();
            OpenFileDialog dl = new OpenFileDialog();
            dl.DefaultExt = "mp4";
            dl.FilterIndex = 1;
            dl.Multiselect = true;
            dl.Title = "Excel dosyası seçiniz";
            dl.Filter = "mp4 files (*.mp4)|*.mp4|avi files (*.avi)|*.avi|All files (*.*)|*.*";
            if (dl.ShowDialog() == DialogResult.OK)
            {

                string[] arrAllFiles = dl.SafeFileNames;

                foreach (var item in arrAllFiles)
                {
                    int bul = item.LastIndexOf('-');
                    excelExport.Add(
                        new ExcelModel()
                        {
                            VideoName = item.Substring(0, bul)
                        });
                }
            }

            return Path.GetDirectoryName(dl.FileName);

        }

        private async void btnEkleOrbim_Click(object sender, EventArgs e)
        {
            service = new ServiceAlbum();

            if (string.IsNullOrEmpty(txtSiraNumarasi.Text))
            {
                MessageBox.Show("Sıra numarası boş olamaz!");
                return;
            }

            if (previousSelectedNode == null)
            {
                MessageBox.Show("İlgili Kitap seçiniz");
                return;
            }
            else
            {
                //ApiService api = new ApiService();
                int yayinId = Convert.ToInt32(comboBox1.SelectedValue);
                int ustUrunId = Convert.ToInt32(treeView1.SelectedNode.Name);

                var uniteGrup = excelListOrbim.Select(x => x.Unite).Distinct().ToList();
                int siraUnite = 1;

                if (uniteGrup[0] == null)
                {
                    string selectedNodes = treeView1.SelectedNode.Name;
                    int siraTest = Convert.ToInt32(txtSiraNumarasi.Text);
                    foreach (var item1 in excelListOrbim)
                    {
                        // album no getir

                        long albumKodu = await service.GetAlbumNo(item1.AlbumAdi);
                        if (albumKodu > 0)
                        {
                            UrunPostModel modelTest = new UrunPostModel()
                            {
                                yid = yayinId,
                                uid = Convert.ToInt32(selectedNodes),
                                urunadi = item1.Test,
                                sira = siraTest,
                                barkodno = item1.QrCode,
                                albumkodu = albumKodu,
                                ilksoruno = "",
                                link = ""
                            };

                            int result = await Ekle(modelTest);
                            if (result == 1)
                            {
                                lstLog.Items.Add("Eklenemedi-" + item1.Test);
                            }
                            else
                            {
                                lstLog.Items.Add($"{item1.Unite} - {item1.Test} - {item1.AlbumAdi} eklendi.");
                            }
                            siraTest++;
                        }
                        else
                        {
                            lstLog.Items.Add($"{item1.Test} isimli albüm bulunamadı");
                        }

                    }
                }
                else
                {
                    foreach (var item in uniteGrup)
                    {
                        var grupFilter = excelListOrbim.Where(x => x.Unite == item).ToList();

                        if (grupFilter.Count != 0)
                        {
                            int eklenenId = 0;
                            int siraTest = Convert.ToInt32(txtSiraNumarasi.Text);
                            UrunPostModel modelUnite = new UrunPostModel()
                            {
                                yid = yayinId,
                                uid = ustUrunId,
                                urunadi = item,
                                sira = siraUnite,
                                barkodno = 0,
                                albumkodu = 0,
                                ilksoruno = "",
                                link = ""
                            };
                            eklenenId = await Ekle(modelUnite);
                            if (eklenenId == 1)
                            {
                                lstLog.Items.Add("Eklenemedi-" + item);
                            }
                            else
                            {

                                foreach (var item1 in grupFilter)
                                {
                                    // album no getir

                                    long albumKodu = await service.GetAlbumNo(item1.AlbumAdi);
                                    if (albumKodu > 0)
                                    {
                                        UrunPostModel modelTest = new UrunPostModel()
                                        {
                                            yid = yayinId,
                                            uid = eklenenId,
                                            urunadi = item1.Test,
                                            sira = siraTest,
                                            barkodno = item1.QrCode,
                                            albumkodu = albumKodu,
                                            ilksoruno = "",
                                            link = ""
                                        };

                                        int result = await Ekle(modelTest);
                                        if (result == 1)
                                        {
                                            lstLog.Items.Add("Eklenemedi-" + item1.Test);
                                        }
                                        else
                                        {
                                            lstLog.Items.Add($"{item1.Unite} - {item1.Test} - {item1.AlbumAdi} eklendi.");
                                        }
                                        siraTest++;
                                    }
                                    else
                                    {
                                        lstLog.Items.Add($"{item1.Test} isimli albüm bulunamadı");
                                    }

                                }

                            }

                        }

                        siraUnite++;
                    }
                }




            }


            MessageBox.Show("Orbim video ilişkilendirmesi yapıldı");

            btnEkleOrbim.Enabled = false;

        }

        private async Task<int> Ekle(UrunPostModel model)
        {

            var properties = from p in model.GetType().GetProperties()
                             where p.GetValue(model, null) != null
                             select p.Name + "=" + WebUtility.UrlEncode(p.GetValue(model, null).ToString());

            // queryString will be set to "Id=1&State=26&Prefix=f&Index=oo"                  
            string queryString = String.Join("&", properties.ToArray());


            var result = await urun.UrunApiPost("ekle", queryString);
            if (result.eklenenid == 0)
                return 1;
            else
                return result.eklenenid;
            // TODO: Add insert logic here
        }

        private void btnExcelOrbim_Click(object sender, EventArgs e)
        {
            excelListOrbim.Clear();
            lstLog.Items.Clear();

            OpenFileDialog dl = new OpenFileDialog();
            dl.DefaultExt = "xlsx";
            dl.FilterIndex = 2;
            dl.Title = "Excel dosyası seçiniz";
            dl.Filter = "xls files (*.xls)|*.xls|xlsx files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            if (dl.ShowDialog() == DialogResult.OK)
            {
                //int rowNumber = Convert.ToInt32(comboBox1.Text);
                excelListOrbim.AddRange(ExcelImport.procExcelOrbim(dl.FileName));

            }

            if (excelListOrbim.Count > 0)
            {
                btnEkleOrbim.Enabled = true;
            }

        }

        private void btnExcelVimeo_Click(object sender, EventArgs e)
        {
            excelList.Clear();
            listBox1.Items.Clear();
            listBox2.Items.Clear();

            OpenFileDialog dl = new OpenFileDialog();
            dl.DefaultExt = "xlsx";
            dl.FilterIndex = 2;
            dl.Title = "Excel dosyası seçiniz";
            dl.Filter = "xls files (*.xls)|*.xls|xlsx files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            if (dl.ShowDialog() == DialogResult.OK)
            {
                //int rowNumber = Convert.ToInt32(comboBox1.Text);
                excelList.AddRange(ExcelImport.procExcelVimeo(dl.FileName));

            }

            if (excelList.Count > 0)
            {
                btnAlbumOlustur.Enabled = true;
            }
        }

        private async void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node;
            //MessageBox.Show(node.Name);
            previousSelectedNode = treeView1.SelectedNode.Name;
            StaticAyar.SelectedNode = treeView1.SelectedNode.Name;

            var urunler = await urun.UrunleriGetir("", node.Name, comboBox1.SelectedValue.ToString());
            string selectedNodes = treeView1.SelectedNode.Name;
            var prn = treeView1.SelectedNode.Parent?.Name;
           
            parenId = prn;

            //MessageBox.Show(selectedNodes);
            label1.Text = $"SEÇİLEN ID :";
            lblUrunID.Text = selectedNodes;
            lblUstId.Text = $"ÜST ID : {prn}";
            treeView1.SelectedNode.Nodes.Clear();
            treeView1.Refresh();
            foreach (var item in urunler.veri)
            {

                treeView1.SelectedNode.Nodes.Add(item.id.ToString(), item.urunadi.ToString());
            }
        }

        private async void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            string comboSelectedValue = comboBox1.SelectedValue.ToString();
            if (comboBox1.SelectedIndex != 0)
            {
                var urunler = await urun.UrunleriGetir("", "#", comboSelectedValue);

                foreach (var item in urunler.veri)
                {
                    treeView1.Nodes.Add(item.id.ToString(), item.urunadi.ToString());
                   
                }
            }
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var result = listBox1.SelectedItem.ToString();
            var parse = result.Split('-');
            string deger = parse.First();
            string vimeo = "https://vimeo.com/";
            System.Diagnostics.Process.Start(vimeo + deger);

        }

        private void renklendirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //this.listBox1.DrawMode = DrawMode.OwnerDrawFixed;
            //this.listBox1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(listBox1_DrawItem);

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //selectedListBox = listBox1.SelectedItem.ToString();

            listIndex = listBox1.SelectedIndex;


        }

        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (listIndex == -1)
                {
                    MessageBox.Show("Önce seçim yapın");
                    return;
                }

            }

            //if (e.Button != MouseButtons.Right) return;
            //var index = listBox1.IndexFromPoint(e.Location);
            //MessageBox.Show(index.ToString());
        }


        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {


        }

        private void lblTemizle_Click(object sender, EventArgs e)
        {
            excelList.Clear();
            listBox1.Items.Clear();
            listBox2.Items.Clear();

        }

        private void lblOrbimTemizle_Click(object sender, EventArgs e)
        {
            excelListOrbim.Clear();
            lstLog.Items.Clear();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
          


        }

        private void kitapEkleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UrunPostModel model = new UrunPostModel()
            {
                yid = Convert.ToInt32(comboBox1.SelectedValue),
                uid = Convert.ToInt32(previousSelectedNode),
                barkodno = 0,
                albumkodu = 0
            };

            frmKitaplar frm = new frmKitaplar(model);
            frm.ShowDialog();
        }

        private async void urunSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Kaydı silmek istiyor musunuz?", "Onay", MessageBoxButtons.YesNoCancel);
            if (result == DialogResult.Yes)
            {
                int yid = Convert.ToInt32(comboBox1.SelectedValue);
                int uid = Convert.ToInt32(previousSelectedNode);
                var result1 = await urun.UrunApiPost("sil", "id=" + uid + "&yid=" + yid);

                if (result1.eklenenid == 0 && result1.durum)
                    MessageBox.Show("Ürün Silindi");
                else
                    MessageBox.Show(result1.mesaj.ToString());
            }
        }

        private async void urunDuzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string yid = comboBox1.SelectedValue.ToString();
            string id = previousSelectedNode;
            string uid = parenId;
              
            var urunler = await urun.UrunleriGetir(id, uid, yid);

            var result = urunler.veri.SingleOrDefault();

            var urun1 = new UrunPostModel()
            {
                aciklama = result.aciklama,
                albumkodu = result.albumkodu,
                barkodno = result.barkodno,
                id = result.id,
                yid = result.yayinid,
                urunadi = result.urunadi,
                uid = result.usturunid,
                sira = result.sira,
                link = result.link,
                ilksoruno = result.ilksoruno
            };

            frmKitaplar frm = new frmKitaplar(urun1);
            frm.ShowDialog();

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void lblUrunID_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(lblUrunID.Text);
        }

        private async void btnUpload_Click(object sender, EventArgs e)
        {
           
           
            vimeoManager = new VimeoManager();

            OpenFileDialog dl = new OpenFileDialog();
            dl.DefaultExt = "mp4";
            dl.FilterIndex = 2;
            dl.Title = "mp4 dosyası seçiniz";
            dl.Filter = "mp4 files (*.mp4)|*.mp4|All files (*.*)|*.*";
            dl.Multiselect = true;
            if (dl.ShowDialog() == DialogResult.OK)
            {
                progressBar1.MarqueeAnimationSpeed = 40;
                progressBar1.Visible = true;

                //var filePath = Path.GetTempFileName();
                //Stream fileStream = dl.OpenFile();
                //var fileContent = string.Empty;

                foreach (var formFile in dl.FileNames)
                {

                    FileStream stream = System.IO.File.OpenRead(formFile);
               

                    VimeoDotNet.Net.BinaryContent content = new VimeoDotNet.Net.BinaryContent(stream.Name);

                    content.OriginalFileName = formFile;

                    VimeoDotNet.Net.IUploadRequest request = await vimeoManager.VideoYukle(content, unchecked((int)stream.Length));
                    VimeoDotNet.Models.VideoUpdateMetadata metadata = new VimeoDotNet.Models.VideoUpdateMetadata();
                    metadata.Name = Path.GetFileNameWithoutExtension(formFile);

                     await vimeoManager.VideoMetadataGuncelle(request.ClipId.Value, metadata);

                }

                progressBar1.Visible = false;

            }


        }
    }
}
