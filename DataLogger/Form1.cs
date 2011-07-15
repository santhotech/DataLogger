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
using System.Collections.Specialized;

namespace DataLogger
{
    public partial class Form1 : Form
    {       
        Methods val = new Methods();                
        private Hashtable _manifestContents;
        private Hashtable _currentAction;
        private Hashtable _manifestIndex;
        private Hashtable _manifestBtn;        
        private Hashtable _manifestFile;
        private Hashtable _objectIndex;
        private Hashtable _manifestDelBtn;
                    
        public Form1()
        {
            InitializeComponent();            
            _currentAction = new Hashtable();
            _manifestContents = new Hashtable();
            _manifestFile = new Hashtable();
            _manifestIndex = new Hashtable();
            _manifestBtn = new Hashtable();
            _objectIndex = new Hashtable();
            _manifestDelBtn = new Hashtable();


            ImageList HeightControlImageList = new System.Windows.Forms.ImageList(this.components);
            HeightControlImageList.ImageSize = new System.Drawing.Size(1, 25);
            HeightControlImageList.TransparentColor = System.Drawing.Color.Transparent;
            logrList.SmallImageList = HeightControlImageList;          
        }

        public void Form_closing(object sender, CancelEventArgs cargs)
        {
            cargs.Cancel = true;
            this.Text = "Please wait closing...";
            Thread t = new Thread(new ThreadStart(SafeClose));
            t.Start();           
        }

        public void SafeClose()
        {
            ArrayList al = new ArrayList();
            foreach (string de in _currentAction.Keys)
            {
                if ((int)_currentAction[de] == 2)
                {
                    al.Add(de);
                }
            }
            foreach (string str in al)
            {
                Button b = (Button)_manifestBtn[str];
                EventArgs e = null;
                b_Actions(b, e);
                Thread.Sleep(3000);
            }
            Environment.Exit(0);
        }

        private void strtBtn_Click(object sender, EventArgs e)
        {            
            string[] txtboxStr = new string[6] { ipTxt.Text, prtTxt.Text, fileSizeTxt.Text, fldrNameTxt.Text, lgrNameTxt.Text, pingRequest.Checked.ToString() };
            StringCollection sc = new StringCollection();
            sc.AddRange(txtboxStr);
            AddLogger(sc);
        }       

        private void AddLogger(StringCollection sc)
        {
            string unid = val.GetUniqueId();
            string[] txtboxStr = new string[7];
            txtboxStr[0] = unid;
            int tmp = 1;
            foreach (string str in sc)
            {
                txtboxStr[tmp] = str;
                tmp++;
            }
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
                ClearForm();         
                AddToListView(txtboxStr);                
                Logger log = new Logger(txtboxStr);
                log.LoggerStatusChanged +=new Logger.LoggerStatusChangedEventHandler(log_LoggerStatusChanged);                
                _objectIndex.Add(unid, log);

            }           
        }

        public void ClearForm()
        {
            lgrNameTxt.Text = string.Empty;            
        }

        public void log_LoggerStatusChanged(int status, string name)
        {
            if (status == 1)
            {
                LoggerWaiting(name);
            }
            if (status == 2)
            {
                LoggerStart(name);
            }
            if (status == 0)
            {
                LoggerStop(name);
            }
        }

        private void fldrBrwsBtn_Click(object sender, EventArgs e)
        {                        
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                fldrNameTxt.Text = folderBrowserDialog1.SelectedPath;
            }                        
        }                     

        private void Error(string msg)
        {
            MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }      
        
        public void AddToListView(string[] contents)
        {            
            string logrName = contents[0];
            string ipAddr = contents[1];
            string fileSize = contents[3];
            string prtNo = contents[2];
            string fldrName = contents[4];

            string[] toListView = new string[4] { contents[5], ipAddr, prtNo, "Stopped" };
            ListViewItem itm = new ListViewItem(toListView);
            logrList.Items.Add(itm); 
           
            Button b = new Button();
            b.Text = "Start";
            b.BackColor = SystemColors.Control;
            b.Font = this.Font;
            b.Name = logrName;
            b.Click += new EventHandler(b_Actions);

            Button d = new Button();
            d.Text = "Delete";
            b.BackColor = SystemColors.Control;
            d.Font = this.Font;
            d.Name = logrName+"Delete";
            d.Click += new EventHandler(d_Delete);

            int cnta = logrList.Items.IndexOf(itm);

            logrList.AddEmbeddedControl(b, 4, cnta);
            logrList.AddEmbeddedControl(d, 5, cnta);

            logrList.Items[cnta].ForeColor = Color.Red;

            

            _manifestContents.Add(logrName, contents);
            _currentAction.Add(logrName,0);
            _manifestIndex.Add(logrName, itm);
            _manifestBtn.Add(logrName, b);
            _manifestDelBtn.Add(logrName, d);            
        }

        public void d_Delete(object sender, EventArgs e)
        {
            Button d = (Button)sender;
            string n = d.Name;
            n = n.Substring(0, 10);
            string[] loggName = (string[])_manifestContents[n];
            string logName = loggName[5];
            ListViewItem indexItm = (ListViewItem)_manifestIndex[n];
            int ind = logrList.Items.IndexOf(indexItm);
            val.RemoveFromLoggerNames(logName);
            logrList.Items.RemoveAt(ind);
            _manifestContents.Remove(n);
            _currentAction.Remove(n);
            _manifestIndex.Remove(n);
            _manifestBtn.Remove(n);
            _manifestDelBtn.Remove(n);                 
        }
        

        public void b_Actions(object sender, EventArgs e)
        {          
            Button b = (Button)sender;
            string n = b.Name;            
            Logger l = (Logger)_objectIndex[n];
            if ((int)_currentAction[n] == 2)
            {
                l.StopLoggin();                                                              
            }
            else if ((int)_currentAction[n] == 0)
            {                
                l.StartLogging();                   
            }                                  
        }
        
        public void LoggerStart(string n)
        {
            _currentAction[n] = 2; 
            Button b = (Button)_manifestBtn[n];
            Button d = (Button)_manifestDelBtn[n];
            ListViewItem indexItm = (ListViewItem)_manifestIndex[n];
            int index = 0;
            logrList.BeginInvoke((MethodInvoker)(() => index = logrList.Items.IndexOf(indexItm)));
            d.BeginInvoke((MethodInvoker)(() => d.Enabled = false));
            b.BeginInvoke((MethodInvoker)(() => b.Enabled = true));
            b.BeginInvoke((MethodInvoker)(() => b.Text = "Stop"));
            logrList.BeginInvoke((MethodInvoker)(() => logrList.Items[index].SubItems[3].Text = "Active"));
            logrList.BeginInvoke((MethodInvoker)(() => logrList.Items[index].ForeColor = Color.DarkGreen));    
        }

        public void LoggerStop(string n)
        {
            _currentAction[n] = 0; 
            Button b = (Button)_manifestBtn[n];
            Button d = (Button)_manifestDelBtn[n];
            ListViewItem indexItm = (ListViewItem)_manifestIndex[n];
            int index = 0;
            logrList.BeginInvoke((MethodInvoker)(() => index = logrList.Items.IndexOf(indexItm)));
            d.BeginInvoke((MethodInvoker)(() => d.Enabled = true));
            b.BeginInvoke((MethodInvoker)(() => b.Enabled = true));
            b.BeginInvoke((MethodInvoker)(() => b.Text = "Start"));            
            logrList.BeginInvoke((MethodInvoker)(() => logrList.Items[index].SubItems[3].Text = "Stopped"));
            logrList.BeginInvoke((MethodInvoker)(() => logrList.Items[index].ForeColor = Color.Red));            
        }

        public void LoggerWaiting(string n)
        {
            Button b = (Button)_manifestBtn[n];
            Button d = (Button)_manifestDelBtn[n];
            ListViewItem indexItm = (ListViewItem)_manifestIndex[n];
            int index = 0;
            logrList.BeginInvoke((MethodInvoker)(() => index = logrList.Items.IndexOf(indexItm)));
            d.BeginInvoke((MethodInvoker)(() => d.Enabled = false));
            b.BeginInvoke((MethodInvoker)(() => b.Enabled = false));

            b.BeginInvoke((MethodInvoker)(() => b.Text = "Waiting"));            
            logrList.BeginInvoke((MethodInvoker)(() => logrList.Items[index].SubItems[3].Text = "Waiting"));
            logrList.BeginInvoke((MethodInvoker)(() => logrList.Items[index].ForeColor = Color.DarkOrange));                        
        }  
    }
}