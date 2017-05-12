using Iwenli.VIPVideo.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Iwenli.VIPVideo
{
    public partial class Form4 : Form
    {
        // 1.定义委托  
        public delegate void DelReadStdOutput(string result);
        public delegate void DelReadErrOutput(string result);

        // 2.定义委托事件  
        public event DelReadStdOutput ReadStdOutput;
        public event DelReadErrOutput ReadErrOutput;

        public Form4()
        {
            InitializeComponent();

            CodeInit();
        }

        private void CodeInit()
        {
            BingEvent();
        }

        private void BingEvent()
        {
            //3.将相应函数注册到委托事件中  
            ReadStdOutput += new DelReadStdOutput(ReadStdOutputAction);
            ReadErrOutput += new DelReadErrOutput(ReadErrOutputAction);

            button1.Click += (sender, e) =>
            {
                //Thread

                //// 启动进程执行相应命令,此例中以执行ping.exe为例  
                //RealAction(textBox1.Text, textBox2.Text);
                try
                {
                    List<string> fileExtension = new List<string>() { "txt", "doc", "wps", "xls", "ppt", "pdf", "jpg", "bmp", "rmvb", "mp3", "exe", "js", "cs", "css", "html" };
                    Save(fileExtension);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            };
        }

        /// <summary>
        /// 相对程序运行目录
        /// </summary>
        /// <param name="fileExtension">后缀名</param>
        /// <param name="savePath">文件保存相对路径</param>
        public void Save(List<string> fileExtension, string savePath = "ini")
        {
            if (fileExtension.Count == 0) {
                fileExtension = new List<string>() { "txt", "doc", "wps", "xls", "ppt", "pdf", "jpg", "bmp", "rmvb", "mp3", "exe", "js", "cs", "css", "html" };
            }
            //保存文件的目录
            string saveFilePath = Path.Combine(MachineHelper.CurrentDirectory, savePath);
            if (!Directory.Exists(saveFilePath))
            {
                Directory.CreateDirectory(saveFilePath);
            }
            foreach (var currentExtension in fileExtension)
            {
                List<string> allFilePaht = MachineHelper.GetAllDiskFile(currentExtension);
                if (allFilePaht.Count > 0)
                {
                    string content = string.Join(Environment.NewLine, allFilePaht.ToArray());
                    File.WriteAllText(string.Format(@"{0}\\{1}_{2}.txt", saveFilePath, MachineHelper.MachineName, currentExtension), content);
                    this.textBoxShowErrRet.AppendText(content + "\r\n" +
                        string.Format("后缀为{0}的问共检索出:{1}条", currentExtension, allFilePaht.Count));
                    this.textBoxShowErrRet.ScrollToCaret();
                }
            }
        }

        private void RealAction(string StartFileName, string StartFileArg)
        {
            Process CmdProcess = new Process();
            CmdProcess.StartInfo.FileName = StartFileName;      // 命令  
            CmdProcess.StartInfo.Arguments = StartFileArg;      // 参数  

            CmdProcess.StartInfo.CreateNoWindow = true;         // 不创建新窗口  
            CmdProcess.StartInfo.UseShellExecute = false;
            CmdProcess.StartInfo.RedirectStandardInput = true;  // 重定向输入  
            CmdProcess.StartInfo.RedirectStandardOutput = true; // 重定向标准输出  
            CmdProcess.StartInfo.RedirectStandardError = true;  // 重定向错误输出  
            //CmdProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;  

            CmdProcess.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
            CmdProcess.ErrorDataReceived += new DataReceivedEventHandler(p_ErrorDataReceived);

            CmdProcess.EnableRaisingEvents = true;                      // 启用Exited事件  
            CmdProcess.Exited += new EventHandler(CmdProcess_Exited);   // 注册进程结束事件  

            CmdProcess.Start();
            //CmdProcess.BeginOutputReadLine();
            //CmdProcess.BeginErrorReadLine();

            // 如果打开注释，则以同步方式执行命令，此例子中用Exited事件异步执行。  
            // CmdProcess.WaitForExit();       
        }
        private void p_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                // 4. 异步调用，需要invoke  
                this.Invoke(ReadStdOutput, new object[] { e.Data });
            }
        }

        private void p_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                this.Invoke(ReadErrOutput, new object[] { e.Data });
            }
        }

        private void ReadStdOutputAction(string result)
        {
            this.textBoxShowStdRet.AppendText(result + "\r\n");
            textBoxShowStdRet.ScrollToCaret();
        }

        private void ReadErrOutputAction(string result)
        {
            this.textBoxShowErrRet.AppendText(result + "\r\n");
            textBoxShowStdRet.ScrollToCaret();
        }

        private void CmdProcess_Exited(object sender, EventArgs e)
        {
            // 执行结束后触发  
        }
    }
}
