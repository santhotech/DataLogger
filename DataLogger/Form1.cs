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
        private Hashtable _manifest;
        private Hashtable _currentAction;
        private Hashtable _manifestIndex;
        private Hashtable _manifestBtn;
        private IList<Thread> _threads;
        private Hashtable _manifestFile;        
                    
        public Form1()
        {
            InitializeComponent();
            _threads = new List<Thread>();
            _currentAction = new Hashtable();
            _manifest = new Hashtable();
            _manifestFile = new Hashtable();
            _manifestIndex = new Hashtable();
            _manifestBtn = new Hashtable();

            ImageList HeightControlImageList = new System.Windows.Forms.ImageList(this.components);
            HeightControlImageList.ImageSize = new System.Drawing.Size(1, 25);
            HeightControlImageList.TransparentColor = System.Drawing.Color.Transparent;
            logrList.SmallImageList = HeightControlImageList;
        }

        public void LaunchThread(string[] contents, string tName)
        {                               
            Thread thread = new Thread(new ParameterizedThreadStart(StartLoggin));
            thread.IsBackground = true;
            thread.Name = tName;
            _threads.Add(thread);
            thread.Start(contents);           
        }

        public void KillThread(string tName)
        {
            try
            {
                foreach (Thread thread in _threads)
                {
                    if (thread.Name == tName)
                    {
                        thread.Abort();
                        RemoveThread(thread);
                        break;
                    }
                }
            }
            catch { }
        }

        public void RemoveThread(Thread t)
        {
            _threads.Remove(t);
        }

        private void strtBtn_Click(object sender, EventArgs e)
        {
            AddLogger();
        }
        private void BtnActions()
        {
            strtBtn.Enabled = false;
            timer1.Enabled = true;
        }
        private void AddLogger()
        {            
            string[] txtboxStr = new string[5] { lgrNameTxt.Text, ipTxt.Text, prtTxt.Text, fileSizeTxt.Text, fldrNameTxt.Text };
            if (!(val.ValidateForm(txtboxStr))) 
            {
                Error("Enter all the fields");
            }
            else if(!(val.CheckLoggerNameExist(lgrNameTxt.Text)))
            {
                Error("Logger Name already exist. Please choose a different name");
            }
            else 
            {
                //string tName = lgrNameTxt.Text;                
                BtnActions();
                AddToListView(txtboxStr, count++);
                //LaunchThread(txtboxStr,tName);
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
            catch { Error("Cannot connect to the client"); setflag = false; ActivateLoggerStop(logrName); }
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
                        _manifestFile[logrName] = fileName;
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
                    MessageBox.Show("Logger Stopped","Information",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    ActivateLoggerStop(logrName);
                    break;
                }
            }            
        }

        private void ActivateLoggerStop(string logrName)
        {
            Button b = (Button)_manifestBtn[logrName];
            int index = (int)_manifestIndex[logrName];
            LoggerStop(b, logrName, index);
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
            catch
            {
                Error("Cannot compress file");
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
        public void AddToListView(string[] contents,int cnt)
        {
            
            string logrName = contents[0];
            string ipAddr = contents[1];
            string fileSize = contents[3];
            string prtNo = contents[2];
            string fldrName = contents[4];
            _manifest.Add(logrName, contents);
            string[] toListView = new string[4] { logrName, ipAddr, prtNo, "Stopped" };
            ListViewItem itm = new ListViewItem(toListView);
            logrList.Items.Add(itm);            
            Button b = new Button();
            b.Text = "Start";
            b.BackColor = SystemColors.Control;
            b.Font = this.Font;
            b.Name = logrName;
            b.Click += new EventHandler(b_Actions);            
            logrList.AddEmbeddedControl(b, 4, cnt);
            _currentAction[logrName] = 0;
            _manifestIndex.Add(logrName, cnt);
            _manifestBtn.Add(logrName, b);
            logrList.Items[cnt].ForeColor = Color.Red;
        }

        public void b_Actions(object sender, EventArgs e)
        {          
            Button b = (Button)sender;
            string n = b.Name;
            int index = (int)_manifestIndex[n];
            if ((int)_currentAction[n] == 1)
            {
                LoggerStop(b, n, index);
            }
            else if ((int)_currentAction[n] == 0)
            {
                LoggerStart(b, n, index);             
            }                                  
        }

        public void LoggerStop(Button b, string n, int index)
        {
            KillThread(n);
            b.BeginInvoke((MethodInvoker)(() => b.Text = "Start"));
            //b.Text = "Start";
            _currentAction[n] = 0;
            //logrList.Items[index].ForeColor = Color.Red;

            logrList.BeginInvoke((MethodInvoker)(() => logrList.Items[index].SubItems[3].Text = "Stopped"));
            logrList.BeginInvoke((MethodInvoker)(() => logrList.Items[index].ForeColor = Color.Red));
            //logrList.Items[index].SubItems[3].Text = "Stopped";
            string oldFile = (string)_manifestFile[n];
            CompressOldFile(oldFile);        
        }

        public void LoggerStart(Button b, string n, int index)
        {
            b.Text = "Stop";
            string[] con = (string[])_manifest[n];
            LaunchThread(con, n);
            _currentAction[n] = 1;
            logrList.Items[index].ForeColor = Color.Green;
            logrList.Items[index].SubItems[3].Text = "Active"; 
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            strtBtn.Enabled = true;
            this.timer1.Enabled = false;
        }        
    }
}