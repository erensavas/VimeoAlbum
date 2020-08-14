using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VimeoAlbum.Models;
using VimeoAlbum.Repository;

namespace VimeoAlbum
{
    public partial class frmKitaplar : Form
    {
        UrunPostModel model = new UrunPostModel();
        Urunler urun = new Urunler();

        public frmKitaplar()
        {
           
        }

        public frmKitaplar(UrunPostModel _model)
        {
            InitializeComponent();
            model = _model;
        }

        private void frmKitaplar_Load(object sender, EventArgs e)
        {
            label1.Text = $"Gelen ID : {StaticAyar.SelectedNode}";

            if (model.urunadi != null)
            {
                txtKitapAdi.Text = model.urunadi;
                txtSiraNo.Text = model.sira.ToString();
                txtBarkodNo.Text = model.barkodno.ToString();
                txtAlbumNo.Text = model.albumkodu.ToString();
                txtUstUrunId.Text = model.uid.ToString();
                lblUrunID.Text = model.id.ToString();

                btnEkle.Text = "Güncelle";
            }
            else
            {
                btnEkle.Text = "Ekle";
            }
           

        }

        private async void btnEkle_Click(object sender, EventArgs e)
        {
            if (btnEkle.Text == "Ekle")
            {
              
                if (string.IsNullOrEmpty(txtKitapAdi.Text) || string.IsNullOrEmpty(txtSiraNo.Text))
                {
                    MessageBox.Show("Lütfen alanları doldurunuz!");
                }
                else
                {
                  
                    model.urunadi = txtKitapAdi.Text;
                    model.sira = Convert.ToInt32(txtSiraNo.Text);

                    if (!string.IsNullOrEmpty(txtAlbumNo.Text))
                    {
                        model.albumkodu = Convert.ToInt64(txtAlbumNo.Text);
                    }
                    if (!string.IsNullOrEmpty(txtBarkodNo.Text))
                    {
                        model.barkodno = Convert.ToInt64(txtBarkodNo.Text);
                    }

                    if (cxbAnaUrun.Checked)
                    {
                        model.uid = 0;
                    }

                    int result = await Ekle(model);
                    if (result == 1)
                        MessageBox.Show("Kitap eklenemedi");
                    else
                        MessageBox.Show("Kitap eklendi");

                    this.Close();
                }
            }
            else if (btnEkle.Text == "Güncelle")
            {
                if (string.IsNullOrEmpty(txtKitapAdi.Text) || string.IsNullOrEmpty(txtSiraNo.Text))
                {
                    MessageBox.Show("Lütfen alanları doldurunuz!");
                }
                else
                {
                    model.urunadi = txtKitapAdi.Text;
                    model.sira = Convert.ToInt32(txtSiraNo.Text);

                    if (!string.IsNullOrEmpty(txtAlbumNo.Text))
                    {
                        model.albumkodu = Convert.ToInt64(txtAlbumNo.Text);
                    }
                    if (!string.IsNullOrEmpty(txtBarkodNo.Text))
                    {
                        model.barkodno = Convert.ToInt64(txtBarkodNo.Text);
                    }
                    if (!string.IsNullOrEmpty(txtUstUrunId.Text))
                    {
                        model.uid = Convert.ToInt32(txtUstUrunId.Text);
                    }

                    var result = await UrunDuzenle(model);
                    if (result.durum==true)
                        MessageBox.Show("Kayıt güncellendi");
                    else
                        MessageBox.Show("kayıt güncellenemedi "+result.mesaj);

                    this.Close();
                }
            }

          
           
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

        public async Task<UrunPostResponseModel> UrunDuzenle(UrunPostModel model)
        {
            var properties = from p in model.GetType().GetProperties()
                             where p.GetValue(model, null) != null
                             select p.Name + "=" + WebUtility.UrlEncode(p.GetValue(model, null).ToString());

            // queryString will be set to "Id=1&State=26&Prefix=f&Index=oo"                  
            string queryString = String.Join("&", properties.ToArray());


            var result = await urun.UrunApiPost("duzenle", queryString);
            return result;
        }

       
    }
}
