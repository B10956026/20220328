using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net; //匯入網路通訊協定相關參數
using System.Net.Sockets; //匯入網路插座功能函數
using System.Threading; //匯入多執行緒功能函數

namespace _20220328
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        UdpClient U;
        Thread Th;

        private void Listen()
        {
            try
            {
                int Port = int.Parse(textBox_receivePort.Text);

                U = new UdpClient(Port);

                IPEndPoint EP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), Port);

                while (true)
                {
                    byte[] B = U.Receive(ref EP);
                    textBox_receiveMsg.Text = Encoding.Default.GetString(B);
                }
            }
            catch
            {

            }
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "我的IP:" + MyIP();
        }

        private void button_startListen_Click(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            Th = new Thread(Listen);

            Th.Start();

            button_startListen.Enabled = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Th.Abort();
                U.Close();
            }
            catch
            {

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string IP = textBox_targetIP.Text;
                int Port = int.Parse(textBox_targetPort.Text);
                byte[] B = Encoding.Default.GetBytes(textBox_sendMsg.Text);
                UdpClient S = new UdpClient();
                S.Send(B, B.Length, IP, Port);
                S.Close();
            }
            catch
            {

            }
            
        }

        private string MyIP()
        {
            string hostname = Dns.GetHostName();
            IPAddress[] ip = Dns.GetHostEntry(hostname).AddressList;

            foreach(IPAddress it in ip)
            {
                if(it.AddressFamily == AddressFamily.InterNetwork && it.ToString() != "192.168.56.1")
                {
                    return it.ToString();
                }
            }
            return "";

        }
    }
}
