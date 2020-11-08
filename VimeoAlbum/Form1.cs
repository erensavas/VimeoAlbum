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
using System.Threading;
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
        List<ExcelModel> domainList = new List<ExcelModel>();
        List<ExcelModel> excelAlbumIdList = new List<ExcelModel>();
        List<AlbumOlusturModel> excelExportAlbumId = new List<AlbumOlusturModel>();
        Urunler urun = new Urunler();
        List<ExcelViewModel> excelListOrbim = new List<ExcelViewModel>();
        public string previousSelectedNode = null;
        public string parenId = null;
        private int listIndex = -1;
        private string listValue = "";
        public IVimeoManager vimeoManager;
        // ÇOKLU KLASÖRÜ TEK YAPMA
        Klasorler klasor = new Klasorler();
        private string targetPath = "";
        private string dosyaAdiAl = "";

        private long VideoId = 0;

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
                            listBox1.Items.Add($"{item}/ videoları vimeoda  eksik. Excel : {videoTestSayisi.Count} Vimeo : {result.Count}");
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
                                        listBox1.Items.Add($"{item1.VideoName}/{item1.VideoId}/ Video hatalı ----------------------");
                                    }
                                    else if (result1.upload.status == "complete")
                                    {
                                        listBox1.Items.Add($"{item1.VideoName}/{item1.VideoId}/ isimli videonun resmi yok. Ama videosu var");
                                    }
                                }

                            }
                        }

                        bool result2 = await service.AlbumeVideoEkleSevices(albumId, result);
                        if (result2)
                        {
                            listBox2.Items.Add($"{item} isimli albüm oluşturuldu.");
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

        private async void Form1_Load(object sender, EventArgs e)
        {
            progressBar1.MarqueeAnimationSpeed = 0;
            progressBar1.Style = ProgressBarStyle.Marquee;
            progressBar1.Visible = false;

            btnAlbumOlustur.Enabled = false;
            btnEkleOrbim.Enabled = false;
            domainOlustur.Enabled = false;
            btnAlbumIdAl.Enabled = false;
            btnAlbumIdExcelExport.Enabled = false;
            txtManuelAlbumUpload.Visible = false;
            btnManuelAlbumUpload.Visible = false;
            txtManuelDomainAdd.Visible = false;
            btnManuleDomainAdd.Visible = false;
            lblManuelDomainAdd.Visible = false;

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
            if (result != "hata")
            {
                if (excelExport.Count > 0)
                {
                    bool deger = await ExcelExport.excelExportKaydet(result, excelExport);
                    if (deger)
                    {
                        MessageBox.Show("Excel dosyası oluşturuldu");
                    }
                }
            }


        }

        private async Task<string> ExcelDoldur()
        {
            excelExport.Clear();
            OpenFileDialog dl = new OpenFileDialog();
            //dl.DefaultExt = ".";
            dl.FilterIndex = 1;
            dl.Multiselect = true;
            dl.Title = "Excel dosyası seçiniz";
            dl.Filter = "mp4 files (*.mp4)|*.mp4|avi files (*.avi)|*.avi|All files (*.*)|*.*";
            dl.FilterIndex = 3;
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
            else
            {
                return "hata";
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
            //listBox1.SelectedIndexChanged += new EventHandler(listBox1_SelectedIndexChanged);
            listValue = listBox1.SelectedItem.ToString();
            var split = listValue.Split('/');
            var siraSil = split[0].Split('-');
            string videoAdi = split[0];

            videoAdi = videoAdi.Substring(0, videoAdi.LastIndexOf('-'));
            if (!string.IsNullOrEmpty(videoAdi))
            {
                MessageBox.Show(videoAdi);
                TekAlbumOlustur(videoAdi);
            }

        }

        private async void TekAlbumOlustur(string AlbumAdi)
        {
            service = new ServiceAlbum();
            long albumId = await service.AlbumIdOlustur(AlbumAdi);
            var videoTestSayisi = excelList.Where(x => x.VideoName == AlbumAdi).ToList();

            if (albumId > 0)
            {

                char start = '"'; char end = '"';
                string aranan = string.Concat(start, AlbumAdi, end).ToString();
                var result = await service.GetVideoGertir(aranan);
                if (result.Count != 0)
                {
                    if (videoTestSayisi.Count > result.Count)
                    {
                        listBox1.Items.Add($"{AlbumAdi}/ videoları vimeoda  eksik. Excel : {videoTestSayisi.Count} Vimeo : {result.Count}");
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
                                    listBox1.Items.Add($"{item1.VideoName}/{item1.VideoId}/ Video hatalı ----------------------");
                                }
                                else if (result1.upload.status == "complete")
                                {
                                    listBox1.Items.Add($"{item1.VideoName}/{item1.VideoId}/ Videonun resmi yok. Ama videosu var");
                                }
                            }

                        }
                    }

                    bool result2 = await service.AlbumeVideoEkleSevices(albumId, result);
                    if (result2)
                    {
                        listBox2.Items.Add($"{AlbumAdi} isimli albüm oluturuldu.");
                    }
                }
                else
                {
                    listBox1.Items.Add($"{AlbumAdi} isimli video bulunamadı");
                }

            }

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //selectedListBox = listBox1.SelectedItem.ToString();

            listIndex = listBox1.SelectedIndex; //  MessageBox.Show("Önce seçim yapın");  için gerekli


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


        private void lblUrunID_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(lblUrunID.Text);
        }

        private async void btnUpload_Click(object sender, EventArgs e)
        {


            vimeoManager = new VimeoManager();

            OpenFileDialog dl = new OpenFileDialog();
            dl.DefaultExt = ".";
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

                    if (request.ClipId.Value > 0 && request.FileLength > 0)
                    {
                        MessageBox.Show("Yükleme başarılı");
                    }

                }

                progressBar1.Visible = false;

            }


        }

        private async void btnKlasorSec_Click(object sender, EventArgs e)
        {
            Klasorler klasor1 = new Klasorler();
            FolderBrowserDialog dl = new FolderBrowserDialog();
            dl.SelectedPath = "E:\\";

            if (dl.ShowDialog() == DialogResult.OK)
            {
                int sayac = 0;
                var result = await klasor1.SubFolders(dl.SelectedPath);
                lblToplamKlasor.Text = $"Toplam dosya sayısı : {result.Count}";
                targetPath = dl.SelectedPath;

                sayac = await klasor1.FileCopy(targetPath);
                MessageBox.Show($"Tamamlandı {sayac}");
            }
        }

        private async void sonrakiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StaticAyar.OnayKodu = "";
            frmOnay onay = new frmOnay();
            onay.ShowDialog();

            if (StaticAyar.OnayKodu == "OK")
            {
                DialogResult resultOnay = MessageBox.Show("Güncelleme yapmadan önce excelden kopyalama yapınız?", "Onay", MessageBoxButtons.YesNoCancel);
                if (resultOnay == DialogResult.Yes)
                {
                    lstLog.Items.Clear();
                    string selectedNodesText = treeView1.SelectedNode.Text;
                    string selectedNodesName = treeView1.SelectedNode.Name;
                    string yid = comboBox1.SelectedValue.ToString();
                    string uid = selectedNodesName;


                    List<string> liste = new List<string>();


                    // Look for a file drop.
                    if (Clipboard.ContainsText(TextDataFormat.Text))
                    {
                        var files = Clipboard.GetText(TextDataFormat.Text);
                        liste.AddRange(files.Split(new[] { Environment.NewLine },
                                          StringSplitOptions.RemoveEmptyEntries));
                    }



                    TreeNode node = treeView1.SelectedNode;
                    int count = node.Nodes.Count;

                    for (int i = 0; i < count; i++)
                    {
                        // treeView1.SelectedNode = node.NextNode;
                        this.treeView1.SelectedNode = node.Nodes[i];


                        string id = treeView1.SelectedNode.Name;


                        var urunler = await urun.UrunleriGetir(id, uid, yid);

                        var result = urunler.veri.SingleOrDefault();

                        var urun1 = new UrunPostModel()
                        {
                            aciklama = result.aciklama,
                            albumkodu = result.albumkodu,
                            barkodno = Convert.ToInt64(liste[i]),
                            id = result.id,
                            yid = result.yayinid,
                            urunadi = result.urunadi,
                            uid = result.usturunid,
                            sira = result.sira,
                            link = result.link,
                            ilksoruno = result.ilksoruno
                        };

                        var DuzenleSonuc = await urun.UrunDuzenle(urun1);

                        lstLog.Items.Add(treeView1.SelectedNode.Name + "---" + result.urunadi + "---" + result.barkodno + "---" + liste[i]);

                    }
                    MessageBox.Show("Güncelleme işlemi bitti");
                }
                else
                {
                    return;
                }

            }
            else
            {
                MessageBox.Show("Lütfen onay kodunu doğru giriniz!");
                return;
            }



        }

        private async void btnAlbumIdBul_Click(object sender, EventArgs e)
        {
            service = new ServiceAlbum();
            lsbAlbumAdi.Items.Clear();
            if (string.IsNullOrEmpty(txtAlbumID.Text))
            {
                MessageBox.Show("Lütfen album id giriniz!");
            }
            else
            {
                var result = await service.GetAlbumName(Convert.ToInt64(txtAlbumID.Text));

                lsbAlbumAdi.Items.Add(result.Name);
                if (!string.IsNullOrEmpty(result.Name))
                {
                    txtAlbumID.Clear();
                }
            }
        }

        private void lsbAlbumAdi_DoubleClick(object sender, EventArgs e)
        {
            if (lsbAlbumAdi.SelectedItem != null)
            {
                Clipboard.SetText(lsbAlbumAdi.SelectedItem.ToString());
            }
        }

        private async void soruSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StaticAyar.OnayKodu = "";
            frmOnay onay = new frmOnay();
            onay.ShowDialog();

            if (StaticAyar.OnayKodu == "OK")
            {
                DialogResult resultOnay = MessageBox.Show("Do you want to delete the videos in the album?", "Onay", MessageBoxButtons.YesNoCancel);
                if (resultOnay == DialogResult.Yes)
                {
                    lstLog.Items.Clear();
                    string selectedNodesText = treeView1.SelectedNode.Text;
                    string selectedNodesName = treeView1.SelectedNode.Name;
                    int yid = Convert.ToInt32(comboBox1.SelectedValue);
                    int uid = Convert.ToInt32(selectedNodesName);


                    // List<string> liste = new List<string>();



                    TreeNode node = treeView1.SelectedNode;
                    int count = node.Nodes.Count;

                    for (int i = 0; i < count; i++)
                    {
                        // treeView1.SelectedNode = node.NextNode;
                        this.treeView1.SelectedNode = node.Nodes[i];


                        string id = treeView1.SelectedNode.Name;

                        //silme işlemleri
                        var result1 = await urun.UrunApiPost("sil", "id=" + id + "&yid=" + yid);

                        if (result1.eklenenid != 0 && result1.durum == false)
                            MessageBox.Show(result1.mesaj.ToString());
                        //silme işlemleri


                        lstLog.Items.Add(treeView1.SelectedNode.Name + "---" + this.treeView1.SelectedNode.Text + " silindi");

                    }
                    MessageBox.Show("Silme işlemi bitti");
                }
                else
                {
                    return;
                }

            }
            else
            {
                MessageBox.Show("Lütfen onay kodunu doğru giriniz!");
                return;
            }

        }

        private void domainExcelImport_Click(object sender, EventArgs e)
        {
            domainList.Clear();
            lbxDomainEkle.Items.Clear();

            OpenFileDialog dl = new OpenFileDialog();
            dl.DefaultExt = "xlsx";
            dl.FilterIndex = 2;
            dl.Title = "Excel dosyası seçiniz";
            dl.Filter = "xls files (*.xls)|*.xls|xlsx files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            if (dl.ShowDialog() == DialogResult.OK)
            {
                //int rowNumber = Convert.ToInt32(comboBox1.Text);
                domainList.AddRange(ExcelImport.procExcelVimeo(dl.FileName));

            }

            if (domainList.Count > 0)
            {
                domainOlustur.Enabled = true;
            }

        }


        private async void domainOlustur_Click(object sender, EventArgs e)
        {
            vimeoManager = new VimeoManager();

            if (String.IsNullOrEmpty(cmbDomainName.Text))
            {
                MessageBox.Show("Lütfen domain seçiniz!");
                return;
            }

            service = new ServiceAlbum();

            var videoGrup = domainList.Select(x => x.VideoName).Distinct().ToList();

            foreach (var item in videoGrup)
            {

                var videoTestSayisi = domainList.Where(x => x.VideoName == item).ToList();


                char start = '"'; char end = '"';
                string aranan = string.Concat(start, item, end).ToString();
                var result = await service.GetVideoGertir(aranan);
                if (result.Count != 0)
                {
                    if (videoTestSayisi.Count > result.Count)
                    {
                        lbxDomainEkle.Items.Add($"{item}/ videoları vimeoda  eksik. Excel : {videoTestSayisi.Count} Vimeo : {result.Count}");
                    }
                    foreach (var item1 in result)
                    {

                        bool result1 = await vimeoManager.VideoDomainIzniEkle(item1.VideoId, cmbDomainName.Text);
                        if (result1)
                        {
                            lbxDomainEkle.Items.Add($"{item1.VideoName}/ isimli videoya   {cmbDomainName.Text} domain izni eklendi");
                        }
                        else
                        {
                            lbxDomainEkle.Items.Add($"{item1.VideoName}/ isimli videoya   {cmbDomainName.Text} domain izni eklenemedi --------");
                        }
                    }


                }
                else
                {
                    lbxDomainEkle.Items.Add($"{item} isimli videolar bulunamadı");
                }

            }

            MessageBox.Show("Vimeo domain oluşturma işlem tamamlandı");

            domainOlustur.Enabled = false;



        }

        private void btnAlbumImport_Click(object sender, EventArgs e)
        {
            excelAlbumIdList.Clear();
            lbxAlbumIdleri.Items.Clear();
            dosyaAdiAl = "";
            btnAlbumIdExcelExport.Enabled = false;

            OpenFileDialog dl = new OpenFileDialog();
            dl.DefaultExt = "xlsx";
            dl.FilterIndex = 2;
            dl.Title = "Excel dosyası seçiniz";
            dl.Filter = "xls files (*.xls)|*.xls|xlsx files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            if (dl.ShowDialog() == DialogResult.OK)
            {
                //int rowNumber = Convert.ToInt32(comboBox1.Text);
                excelAlbumIdList.AddRange(ExcelImport.procExcelVimeo(dl.FileName));

            }


            if (excelAlbumIdList.Count > 0)
            {
                btnAlbumIdAl.Enabled = true;

            }
            dosyaAdiAl = Path.GetFileNameWithoutExtension(dl.FileName);


        }

        private async void btnAlbumIdAl_Click(object sender, EventArgs e)
        {
            service = new ServiceAlbum();
            excelExportAlbumId.Clear();
            // List<string> videoGrup1 = new List<string>();


            var videoGrup1 = excelAlbumIdList.Select(x => x.VideoName).Distinct().ToList();

            // buraya geçici parse ekle
            //foreach (var item in videoGrup)
            //{
            //    int bul = item.LastIndexOf('-');
            //    videoGrup1.Add(item.Substring(0, bul));
            //}
            //videoGrup1 = videoGrup1.Distinct().ToList();
            // buraya geçici parse ekle

            if (videoGrup1 != null)
            {
                foreach (var item in videoGrup1)
                {
                    var result = await service.GetAlbumAdi(item);

                    if (result == null)
                    {
                        long id = 0;
                        string name = item;

                        if (!name.Contains("orneksorular"))
                        {
                            lbxAlbumIdleri.Items.Add(name + " - " + id);
                            excelExportAlbumId.Add(
                           new AlbumOlusturModel
                           {
                               id = id,
                               Name = name
                           });
                        }

                    }
                    else
                    {
                        long id = result.Select(x => x.id).FirstOrDefault();
                        string name = result.Select(x => x.Name).FirstOrDefault();

                        if (!name.Contains("orneksorular"))
                        {
                            lbxAlbumIdleri.Items.Add(name + " - " + id);
                            excelExportAlbumId.Add(
                            new AlbumOlusturModel
                            {
                                id = id,
                                Name = name
                            });
                        }
                    }
                }

                MessageBox.Show("Vimeo album id oluşturma işlemi tamamlandı");
                btnAlbumIdExcelExport.Enabled = true;
                btnAlbumIdAl.Enabled = false;

            }



        }

        private async void btnAlbumIdExcelExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog sd = new SaveFileDialog();
            sd.DefaultExt = "xlsx";
            sd.FilterIndex = 2;
            sd.Title = "Excel dosyası kaydet";
            sd.RestoreDirectory = true;
            sd.FileName = dosyaAdiAl + "-AlbumID";
            sd.Filter = "xls files (*.xls)|*.xls|xlsx files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            if (sd.ShowDialog() == DialogResult.OK)
            {

                if (excelAlbumIdList != null && excelAlbumIdList.Count > 0)
                {

                    bool deger = await ExcelExport.excelExportKaydet(sd.FileName, excelExportAlbumId);
                    if (deger)
                    {
                        MessageBox.Show("Excel dosyası oluşturuldu");

                    }

                }

            }
        }

        private async void txtArananVideoAdi_TextChanged(object sender, EventArgs e)
        {
            if (txtArananVideoAdi.Text.Length > 5)
            {
                service = new ServiceAlbum();
                var result = await service.GetVideoId(txtArananVideoAdi.Text);

                txtDegisecekVideoAdi.Text = result.Name;
                lblVideoId.Text = result.Id.ToString(); ;
                VideoId = result.Id.Value;
            }
        }

        private async void btnVideoIsmiDegistir_Click(object sender, EventArgs e)
        {
            service = new ServiceAlbum();
            service.UpdateVideoMetadata(VideoId, txtDegisecekVideoAdi.Text);
        }

        private async void btnManuleDomainAdd_Click(object sender, EventArgs e)
        {
            vimeoManager = new VimeoManager();
            service = new ServiceAlbum();


            if (String.IsNullOrEmpty(cmbDomainName.Text) || String.IsNullOrEmpty(txtManuelDomainAdd.Text))
            {
                MessageBox.Show("Lütfen domain ve video isimi giriniz!");
                return;
            }


                    char start = '"'; char end = '"';
                    string aranan = string.Concat(start, txtManuelDomainAdd.Text, end).ToString();
                    var result = await service.GetVideoGertir(aranan);
                    if (result.Count != 0)
                    {

                        foreach (var item1 in result)
                        {

                            bool result1 = await vimeoManager.VideoDomainIzniEkle(item1.VideoId, cmbDomainName.Text);
                            if (result1)
                            {
                                lbxDomainEkle.Items.Add($"{item1.VideoName}/ isimli videoya   {cmbDomainName.Text} domain izni eklendi");
                            }
                            else
                            {
                                lbxDomainEkle.Items.Add($"{item1.VideoName}/ isimli videoya   {cmbDomainName.Text} domain izni eklenemedi --------");
                            }
                        }


                    }
                    else
                    {
                        lbxDomainEkle.Items.Add($"{txtManuelDomainAdd.Text} isimli videolar bulunamadı");
                    }

             

                MessageBox.Show("Vimeo domain oluşturma işlem tamamlandı");
               txtManuelDomainAdd.Text = "";
          

        }

        private async void btnManuelAlbumUpload_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtManuelAlbumUpload.Text))
            {
                MessageBox.Show("Lütfen video isimi giriniz!");
                return;
            }

            btnManuelAlbumUpload.Enabled = false;

            service = new ServiceAlbum();

            List<string> videoGrup = new List<string>();
                videoGrup.Add(txtManuelAlbumUpload.Text);

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
                            listBox1.Items.Add($"{item}/ videoları vimeoda  eksik. Excel : {videoTestSayisi.Count} Vimeo : {result.Count}");
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
                                        listBox1.Items.Add($"{item1.VideoName}/{item1.VideoId}/ Video hatalı ----------------------");
                                    }
                                    else if (result1.upload.status == "complete")
                                    {
                                        listBox1.Items.Add($"{item1.VideoName}/{item1.VideoId}/ isimli videonun resmi yok. Ama videosu var");
                                    }
                                }

                            }
                        }

                        bool result2 = await service.AlbumeVideoEkleSevices(albumId, result);
                        if (result2)
                        {
                            listBox2.Items.Add($"{item} isimli albüm oluşturuldu.");
                        }
                    }
                    else
                    {
                        listBox1.Items.Add($"{item} isimli video bulunamadı");
                    }

                }
            }

            MessageBox.Show("Vimeo album oluşturma işlem tamamlandı");

            btnManuelAlbumUpload.Enabled = true;

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                txtManuelAlbumUpload.Visible = true;
                btnManuelAlbumUpload.Visible = true;
            }
            else
            {
                txtManuelAlbumUpload.Visible = false;
                btnManuelAlbumUpload.Visible = false;
            }
        }

        private void chkManualDomainAdd_CheckedChanged(object sender, EventArgs e)
        {
            if (chkManualDomainAdd.Checked)
            {
                txtManuelDomainAdd.Visible = true;
                btnManuleDomainAdd.Visible = true;
                lblManuelDomainAdd.Visible = true;
            }
            else
            {
                txtManuelDomainAdd.Visible = false;
                btnManuleDomainAdd.Visible = false;
                lblManuelDomainAdd.Visible = false;
            }
        }
    }
}
