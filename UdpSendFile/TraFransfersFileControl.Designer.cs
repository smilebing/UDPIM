namespace CSharpWin
{
    partial class TraFransfersFileControl
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.comment = new System.Windows.Forms.Label();
            this.fileName = new System.Windows.Forms.Label();
            this.picIcon = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.trafransferInfo = new System.Windows.Forms.Label();
            this.labelSpeed = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.labelReceive = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.labelSave = new System.Windows.Forms.Label();
            this.panel7 = new System.Windows.Forms.Panel();
            this.labelCancel = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.comment);
            this.panel1.Controls.Add(this.fileName);
            this.panel1.Controls.Add(this.picIcon);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(302, 34);
            this.panel1.TabIndex = 4;
            // 
            // comment
            // 
            this.comment.AutoSize = true;
            this.comment.Location = new System.Drawing.Point(43, 1);
            this.comment.Name = "comment";
            this.comment.Size = new System.Drawing.Size(53, 12);
            this.comment.TabIndex = 4;
            this.comment.Text = "SendFile";
            // 
            // fileName
            // 
            this.fileName.AutoSize = true;
            this.fileName.Location = new System.Drawing.Point(43, 21);
            this.fileName.Name = "fileName";
            this.fileName.Size = new System.Drawing.Size(35, 12);
            this.fileName.TabIndex = 3;
            this.fileName.Text = "a.txt";
            // 
            // picIcon
            // 
            this.picIcon.Location = new System.Drawing.Point(3, 1);
            this.picIcon.Name = "picIcon";
            this.picIcon.Size = new System.Drawing.Size(32, 32);
            this.picIcon.TabIndex = 2;
            this.picIcon.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.progressBar);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 34);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(302, 18);
            this.panel2.TabIndex = 5;
            // 
            // progressBar
            // 
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar.Location = new System.Drawing.Point(0, 8);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(302, 10);
            this.progressBar.TabIndex = 3;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.panel3);
            this.panel4.Controls.Add(this.labelSpeed);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 52);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(302, 18);
            this.panel4.TabIndex = 10;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.trafransferInfo);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(96, 0);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.panel3.Size = new System.Drawing.Size(206, 18);
            this.panel3.TabIndex = 10;
            // 
            // trafransferInfo
            // 
            this.trafransferInfo.AutoSize = true;
            this.trafransferInfo.Dock = System.Windows.Forms.DockStyle.Right;
            this.trafransferInfo.Location = new System.Drawing.Point(189, 3);
            this.trafransferInfo.Name = "trafransferInfo";
            this.trafransferInfo.Size = new System.Drawing.Size(17, 12);
            this.trafransferInfo.TabIndex = 5;
            this.trafransferInfo.Text = "0k";
            this.trafransferInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelSpeed
            // 
            this.labelSpeed.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelSpeed.Location = new System.Drawing.Point(0, 0);
            this.labelSpeed.Name = "labelSpeed";
            this.labelSpeed.Size = new System.Drawing.Size(96, 18);
            this.labelSpeed.TabIndex = 9;
            this.labelSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.labelReceive);
            this.panel5.Controls.Add(this.panel6);
            this.panel5.Controls.Add(this.labelSave);
            this.panel5.Controls.Add(this.panel7);
            this.panel5.Controls.Add(this.labelCancel);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(0, 70);
            this.panel5.Name = "panel5";
            this.panel5.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.panel5.Size = new System.Drawing.Size(302, 20);
            this.panel5.TabIndex = 11;
            // 
            // labelReceive
            // 
            this.labelReceive.AutoSize = true;
            this.labelReceive.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelReceive.ForeColor = System.Drawing.Color.Blue;
            this.labelReceive.Location = new System.Drawing.Point(155, 3);
            this.labelReceive.Name = "labelReceive";
            this.labelReceive.Size = new System.Drawing.Size(29, 12);
            this.labelReceive.TabIndex = 4;
            this.labelReceive.Text = "接收";
            // 
            // panel6
            // 
            this.panel6.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel6.Location = new System.Drawing.Point(184, 3);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(24, 17);
            this.panel6.TabIndex = 3;
            // 
            // labelSave
            // 
            this.labelSave.AutoSize = true;
            this.labelSave.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelSave.ForeColor = System.Drawing.Color.Blue;
            this.labelSave.Location = new System.Drawing.Point(208, 3);
            this.labelSave.Name = "labelSave";
            this.labelSave.Size = new System.Drawing.Size(41, 12);
            this.labelSave.TabIndex = 2;
            this.labelSave.Text = "另存为";
            // 
            // panel7
            // 
            this.panel7.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel7.Location = new System.Drawing.Point(249, 3);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(24, 17);
            this.panel7.TabIndex = 1;
            // 
            // labelCancel
            // 
            this.labelCancel.AutoSize = true;
            this.labelCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelCancel.ForeColor = System.Drawing.Color.Blue;
            this.labelCancel.Location = new System.Drawing.Point(273, 3);
            this.labelCancel.Name = "labelCancel";
            this.labelCancel.Size = new System.Drawing.Size(29, 12);
            this.labelCancel.TabIndex = 0;
            this.labelCancel.Text = "拒绝";
            // 
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
            // 
            // TraFransfersFileControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "TraFransfersFileControl";
            this.Size = new System.Drawing.Size(302, 90);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label comment;
        private System.Windows.Forms.Label fileName;
        private System.Windows.Forms.PictureBox picIcon;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label trafransferInfo;
        private System.Windows.Forms.Label labelSpeed;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label labelReceive;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label labelSave;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label labelCancel;
        private System.Windows.Forms.ToolTip toolTip;




    }
}
