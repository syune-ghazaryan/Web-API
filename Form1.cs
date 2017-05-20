using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace FormSender
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Sent image to server by Http web request 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSend_Click(object sender, EventArgs e)
        {
            byte[] bytes = new byte[1024 * 100];

            //Load image and convert it into byte array
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Open Image";
            dlg.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                bytes = File.ReadAllBytes(dlg.FileName);
                txtImageName.Text = Path.GetFileName(dlg.FileName);
            }

            //creating url and send stream of bytes by POST method
            txtUrl.Text = "http://localhost:64852/api/Index/";
            string url = txtUrl.Text;
            string commandUrl = url + "addimage?name=" + Path.GetFileName(dlg.FileName);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(commandUrl);
            request.Method = "POST";
            Stream strReq = request.GetRequestStream();
            request.ContentType = "application/octet-stream";
            strReq.Write(bytes, 0, bytes.Length);
            WebResponse response = request.GetResponse();
        }
       
        /// <summary>
        /// get image from server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGet_Click(object sender, EventArgs e)
        {
            txtUrl.Text = "http://localhost:64852/api/Index/";
            string url = txtUrl.Text;
            string commandUrl = url + "getimage?name=" + txtImageName.Text;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(commandUrl);
            request.Method = "GET";
            WebResponse response = request.GetResponse();
            //get response stream convert to file stream
            using (Stream stream = response.GetResponseStream())
            {
                MemoryStream ms = new MemoryStream();
                stream.CopyTo(ms);
                ms.Seek(0, SeekOrigin.Begin);
                ms.Position = 0;
                string receivePath = @"C:\Users\Syune\Desktop\images\Client Receive\" + txtImageName.Text;
                using (Stream s = File.Create(receivePath))
                {
                    ms.CopyTo(s);
                }

            }


        }
    
    }
}
