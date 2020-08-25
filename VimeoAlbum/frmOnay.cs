using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VimeoAlbum.Models;

namespace VimeoAlbum
{
    public partial class frmOnay : Form
    {
        public frmOnay()
        {
            InitializeComponent();
        }

        private void btnOnay_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtOnay.Text))
            {
                MessageBox.Show("Lütfen kod giriniz");
            }
            else if(txtOnay.Text =="1517")
            {
                StaticAyar.OnayKodu = "OK";
                this.Close();
            }
            else
            {
                StaticAyar.OnayKodu = "";
                this.Close();
            }
        }
    }
}
