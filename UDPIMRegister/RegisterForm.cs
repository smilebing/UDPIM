using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using KeyboardIdentify;
using Model;

namespace UDPIMRegister
{
    public partial class RegisterForm : Form
    {
        Access access;
        OleDbConnection conn;
        bool recordStarted = false;

        public RegisterForm()
        {
            conn = new OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;data source=" + Application.StartupPath + @"\IMDB.mdb");
            access = new Access(conn);
            access.openConn();

            InitializeComponent();
        }

        private void RegisterForm_Load(object sender, EventArgs e) { }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text.Trim()))
            {
                MessageBox.Show("请输入用户名");
                return;
            }
            int pwdLength = 0;
            try
            {
                pwdLength = Convert.ToInt32(textBox2.Text.Trim().Length);

                if (pwdLength <= 10 || pwdLength >= 26)
                {
                    MessageBox.Show("请输入正确的密码长度");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("请输入正确的密码长度");
                return;
            }

            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = true;
            recordStarted = true;
            InitializeKeyboardVariables();
        }

        // 键盘特征记录
        private KeyboardTimeline timeline = new KeyboardTimeline();
        private int recordCounter = 0; //输入次数计数器
        private List<Vector> recordList = null; //在内存中储存输入的特征向量
        private const int MAX_RECORD_REQUIRED = 3;

        private void InitializeKeyboardVariables()
        {
            recordList = new List<Vector>();
            recordCounter = 0;
            timeline = new KeyboardTimeline();
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            //Console.WriteLine("key down");

            //若还未开始记录，则返回
            if (!recordStarted)
                return;

            //过滤除了字母以及数字的按键
            if (!((e.KeyValue > (int)Keys.A && e.KeyValue < (int)Keys.Z) ||
                (e.KeyValue > (int)Keys.D0 && e.KeyValue < (int)Keys.D9)))
                return;

            if(e.KeyValue == (int)Keys.Back)
            {
                MessageBox.Show("不允许退格，该次输入无效！", "错误");
                textBox3.Clear();
                timeline = new KeyboardTimeline();
                textBox3.Focus();
                return;
            }

            if ((Keys)e.KeyValue != Keys.Enter)
                timeline.MarkDown(e.KeyValue);

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            Console.WriteLine("text changed to " + textBox3.Text);
        }

        private void textBox3_KeyUp(object sender, KeyEventArgs e)
        {
            //Console.WriteLine("key up");

            //过滤除了字母以及数字的按键
            if (!((e.KeyValue > (int)Keys.A && e.KeyValue < (int)Keys.Z) ||
                (e.KeyValue > (int)Keys.D0 && e.KeyValue < (int)Keys.D9)))
                return;

            timeline.MarkUp(e.KeyValue);
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            Console.WriteLine("key press");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //密码输入错误
            if (!textBox2.Text.Equals(textBox3.Text))
            {
                MessageBox.Show("密码输入错误！", "错误");
                textBox3.Clear();
                timeline = new KeyboardTimeline();
                textBox3.Focus();
                return;
            }

            if (MessageBox.Show("是否保存？", "保存", MessageBoxButtons.YesNo)
                == DialogResult.Yes)
            {
                recordList.Add(timeline.ToVector());
                recordCounter++;
                textBox3.Clear();
                textBox3.Focus();

                //已经记录完毕
                if (recordCounter >= MAX_RECORD_REQUIRED)
                {
                    //新增用户
                    access.insert(textBox1.Text, textBox2.Text);

                    foreach (var record in recordList)
                    {
                        access.InsertKeyboardData(textBox1.Text, record);
                    }
                }
            }
        }
    }
}
