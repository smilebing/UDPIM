using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UDPIM.Socket;

namespace UDPIM
{
    public partial class Form1 : Form
    {
        private Server server;
        public Form1()
        {
            InitializeComponent();
            server = Server.getInstance();
            server.start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            server.stop();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            server.stop();
        }
    }
}
