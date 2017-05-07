using Iwenli.VIPVideo.Utility;
using System;
using System.Threading;
using System.Windows.Forms;

namespace Iwenli.VIPVideo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Inin();
            
        }

        private void Inin()
        {
            this.comboBox1.SelectedIndex = 0;
            BingEvent();
        }

        private void BingEvent()
        {
            this.serbtn.Click += (sender, e) => {
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

                        //text2 = System.Web.HttpUtility.UrlEncode(text2, System.Text.Encoding.UTF8);
                        //var url = http.GetPage("http://iwenli.org/vip.aspx?_=" + text2, "http://iwenli.org/");
                        this.webBrowser1.Navigate("http://api.47ks.com/webcloud/?v=" + text2);
                    }
                }

                this.serbtn.Enabled = true;
            };
        }
    }
}

