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
        Methods val = new Methods();
        public Form1()
        {
            InitializeComponent();
            
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        

        private void strtBtn_Click(object sender, EventArgs e)
        {
            AddLogger();
        }

        private void AddLogger()
        {
            string[] txtboxStr = new string[5] { lgrNameTxt.Text, ipTxt.Text, prtTxt.Text, fileSizeTxt.Text, fldrNameTxt.Text };
            AddToListView(txtboxStr);
            if (val.ValidateForm(txtboxStr))
            {
                Thread t = new Thread(new ParameterizedThreadStart(StartLoggin));
                t.IsBackground = true;
                t.Start(txtboxStr);                
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

        public TcpClient GetTcpClient(string ip, string port)
        {
            TcpClient tc = new TcpClient();
            if (val.IsAllDigits(port))
            {
                int prt = Convert.ToInt32(port);
                tc.Connect(ip, prt);
            }
            else
            {
                MessageBox.Show("Cannot connect to the socket", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return tc;
        }

        public void StartLoggin(object obj)
        {
            string[] contents = (string[])obj;
            bool setflag = true;            
            string logrName = contents[0];
            string fldrName = contents[4];
            long fileSize = val.GetBytesAsLong(contents[3]);            
            string fileName = GetFileName(fldrName, logrName);
            int prt = Convert.ToInt32(contents[2]);
            TcpClient tc = new TcpClient();
            try { tc.Connect(contents[1], prt); }
            catch { Error("Cannot connect to the client"); setflag = false; }
            Thread.Sleep(2000);
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
                        catch
                        {
                            Error("Error writing to file");
                            break;
                        }
                    }
                    else
                    {
                        CompressOldFile(fileName);
                        fileName = GetFileName(fldrName, logrName);    
                    }
                }
                catch
                {
                    Error("Cannot connect to socket");
                    break;
                }
            }            
        }
        private void Error(string msg)
        {
            MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        public void AddToListView(string[] contents)
        {            
            
            string logrName = contents[0];
            string ipAddr = contents[1];
            string fileSize = contents[3];
            string prtNo = contents[2];
            string fldrName = contents[4];
            string[] toListView = new string[3] { logrName, ipAddr, prtNo };

            ListViewItem itm = new ListViewItem(toListView);
            logrList.Items.Add(itm);

            /*
            Button b = new Button();
            b.Text = "ClickMe";
            b.BackColor = SystemColors.Control;
            b.Font = this.Font;
            b.Click += new EventHandler(b_Click);
            // Put it in the first column of the fourth row
            logrList.AddEmbeddedControl(b, 0, 3);
             * */
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