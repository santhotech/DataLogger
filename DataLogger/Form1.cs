using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.IO.Compression;

namespace DataLogger
{
    public partial class Form1 : Form
    {
        validation val = new validation();
        public Form1()
        {
            InitializeComponent();
            
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        

        private void strtBtn_Click(object sender, EventArgs e)
        {            
            int port;
            string ip = ipTxt.Text;
            string logrName = lgrNameTxt.Text;
            Int32.TryParse(prtTxt.Text,out port);
            TcpClient tcpClient = new TcpClient();
            string[] txtboxStr = new string[4] { prtTxt.Text, ipTxt.Text, lgrNameTxt.Text, fldrNameTxt.Text };
            if (val.ValidateForm(txtboxStr))
            {
                try
                {
                    tcpClient.Connect(ip, port);
                    object[] obj = new object[4];
                    obj[0] = tcpClient; obj[1] = logrName; obj[2] = fldrNameTxt.Text;
                    long flSize = Convert.ToInt32(fileSizeTxt.Text) * 1000000;
                    obj[3] = flSize;
                    Thread t = new Thread(new ParameterizedThreadStart(StartLoggin));
                    t.Start(obj);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    MessageBox.Show("Cannot connect to the data source", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please enter all the inputs", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        public void StartLoggin(object obj)
        {
            Array argArray = new object[3];
            argArray = (Array)obj;
            TcpClient tc = (TcpClient)argArray.GetValue(0);            
            string logrName = (string)argArray.GetValue(1);
            string fldrName = (string)argArray.GetValue(2);
            long fileSize = (long)argArray.GetValue(3);
            MessageBox.Show(fileSize.ToString());
            string fileName = GetFileName(fldrName, logrName);                                                            
            bool setflag = true;
            while (setflag)
            {
                try
                {
                    long s1 = 0;

                    if (File.Exists(fileName))
                    {
                        FileInfo f = new FileInfo(fileName);
                        s1 = f.Length;
                    }

                    if (s1 < fileSize)
                    {
                        NetworkStream ns = tc.GetStream();
                        byte[] instream = new byte[tc.ReceiveBufferSize];
                        int actuallyRead = ns.Read(instream, 0, tc.ReceiveBufferSize);
                        string decodedData = string.Empty;
                        decodedData = System.Text.Encoding.ASCII.GetString(instream, 0, actuallyRead);
                        decodedData = decodedData.Trim('\0');
                        decodedData = decodedData.Replace("\n", "\r\n");
                        try
                        {
                            File.AppendAllText(fileName, decodedData);
                        }
                        catch (Exception ex) { MessageBox.Show(ex.ToString()); }
                    }
                    else
                    {
                        CompressOldFile(fileName);
                        fileName = GetFileName(fldrName, logrName);    
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }            
        }
        public void CompressOldFile(string fileName)
        {
            try
            {
                string allData = File.ReadAllText(fileName);
                CompressStringToFile(fileName + ".gz", allData);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public string GetFileName(string fldrName,string logrName)
        {
            string fileName = DateTime.UtcNow.ToString("s");
            fileName = logrName + "-" + fileName;
            fileName = fileName.Replace(':', '-');
            fileName = fileName.Replace(' ', '-');
            fileName = fldrName + "\\" + fileName + ".txt";                        
            return fileName;
        }
        public void AddToListView()
        {
            Button b = new Button();
            b.Text = "ClickMe";
            b.BackColor = SystemColors.Control;
            b.Font = this.Font;
            b.Click += new EventHandler(b_Click);
            // Put it in the first column of the fourth row
            logrList.AddEmbeddedControl(b, 0, 3);
        }
        public void b_Click(object sender, EventArgs e)
        {

        }

        public static void CompressStringToFile(string fileName, string value)
        {
            string temp = Path.GetTempFileName();
            File.WriteAllText(temp, value);
            byte[] b;
            using (FileStream f = new FileStream(temp, FileMode.Open))
            {
                b = new byte[f.Length];
                f.Read(b, 0, (int)f.Length);
            }
            using (FileStream f2 = new FileStream(fileName,FileMode.Create))
            using (GZipStream gz = new GZipStream(f2,CompressionMode.Compress,false))
            {
                gz.Write(b,0,b.Length);
            }
        }        
    }
}