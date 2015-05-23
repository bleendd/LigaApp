using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LigaApp
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        public void Check()
        {
            if (!(textBox1.Text == "") && !Regex.IsMatch(textBox1.Text, @"^\d+$"))
            {
                Form1 forma = new Form1(textBox1.Text);
                forma.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Nuk lejohen numra ose zbrazet!");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Check();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
