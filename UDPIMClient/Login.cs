using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UDPIMClient.Socket;
using System.Net;
using KeyboardIdentify;
using Model;

namespace UDPIMClient
{
    public partial class Login : Form
    {

        static Login instance = null;
        Server server = Server.getInstance();
        private  Login()
        {
            InitializeComponent();

            var conn = new OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;data source=" + @"..\..\..\IMDB.mdb");
            access = new Access(conn);
            access.openConn();
            InitializeKeyboardVariable();
        }

        /// <summary>
        /// 登录窗口单例
        /// </summary>
        /// <returns></returns>
        public static Login getInstance()
        {
            if(instance==null)
            {
                instance = new Login();
            }
            return instance;
        }

        private Access access;

        //登录
        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();

            if(string.IsNullOrEmpty(username)||string.IsNullOrEmpty(password))
            {
                MessageBox.Show("请输入完整信息");
                return;
            }

            if (access.SearchUserID(username) < 0)
            {
                MessageBox.Show("用户不存在！");
                return;
            }

            if (password != access.GetUserPassword(username))
            {
                MessageBox.Show("密码错误！", "错误");
                return;
            }

            var userVector = timeline.ToVector();
            var storedVectors = access.FetchKeyboardVectors(username);
            if (!Verifier.Verify(userVector, storedVectors))
            {
                MessageBox.Show("键盘特征非用户本人！", "错误");
                InitializeKeyboardVariable();
                textBox2.Clear();
                textBox2.Focus();
                return;
            }

            access.InsertKeyboardData(username, userVector);

#if KEYBOARD_DEBUG
            MessageBox.Show("OK!");
            InitializeKeyboardVariable();
            return;
#endif

            //构造登录信息
            LoginModel loginModel=new LoginModel();
            loginModel.username=username;
            loginModel.password=password;

            //发送登录信息
            MyMessage loginMsg = new MyMessage();
            loginMsg.from = username;
            loginMsg.to = "server";
            loginMsg.type = "login";
            loginMsg.loginModel = loginModel;

            server.sendMsg(loginMsg, ServerIP.getServerIPEndPoint());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RegisterForm registerForm = new RegisterForm();
            registerForm.Show();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            //加载窗体时启动服务
            server.start();
        }

        public void hide()
        {
            this.Close();
        }

        //键盘特征记录
        private KeyboardTimeline timeline;

        private void InitializeKeyboardVariable()
        {
            timeline = new KeyboardTimeline();
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (int) Keys.Back || e.KeyValue == (int) Keys.Space)
            {
                MessageBox.Show("不允许退格或空格，该次输入无效！", "错误");
                resetButton_Click(this, new EventArgs());
                return;
            }

            //过滤除了字母以及数字的按键
            if (KeyboardTimeline.IsAvailableKey(e.KeyValue))
                return;
            
            timeline.MarkDown(e.KeyValue);
        }

        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            //过滤除了字母以及数字的按键
            if (KeyboardTimeline.IsAvailableKey(e.KeyValue))
                return;

            timeline.MarkUp(e.KeyValue);
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
            textBox2.Focus();
            InitializeKeyboardVariable();
        }
    }
}
