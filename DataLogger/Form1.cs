using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace DataLogger
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private bool ValidateForm()
        {
            TextBox[] tb = new TextBox[4] { prtTxt, ipTxt, lgrNameTxt, fldrNameTxt };
            foreach (TextBox t in tb)
            {
                if (t.Text.Trim() == "")
                {
                    MessageBox.Show("Enter All fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }
        private void strtBtn_Click(object sender, EventArgs e)
        {            
            int port;
            string ip = ipTxt.Text;
            Int32.TryParse(prtTxt.Text,out port);
            TcpClient tcpClient = new TcpClient();
            if (ValidateForm())
            {
                try
                {
                    tcpClient.Connect(ip, port);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    MessageBox.Show("Cannot connect to the data source", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }            
        }

        private void fldrBrwsBtn_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                fldrNameTxt.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int i = 0;
            MessageBox.Show(Convert.ToString(i));
        }        
    }
}
