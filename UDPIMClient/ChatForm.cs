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
using UDPIMClient.Socket;
using UDPIMClient.Model;
using Model;

namespace UDPIMClient
{
    public partial class ChatForm : Form
    {
        public ChatForm()
        {
            InitializeComponent();
        }

        Server server = Server.getInstance();

        public string username { get; set; }
        public IPEndPoint remoteIPEndPoint { get; set; }

        

        /// <summary>
        /// 发送信息给好友
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            string msg = richTextBox2.Text.Trim();
            MyMessage msgObject = new MyMessage();
            msgObject.from = Server.getInstance().currentUsername;
            msgObject.to = username;
            msgObject.type = "chat";
            msgObject.content = msg;
            
            
            server.sendMsg(msgObject, remoteIPEndPoint);

            //添加消息记录
            richTextBox1.AppendText(Server.getInstance().currentUsername + " " + DateTime.Now.ToLongTimeString().ToString() + "\n");
            richTextBox1.AppendText("    "+msg + "\n");
            richTextBox2.Text = "";
            
        }


        public void addTips(String msg)
        {
            richTextBox1.AppendText(username+ " "+DateTime.Now.ToLongTimeString().ToString()+"\n");
            richTextBox1.AppendText("    "+msg+"\n");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Server server = Server.getInstance();

            //发送发送文件的通知
            MyMessage msg = new MyMessage();
            msg.from = server.currentUsername;
            msg.to = username;
            msg.type = "file";
            msg.content = "send";

            server.sendMsg(msg,remoteIPEndPoint);
        }

        private void ChatForm_Load(object sender, EventArgs e)
        {
            this.Text = username;
        }
    }
}
