using CSharpWin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UDPIMClient
{
    public partial class SendFileForm : Form,IDisposable
    {
        public static int localPort = 10002;
        public static int remotePort = 10003;
        public static string remoteIp = "127.0.0.1";
        #region Fields

        private UdpSendFile udpSendFile;


        public string remoteUsername
        {
            get;
            set;
        }

        public IPEndPoint remoteIPEndPoint
        {
            get;
            set;
        }
        #endregion

        #region Constructor

        /// <summary>
        /// 私有化构造函数，实现单例
        /// </summary>
        public SendFileForm()
        {
            InitializeComponent();

        }

   


        #endregion

        #region Control Events

        private void button1_Click(
            object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    SendFileManager sendFileManager = new SendFileManager(
                        ofd.FileName);
                    if (udpSendFile.CanSend(sendFileManager))
                    {
                        FileTransfersItem item = fileTansfersContainer.AddItem(
                            sendFileManager.MD5,
                            "发送文件",
                            sendFileManager.Name,
                            Icon.ExtractAssociatedIcon(ofd.FileName).ToBitmap(),
                            sendFileManager.Length,
                            FileTransfersItemStyle.Send);
                        item.CancelButtonClick += new EventHandler(ItemCancelButtonClick);
                        item.Tag = sendFileManager;
                        sendFileManager.Tag = item;
                        udpSendFile.SendFile(sendFileManager, item.Image);
                    }
                    else
                    {
                        MessageBox.Show("文件正在发送，不能发送重复的文件。");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void ItemCancelButtonClick(object sender, EventArgs e)
        {
            FileTransfersItem item =
                sender as FileTransfersItem;
            SendFileManager sendFileManager =
                       item.Tag as SendFileManager;
            udpSendFile.CancelSend(sendFileManager.MD5);

            fileTansfersContainer.RemoveItem(item);
            AppendLog(string.Format(
                "取消发送文件 {0} 。",
                sendFileManager.Name), true);
        }

        //private void button2_Click(object sender, EventArgs e)
        //{
        //    udpSendFile = new UdpSendFile(
        //        tbRemoteIP.Text,
        //        int.Parse(tbRemotePort.Text),
        //        int.Parse(tbLocalPort.Text));
        //    sendFile.Log += new TraFransfersFileLogEventHandler(SendFileLog);
        //    udpSendFile.FileSendBuffer +=
        //        new FileSendBufferEventHandler(FileSendBuffer);
        //    udpSendFile.FileSendAccept +=
        //        new FileSendEventHandler(FileSendAccept);
        //    udpSendFile.FileSendRefuse +=
        //        new FileSendEventHandler(FileSendRefuse);
        //    udpSendFile.FileSendCancel += new FileSendEventHandler(FileSendCancel);
        //    udpSendFile.FileSendComplete +=
        //        new FileSendEventHandler(FileSendComplete);
        //    udpSendFile.Start();
        //    AppendLog(string.Format(
        //        "开始侦听，端口：{0}", udpSendFile.Port), false);
        //}

        #endregion

        #region SendFile Events

        private void FileSendCancel(object sender, FileSendEventArgs e)
        {
            FileTransfersItem item =
                e.SendFileManager.Tag as FileTransfersItem;
            if (item != null)
            {
                BeginInvoke(new MethodInvoker(delegate()
                {
                    fileTansfersContainer.RemoveItem(item);
                    item.Dispose();
                }));
            }

            AppendLog(string.Format(
                "对方取消接收文件 {0} 。",
                e.SendFileManager.Name), true);
        }

        private void FileSendComplete(object sender, FileSendEventArgs e)
        {
            FileTransfersItem item =
                e.SendFileManager.Tag as FileTransfersItem;
            if (item != null)
            {
                BeginInvoke(new MethodInvoker(delegate()
                {
                    fileTansfersContainer.RemoveItem(item);
                    item.Dispose();
                }));
            }

            AppendLog(string.Format(
                "文件 {0} 发送完成。",
                e.SendFileManager.Name), true);
        }

        private void FileSendRefuse(object sender, FileSendEventArgs e)
        {
            FileTransfersItem item =
                e.SendFileManager.Tag as FileTransfersItem;
            if (item != null)
            {
                BeginInvoke(new MethodInvoker(delegate()
                {
                    fileTansfersContainer.RemoveItem(item);
                    item.Dispose();
                }));
            }

            AppendLog(string.Format(
                "对方拒绝接收文件 {0} 。",
                e.SendFileManager.Name), true);
        }

        private void FileSendAccept(
            object sender, FileSendEventArgs e)
        {
            FileTransfersItem item =
                e.SendFileManager.Tag as FileTransfersItem;
            if (item != null)
            {
                //BeginInvoke(new MethodInvoker(delegate()
                //{
                item.Start();
                //}));
            }

            AppendLog(string.Format(
                "对方同意接收文件 {0}。",
                e.SendFileManager.Name), true);
        }

        private void FileSendBuffer(
            object sender, FileSendBufferEventArgs e)
        {
            FileTransfersItem item =
                e.SendFileManager.Tag as FileTransfersItem;
            if (item != null)
            {
                BeginInvoke(new MethodInvoker(delegate()
                {
                    item.TotalTransfersSize += e.Size;
                }));
            }
        }

        #endregion

        #region Help Methods

        private void AppendLog(string text, bool async)
        {
            if (async)
            {
                Invoke(new MethodInvoker(delegate()
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
            if (udpSendFile != null)
            {
                udpSendFile.Dispose();
            }
        }

        #endregion

        private void SendFileForm_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(remoteUsername))
            {
                return;
            }

            if (remoteIPEndPoint == null)
            {
                return;
            }

            udpSendFile = new UdpSendFile(
                remoteIPEndPoint.Address.ToString(),
                10002,
                10002);
            //sendFile.Log += new TraFransfersFileLogEventHandler(SendFileLog);
            udpSendFile.FileSendBuffer +=
                new FileSendBufferEventHandler(FileSendBuffer);
            udpSendFile.FileSendAccept +=
                new FileSendEventHandler(FileSendAccept);
            udpSendFile.FileSendRefuse +=
                new FileSendEventHandler(FileSendRefuse);
            udpSendFile.FileSendCancel += new FileSendEventHandler(FileSendCancel);
            udpSendFile.FileSendComplete +=
                new FileSendEventHandler(FileSendComplete);
            udpSendFile.Start();
            AppendLog(string.Format(
                "开始侦听，端口：{0}", udpSendFile.Port), false);


        }

        private void SendFileForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
        }
    }

}
