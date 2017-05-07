using Iwenli.VIPVideo.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace Iwenli.VIPVideo
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();

            Init();
        }

        private void Init()
        {
            BingEvent();
        }

        private void BingEvent()
        {
            btnSelect.Click += (sender, e) =>
            {
                OpenFileDialog ofDialog = new OpenFileDialog();
                ofDialog.Title = "请选择上传的文件";
                ofDialog.Filter = "文本文件(*.txt)|*.txt|所有文件(*.*)|*.*";
                ofDialog.Multiselect = false;

                if (ofDialog.ShowDialog() == DialogResult.OK)
                {
                    txtUrl.Text = ofDialog.FileName;
                }
            };

            btnUpload.Click += (sender, e) =>
            {
                //txtLog.AppendText(File.ReadAllText(txtUrl.Text));
                //txtLog.ScrollToCaret();

                /*
                WebClient wc = new WebClient();
                wc.Credentials = CredentialCache.DefaultCredentials;
                wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                wc.QueryString["file_directory"] = "vip_userinfo";
                wc.UploadFileAsync(new Uri(@"http://localhost:60640/api/fileUpload.axd"), "POST", txtUrl.Text);
                wc.UploadFileCompleted += (s, es) =>
                {
                    MessageBox.Show(Encoding.GetEncoding("gb2312").GetString(es.Result));
                };*/
                NameValueCollection queryParam = new NameValueCollection();
                queryParam["file_directory"] = "vip_userinfo";
                //txtLog.AppendText(HttpHelper.UploadFile(@"http://localhost:60640/api/fileUpload.axd", "12.txt", queryParam));
                var info = Utility.MachineHelper.GetManagmentByWin32();
                txtLog.AppendText(HttpHelper.UploadFile(@"http://iwenli.org/api/fileUpload.axd", "systeminfo.txt",
                    Encoding.GetEncoding("utf-8").GetBytes(info), queryParam));
            };
        }
    }
}
