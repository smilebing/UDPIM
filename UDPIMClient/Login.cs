using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UDPIMClient.Socket;
using System.Net;
using Model;
namespace UDPIMClient
{
    public partial class Login : Form
    {
        Server server = Server.getInstance();
        public Login()
        {
            InitializeComponent();
        }


        //登录
        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();

            if(string.IsNullOrEmpty(username)||string.IsNullOrEmpty(password))
            {
                MessageBox.Show("请输入完整信息");
            }


            LoginModel loginModel=new LoginModel();
            loginModel.username=username;
            loginModel.password=password;

            //发送登录信息
            MyMessage loginMsg = new MyMessage();
            loginMsg.from = username;
            loginMsg.to = "server";
            loginMsg.type = "login";
            loginMsg.loginModel = loginModel;

            server.start();
            server.sendMsg(loginMsg,ser)
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RegisterForm registerForm = new RegisterForm();
            registerForm.Show();
        }
    }
}
