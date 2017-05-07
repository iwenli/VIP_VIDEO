using System;
using System.Windows.Forms;

namespace Iwenli.VIPVideo
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //www.iwenli.org/api/log.axd?file_directory=vip_video&content=hello
            Application.Run(new Form1());
        }
    }
}
