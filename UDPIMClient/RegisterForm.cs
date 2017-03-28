using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UDPIMClient
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(textBox1.Text.Trim()))
            {
                MessageBox.Show("请输入用户名");
                return;
            }
            int  pwdLength=0;
            try
            {
                pwdLength= Convert.ToInt32( textBox2.Text.Trim());

                if(pwdLength<=10||pwdLength>=26)
                {
                    MessageBox.Show("请输入正确的密码长度");
                    return;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("请输入正确的密码长度");
                return;
            }

            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = true;
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine("key down");
        }

   

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_KeyUp(object sender, KeyEventArgs e)
        {
            Console.WriteLine("key up");
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            Console.WriteLine("key press");
        }
    }
}
