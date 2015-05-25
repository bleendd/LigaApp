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
using LigaApp.WebServiceRef;
using System.Xml;
using System.Xml.Linq;

namespace LigaApp
{
    public partial class Form1 : Form
    {
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
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
            clientSocketChat.Client.Send(Encoding.ASCII.GetBytes("1"));
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
                MessageBox.Show("Lidhja me serverin e chat mungon!");
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
            try
            {
                dataGridView2.Rows.Add(
                    int.Parse(textBox5.Text),
                    comboBox4.SelectedItem,
                    comboBox3.SelectedItem,
                    int.Parse(textBox8.Text),
                    int.Parse(textBox7.Text),
                    textBox6.Text);
                GridNumrat(dataGridView2);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Te dhenat nuk jane ne formatin e duhur!");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Menaxhim();
            Mbushja();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView3.Rows.Add(textBox11.Text);
            GridNumrat(dataGridView3);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Regjistrim();
            Mbushja();
        }

        private void btnShfaq_Click(object sender, EventArgs e)
        {
            UebSherbimiSoapClient client = new UebSherbimiSoapClient();
            XElement xml = client.GetMenaxhimi();
            int numrimi = xml.Elements("ClassMenaxhimi").Count();

            var ElementKlasa = xml.Elements("ClassMenaxhimi");

            var AtributJava = ElementKlasa.Attributes("Java");
            var ElementEkipi1 = ElementKlasa.Elements("Ekipi1");
            var ElementEkipi2 = ElementKlasa.Elements("Ekipi2");
            var ElementGolaE1 = ElementKlasa.Elements("GolaE1");
            var ElementGolaE2 = ElementKlasa.Elements("GolaE2");
            var ElementKomenti = ElementKlasa.Elements("Komenti");

            dataGridView1.Rows.Clear();

            int i = 0;
            foreach (var element in ElementKlasa)
            {
                if (ElementKlasa.Attributes("Java").ElementAt(i).Value == textBox4.Text)
                {
                    dataGridView1.Rows.Add(
                        ElementEkipi1.ElementAt(i).Value,
                        ElementEkipi2.ElementAt(i).Value,
                        ElementGolaE1.ElementAt(i).Value,
                        ElementGolaE2.ElementAt(i).Value,
                        ElementKomenti.ElementAt(i).Value);
                }
                i++;
            }
            GridNumrat(dataGridView1);
        }


        private void GridNumrat(DataGridView dgv)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                row.HeaderCell.Value = (row.Index + 1).ToString();
            }
        }

        private void Mbushja()
        {
            dataGridView2.Rows.Clear();
            dataGridView3.Rows.Clear();
            comboBox3.Items.Clear();
            comboBox4.Items.Clear();
            UebSherbimiSoapClient client = new UebSherbimiSoapClient();
            XElement xml = client.GetRegjistrimi();
            var ElementKlasa = xml.Elements("ClassRegjistrimi");
            var ElementEkipi1 = ElementKlasa.Elements("Ekipi");
            foreach (var x in ElementEkipi1)
            {
                dataGridView3.Rows.Add(x.Value);
                comboBox3.Items.Add(x.Value);
                comboBox4.Items.Add(x.Value);
            }
            GridNumrat(dataGridView3);
            xml = client.GetMenaxhimi();
            ElementKlasa = xml.Elements("ClassMenaxhimi");
            int numrimi = xml.Elements("ClassMenaxhimi").Count();
            ElementEkipi1 = ElementKlasa.Elements("Ekipi1");
            var AtributJava = ElementKlasa.Attributes("Java");
            var ElementEkipi2 = ElementKlasa.Elements("Ekipi2");
            var ElementGolaE1 = ElementKlasa.Elements("GolaE1");
            var ElementGolaE2 = ElementKlasa.Elements("GolaE2");
            var ElementKomenti = ElementKlasa.Elements("Komenti");
            for (int i = 0; i < numrimi; i++)
            {
                dataGridView2.Rows.Add();
                dataGridView2.Rows[i].Cells[0].Value = AtributJava.ElementAt(i).Value;
                dataGridView2.Rows[i].Cells[1].Value = ElementEkipi1.ElementAt(i).Value;
                dataGridView2.Rows[i].Cells[2].Value = ElementEkipi2.ElementAt(i).Value;
                dataGridView2.Rows[i].Cells[3].Value = ElementGolaE1.ElementAt(i).Value;
                dataGridView2.Rows[i].Cells[4].Value = ElementGolaE2.ElementAt(i).Value;
                dataGridView2.Rows[i].Cells[5].Value = ElementKomenti.ElementAt(i).Value;
            }
            GridNumrat(dataGridView2);

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

        private void Lidhja()
        {

            clientSocketChat.Connect("127.0.0.1", 8888);
            serverStream = clientSocketChat.GetStream();
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(Emri + "$");
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();

            Thread ctThread = new Thread(getMessage);
            ctThread.Start();

        }

        private void getMessage()
        {
            while (clientSocketChat.Connected)
            {
                serverStream = clientSocketChat.GetStream();
                byte[] inStream = new byte[4096];
                try
                {
                    int inStreamLength = serverStream.Read(inStream, 0, inStream.Length);
                    string returndata = System.Text.Encoding.ASCII.GetString(inStream, 0, inStreamLength);
                    readData = "" + returndata;
                }
                catch
                {
                    MessageBox.Show("Chat serveri u shkeput!");
                    break;
                }
                msg();

            }
        }

        private void msg()
        {
            if (textBox2.InvokeRequired)
                textBox2.Invoke(new MethodInvoker(msg));
            else
            {
                string x = textBox2.Text;
                textBox2.Text = "";
                textBox2.AppendText(x + Environment.NewLine + " >> " + readData);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                Lidhja();
                Mbushja();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lidhja me chatserver ose me webservice nuk mund te realizohet!");
            }
        }


    }
}
