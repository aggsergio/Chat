using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net.Sockets;
using System.IO;


namespace cliente_chat
{
    public partial class Form1 : Form
    {

        static private NetworkStream stream;
        static private StreamWriter streamw;
        static private StreamReader streamr;
        static private TcpClient client = new TcpClient();
        static private string nombre = "Anon";
        private delegate void DAddItem(String s);

        public Form1()
        {
            InitializeComponent();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            streamw.WriteLine(textBox2.Text);
            streamw.Flush();
            textBox2.Clear();
        }

        private void AddItem(String s)
        {
            listBox1.Items.Add(s);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Visible = false;
            textBox1.Visible = false;
            button1.Visible = false;
            listBox1.Visible = true;
            textBox2.Visible = true;
            button2.Visible = true;
            pictureBox1.Visible = false;
            pictureBox2.Visible = true;
            button3.Visible = true;
            label2.Visible = true;
            nombre = textBox1.Text;
            label2.Text = "Bienvenido " + nombre;
            Conectar();

        }

        void Conectar()
        {
            try
            {
                client.Connect("127.0.0.1", 8000);
                if (client.Connected)
                {
                    Thread t = new Thread(Listen);

                    stream = client.GetStream();
                    streamw = new StreamWriter(stream);
                    streamr = new StreamReader(stream);

                    streamw.WriteLine(nombre);
                    streamw.Flush();

                    t.Start();
                }
                else
                {
                    MessageBox.Show("Desconectado");
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Desconectado");
                Application.Exit();
            }
        }

        void Listen()
        {
            while (client.Connected)
            {
                try
                {
                    this.Invoke(new DAddItem(AddItem), streamr.ReadLine());

                }
                catch
                {
                    MessageBox.Show("Implosible conectar al servidor");
                    Application.Exit();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
