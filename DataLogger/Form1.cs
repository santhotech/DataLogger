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
using System.Collections;
using System.IO.Compression;

namespace DataLogger
{
    public partial class Form1 : Form
    {       
        Methods val = new Methods();        
        int count = 0;
        Hashtable ht = new Hashtable();
        private IList<Thread> _threads;
                    
        public Form1()
        {
            InitializeComponent();
            _threads = new List<Thread>();
        }

        public void LaunchThread(string[] contents, string tName)
        {            
            AddToListView(contents, count, tName);            
            Thread thread = new Thread(new ParameterizedThreadStart(StartLoggin));
            thread.IsBackground = true;
            thread.Name = tName;
            _threads.Add(thread);
            thread.Start(contents);
            count++;                       
        }

        public void KillThread(string tName)
        {            
            foreach (Thread thread in _threads)
            {
                if (thread.Name == tName)
                    thread.Abort();
            }
        }

        private void strtBtn_Click(object sender, EventArgs e)
        {
            AddLogger();
        }        
        
        private void AddLogger()
        {                            
            string[] txtboxStr = new string[5] { lgrNameTxt.Text, ipTxt.Text, prtTxt.Text, fileSizeTxt.Text, fldrNameTxt.Text };                       
            if (val.ValidateForm(txtboxStr))
            {
                LaunchThread(txtboxStr);
            }
            else
            {
                MessageBox.Show("Please enter all the inputs", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void fldrBrwsBtn_Click(object sender, EventArgs e)
        {                        
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                fldrNameTxt.Text = folderBrowserDialog1.SelectedPath;
            }                        
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
        public void AddToListView(string[] contents,int cnt, string unid)
        {                        
            string logrName = contents[0];
            string ipAddr = contents[1];
            string fileSize = contents[3];
            string prtNo = contents[2];
            string fldrName = contents[4];
            string[] toListView = new string[4] { logrName, ipAddr, prtNo, "Active" };
            ListViewItem itm = new ListViewItem(toListView);
            logrList.Items.Add(itm);            
            Button b = new Button();
            b.Text = "Stop";
            b.BackColor = SystemColors.Control;
            b.Font = this.Font;
            b.Name = unid;
            b.Click += new EventHandler(b_Stop);            
            logrList.AddEmbeddedControl(b, 4, cnt);            
        }

        public void b_Stop(object sender, EventArgs e)
        {          
            Button b = (Button)sender;
            string n = b.Name;
            KillThread(n);
            b.Text = "Start";
            b.Click += new EventHandler(b_Start);
        }

        public void b_Start(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            string n = b.Name;
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