using CSharpWin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Model;

namespace UDPIMClient
{
    public partial class ReceiveFileForm : Form
    {
        public static string localPort = "10002";

        #region Fields

        private UdpReceiveFile udpReceiveFile;

        private Color _baseColor = Color.DarkGoldenrod;
        private Color _borderColor = Color.FromArgb(64, 64, 0);
        private Color _progressBarBarColor = Color.Gold;
        private Color _progressBarBorderColor = Color.Olive;
        private Color _progressBarTextColor = Color.Olive;

        #endregion

        #region Constructor

        public ReceiveFileForm()
        {
            InitializeComponent();

        }

        #endregion

        #region udpReceiveFile Events

        private void FileReceiveCancel(
            object sender, FileReceiveEventArgs e)
        {
            string md5 = string.Empty;
            if (e.ReceiveFileManager != null)
            {
                md5 = e.ReceiveFileManager.MD5;
            }
            else
            {
                md5 = e.Tag.ToString();
            }

            FileTransfersItem item = fileTansfersContainer.Search(md5);

            BeginInvoke(new MethodInvoker(delegate()
            {
                fileTansfersContainer.RemoveItem(item);
            }));

            AppendLog(string.Format(
                "对方取消发送文件文件 {0} 。",
                item.FileName), true);
        }

        private void FileReceiveComplete(
            object sender, FileReceiveEventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate()
            {
                fileTansfersContainer.RemoveItem(e.ReceiveFileManager.MD5);
            }));

            AppendLog(string.Format(
                "文件 {0} 接收完成，MD5 校验: {1}。",
                e.ReceiveFileManager.Name, e.ReceiveFileManager.Success), true);
        }

        private void FileReceiveBuffer(
            object sender, FileReceiveBufferEventArgs e)
        {
            FileTransfersItem item = fileTansfersContainer.Search(
                e.ReceiveFileManager.MD5);
            if (item != null)
            {
                BeginInvoke(new MethodInvoker(delegate()
                {
                    item.TotalTransfersSize += e.Size;
                }));
            }
        }

        private void RequestSendFile(
            object sender, RequestSendFileEventArgs e)
        {
            TraFransfersFileStart traFransfersFileStart = e.TraFransfersFileStart;
            BeginInvoke(new MethodInvoker(delegate()
            {
                FileTransfersItem item = fileTansfersContainer.AddItem(
                    traFransfersFileStart.MD5,
                    "接收文件",
                    traFransfersFileStart.FileName,
                    traFransfersFileStart.Image,
                    traFransfersFileStart.Length,
                    FileTransfersItemStyle.ReadyReceive);

                item.BaseColor = _baseColor;
                item.BorderColor = _borderColor;
                item.ProgressBarBarColor = _progressBarBarColor;
                item.ProgressBarBorderColor = _progressBarBorderColor;
                item.ProgressBarTextColor = _progressBarTextColor;

                item.Tag = e;
                item.SaveButtonClick += new EventHandler(ItemSaveButtonClick);
                item.SaveToButtonClick += new EventHandler(ItemSaveToButtonClick);
                item.RefuseButtonClick += new EventHandler(ItemRefuseButtonClick);
            }));

            AppendLog(string.Format(
                "请求发送文件 {0}。",
                traFransfersFileStart.FileName), true);
        }

        #endregion

        #region Control Events

 

        private void ItemRefuseButtonClick(object sender, EventArgs e)
        {
            FileTransfersItem item = sender as FileTransfersItem;
            RequestSendFileEventArgs rse = item.Tag as RequestSendFileEventArgs;
            rse.Cancel = true;
            fileTansfersContainer.RemoveItem(item);
            item.Dispose();
            AppendLog(string.Format(
                "拒绝接收文件 {0}。",
                rse.TraFransfersFileStart.FileName), false);
            udpReceiveFile.AcceptReceive(rse);
        }

        private void ItemSaveToButtonClick(object sender, EventArgs e)
        {
            FileTransfersItem item = sender as FileTransfersItem;
            RequestSendFileEventArgs rse = item.Tag as RequestSendFileEventArgs;
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                rse.Path = fbd.SelectedPath;
                AppendLog(string.Format(
                    "同意接收文件 {0}。",
                    rse.TraFransfersFileStart.FileName), false);
                ControlTag tag = new ControlTag(
                    rse.TraFransfersFileStart.MD5,
                    rse.TraFransfersFileStart.FileName,
                    rse.RemoteIP);
                item.Tag = tag;
                item.Style = FileTransfersItemStyle.Receive;
                item.CancelButtonClick += new EventHandler(ItemCancelButtonClick);
                item.Start();

                udpReceiveFile.AcceptReceive(rse);
            }
        }

        private void ItemSaveButtonClick(object sender, EventArgs e)
        {
            FileTransfersItem item = sender as FileTransfersItem;
            RequestSendFileEventArgs rse = item.Tag as RequestSendFileEventArgs;

            rse.Path = Application.StartupPath;
            AppendLog(string.Format(
                   "同意接收文件 {0}。",
                   rse.TraFransfersFileStart.FileName), false);
            ControlTag tag = new ControlTag(
                rse.TraFransfersFileStart.MD5,
                rse.TraFransfersFileStart.FileName,
                rse.RemoteIP);
            item.Tag = tag;
            item.Style = FileTransfersItemStyle.Receive;
            item.CancelButtonClick += new EventHandler(ItemCancelButtonClick);
            item.Start();

            udpReceiveFile.AcceptReceive(rse);
        }

        private void ItemCancelButtonClick(object sender, EventArgs e)
        {
            FileTransfersItem item = sender as FileTransfersItem;
            ControlTag tag = item.Tag as ControlTag;
            udpReceiveFile.CancelReceive(tag.MD5, tag.RemoteIP);
            fileTansfersContainer.RemoveItem(item);
            item.Dispose();
            AppendLog(string.Format(
               "取消接收文件 {0}。",
               tag.FileName), false);
        }

        #endregion

        #region Help Methods

        private void AppendLog(string text, bool async)
        {
            if (async)
            {
                BeginInvoke(new MethodInvoker(delegate()
                {
                    int index = listBox1.Items.Add(text);
                    listBox1.SelectedIndex = index;
                }));
            }
            else
            {
                int index = listBox1.Items.Add(text);
                listBox1.SelectedIndex = index;
            }
        }

        #endregion

        #region Override Methods

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (udpReceiveFile != null)
            {
                udpReceiveFile.Dispose();
            }
        }

        #endregion

        /// <summary>
        /// 加载窗体时就进行监听
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReceiveFileForm_Load(object sender, EventArgs e)
        {
            udpReceiveFile = new UdpReceiveFile(
              int.Parse(localPort));
            udpReceiveFile.RequestSendFile +=
                new RequestSendFileEventHandler(RequestSendFile);
            udpReceiveFile.FileReceiveBuffer +=
                new FileReceiveBufferEventHandler(FileReceiveBuffer);
            udpReceiveFile.FileReceiveComplete +=
                new FileReceiveEventHandler(FileReceiveComplete);
            udpReceiveFile.FileReceiveCancel +=
                new FileReceiveEventHandler(FileReceiveCancel);
            udpReceiveFile.Start();
//            AppendLog(string.Format(
//                "开始侦听，端口：{0}", udpReceiveFile.Port), false);
        }
    }
}
