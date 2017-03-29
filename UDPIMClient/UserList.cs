using Model;
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
    public partial class UserList : Form
    {
        static UserList instance = null;
        private UserList()
        {
            InitializeComponent();
            timer1.Start();
        }

        /// <summary>
        ///单例
        /// </summary>
        /// <returns></returns>
        public static UserList getInstance()
        {
            if(instance==null)
            {
                instance = new UserList();
            }
            return instance;
        }


        TreeNode root = new TreeNode();
        private void UserList_Load(object sender, EventArgs e)
        {
            Console.WriteLine("load");
            root.Text = "在线用户";
            treeView_user.Nodes.Add(root);

            refreshTree();
        }



        private void treeView_user_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            Console.WriteLine("选中用户：" + e.Node.Text);
            //显示聊天界面

            if(e.Node.Text=="在线用户")
            {
                return;
            }

            MyIPEndPoint ip;
            if (OnlineUsers.getInstance().users.TryGetValue(e.Node.Text, out ip))
            {
                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(ip.ip), ip.port);
                //如果用户在线
                ChatForm chatForm = new ChatForm();
                chatForm.username = e.Node.Text;
                chatForm.remoteIPEndPoint = ipEndPoint;
                chatForm.Show();
            }
            else
            {
                MessageBox.Show("用户离线");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            refreshTree();
        }

        private void refreshTree()
        {
            root.Nodes.Clear();
            //填充在线用户信息

            Console.WriteLine("用户列表刷新");
            foreach (KeyValuePair<string, MyIPEndPoint> keyValuePair in OnlineUsers.getInstance().users)
            {
                TreeNode node = new TreeNode();
                node.Text = keyValuePair.Key;
                root.Nodes.Add(node);
            }
        }
    }
}
