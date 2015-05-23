using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;

namespace LigaApp
{
    public partial class Form1 : Form
    {
        Thread threadi;
        TcpClient clientSocketChat = new TcpClient();
        TcpClient clientMenaxhimi = new TcpClient();

        NetworkStream serverStream = default(NetworkStream);
        NetworkStream serverStreamMenaxhimi = default(NetworkStream);
        string readData = null;
        string Emri = null;

        public Form1(string x)
        {
            InitializeComponent();
            this.Emri = x;
            Lidhja();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                byte[] outStream = System.Text.Encoding.ASCII.GetBytes(textBox3.Text + "$");
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Flush();
                textBox3.Text = "";
                textBox3.Focus();
            }
            catch
            {
                textBox3.Text = "";
                textBox3.Focus();
                MessageBox.Show("Lidhja me serverin e chat mungon!");
            }
        }

        private void Lidhja()
        {
            try
            {
                clientSocketChat.Connect("127.0.0.1", 8888);
                serverStream = clientSocketChat.GetStream();
                byte[] outStream = System.Text.Encoding.ASCII.GetBytes(Emri + "$");
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Flush();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void getMessage()
        {
            while (true)
            {
                serverStream = clientSocketChat.GetStream();
                byte[] inStream = new byte[4096];
                int inStreamLength = serverStream.Read(inStream, 0, inStream.Length);
                string returndata = System.Text.Encoding.ASCII.GetString(inStream, 0, inStreamLength);
                readData = "" + returndata;
                msg();
            }
        }

        private void msg()
        {
            if (this.InvokeRequired)
                this.Invoke(new MethodInvoker(msg));
            else
            {
                string x = textBox2.Text;
                textBox2.Text = "";
                textBox2.AppendText(x + Environment.NewLine + " >> " + readData);
            }
        }

        private void Menaxhim()
        {
            if (dataGridView2.Rows.Count > 0)
            {
                try
                {
                    clientMenaxhimi.Connect("127.0.0.1", 55666);
                    serverStreamMenaxhimi = clientMenaxhimi.GetStream();
                    string dergimi = "menaxhimi";

                    for (int i = 0; i < dataGridView2.Rows.Count; i++)
                    {
                        dergimi += dataGridView2.Rows[i].Cells[0].Value.ToString() + "$";
                        dergimi += dataGridView2.Rows[i].Cells[1].Value.ToString() + "$";
                        dergimi += dataGridView2.Rows[i].Cells[2].Value.ToString() + "$";
                        dergimi += dataGridView2.Rows[i].Cells[3].Value.ToString() + "$";
                        dergimi += dataGridView2.Rows[i].Cells[4].Value.ToString() + "$";
                        dergimi += dataGridView2.Rows[i].Cells[5].Value.ToString() + "$";
                    }

                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes(dergimi);
                    serverStreamMenaxhimi.Write(outStream, 0, outStream.Length);
                    serverStreamMenaxhimi.Flush();
                    clientMenaxhimi.Close();
                    clientMenaxhimi = new TcpClient();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void Regjistrim()
        {
            if (dataGridView3.Rows.Count > 0)
            {
                try
                {
                    clientMenaxhimi.Connect("127.0.0.1", 55666);
                    serverStreamMenaxhimi = clientMenaxhimi.GetStream();
                    string dergimi = "regjistrimi";

                    for (int i = 0; i < dataGridView3.Rows.Count; i++)
                    {
                        dergimi += dataGridView3.Rows[i].Cells[0].Value.ToString() + "$";
                    }

                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes(dergimi);
                    serverStreamMenaxhimi.Write(outStream, 0, outStream.Length);
                    serverStreamMenaxhimi.Flush();
                    clientMenaxhimi.Close();
                    clientMenaxhimi = new TcpClient();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void bttnRez_Click(object sender, EventArgs e)
        {
            tabContorl1.SelectTab("Rezultatet");   
        }

        private void bttnMen_Click(object sender, EventArgs e)
        {
            tabContorl1.SelectTab("Menaxhimi");   
        }

        private void bttnRegjistrimi_Click(object sender, EventArgs e)
        {
            tabContorl1.SelectTab("Regjistrimi");   
        }

        private void btnFutnList_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Add("2", "Asd", "Asd", "1", "2", "Asd");
            GridNumrat(dataGridView2);
        }

        private void GridNumrat(DataGridView dgv)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                row.HeaderCell.Value = (row.Index + 1).ToString();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Menaxhim();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView3.Rows.Add(textBox11.Text);
            GridNumrat(dataGridView3);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Regjistrim();
        }
       

    }
}
