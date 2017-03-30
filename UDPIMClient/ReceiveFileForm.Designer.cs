namespace UDPIMClient
{

  partial class ReceiveFileForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tbLocalPort = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.fileTansfersContainer = new CSharpWin.FileTansfersContainer();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbLocalPort
            // 
            this.tbLocalPort.Location = new System.Drawing.Point(95, 7);
            this.tbLocalPort.Name = "tbLocalPort";
            this.tbLocalPort.Size = new System.Drawing.Size(100, 21);
            this.tbLocalPort.TabIndex = 12;
            this.tbLocalPort.Text = "10002";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(19, 40);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "开始监听";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 11;
            this.label1.Text = "本机监听端口";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.tbLocalPort);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 206);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(502, 74);
            this.panel1.TabIndex = 14;
            // 
            // fileTansfersContainer
            // 
            this.fileTansfersContainer.AutoScroll = true;
            this.fileTansfersContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(250)))), ((int)(((byte)(249)))));
            this.fileTansfersContainer.Dock = System.Windows.Forms.DockStyle.Right;
            this.fileTansfersContainer.Location = new System.Drawing.Point(242, 0);
            this.fileTansfersContainer.Name = "fileTansfersContainer";
            this.fileTansfersContainer.Size = new System.Drawing.Size(260, 206);
            this.fileTansfersContainer.TabIndex = 15;
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.HorizontalScrollbar = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(0, 0);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(242, 206);
            this.listBox1.TabIndex = 17;
            // 
            // ReceiveFileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(502, 280);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.fileTansfersContainer);
            this.Controls.Add(this.panel1);
            this.Name = "ReceiveFileForm";
            this.Text = "接收文件";
            this.Load += new System.EventHandler(this.ReceiveFileForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox tbLocalPort;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private CSharpWin.FileTansfersContainer fileTansfersContainer;
        private System.Windows.Forms.ListBox listBox1;

    }
}