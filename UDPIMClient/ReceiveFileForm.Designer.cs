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
            this.fileTansfersContainer = new CSharpWin.FileTansfersContainer();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // fileTansfersContainer
            // 
            this.fileTansfersContainer.AutoScroll = true;
            this.fileTansfersContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(250)))), ((int)(((byte)(249)))));
            this.fileTansfersContainer.Dock = System.Windows.Forms.DockStyle.Right;
            this.fileTansfersContainer.Location = new System.Drawing.Point(242, 0);
            this.fileTansfersContainer.Name = "fileTansfersContainer";
            this.fileTansfersContainer.Size = new System.Drawing.Size(260, 280);
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
            this.listBox1.Size = new System.Drawing.Size(242, 280);
            this.listBox1.TabIndex = 17;
            // 
            // ReceiveFileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(502, 280);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.fileTansfersContainer);
            this.Name = "ReceiveFileForm";
            this.Text = "接收文件";
            this.Load += new System.EventHandler(this.ReceiveFileForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private CSharpWin.FileTansfersContainer fileTansfersContainer;
        private System.Windows.Forms.ListBox listBox1;

    }
}