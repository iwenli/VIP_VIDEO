namespace Iwenli.VIPVideo
{
    partial class Form4
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBoxShowErrRet = new System.Windows.Forms.RichTextBox();
            this.textBoxShowStdRet = new System.Windows.Forms.RichTextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(592, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(13, 13);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(168, 21);
            this.textBox1.TabIndex = 1;
            // 
            // textBoxShowErrRet
            // 
            this.textBoxShowErrRet.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBoxShowErrRet.Location = new System.Drawing.Point(0, 283);
            this.textBoxShowErrRet.Name = "textBoxShowErrRet";
            this.textBoxShowErrRet.Size = new System.Drawing.Size(679, 220);
            this.textBoxShowErrRet.TabIndex = 2;
            this.textBoxShowErrRet.Text = "";
            // 
            // textBoxShowStdRet
            // 
            this.textBoxShowStdRet.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBoxShowStdRet.Location = new System.Drawing.Point(0, 56);
            this.textBoxShowStdRet.Name = "textBoxShowStdRet";
            this.textBoxShowStdRet.Size = new System.Drawing.Size(679, 227);
            this.textBoxShowStdRet.TabIndex = 3;
            this.textBoxShowStdRet.Text = "";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(208, 12);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(168, 21);
            this.textBox2.TabIndex = 1;
            // 
            // Form4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(679, 503);
            this.Controls.Add(this.textBoxShowStdRet);
            this.Controls.Add(this.textBoxShowErrRet);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Name = "Form4";
            this.Text = "遍历文件";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.RichTextBox textBoxShowErrRet;
        private System.Windows.Forms.RichTextBox textBoxShowStdRet;
        private System.Windows.Forms.TextBox textBox2;
    }
}