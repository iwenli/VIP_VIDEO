using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Iwenli.VIPVideo
{
    public partial class Form2 : Form
    {
        Thread t;
        public Form2()
        {
            InitializeComponent();

            t = new Thread(ExecTherad);
            t.Start();
            t.IsBackground = true;
        }

        public void SetText(string text)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    SetText(text);
                }));
                return;
            }
            this.richTextBox1.AppendText(text);
            this.richTextBox1.AppendText(Environment.NewLine);
            richTextBox1.ScrollToCaret();
        }


        public void ExecTherad() {
            SetText(Utility.MachineHelper.GetManagmentByWin32());
            SetText(Utility.MachineHelper.GetManagmentByCPU());
            SetText(Utility.MachineHelper.GetManagmentByDisk());
            SetText(Utility.MachineHelper.GetManagmentByPcSys());
        }
    }
}
