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


namespace DataLogger
{
    public partial class Form1 : Form
    {       
        Methods val = new Methods();        
        int count = 0;
        private Hashtable _manifestContents;
        private Hashtable _currentAction;
        private Hashtable _manifestIndex;
        private Hashtable _manifestBtn;
        private IList<Thread> _threads;
        private Hashtable _manifestFile;
        private Hashtable _objectIndex;
                    
        public Form1()
        {
            InitializeComponent();
            _threads = new List<Thread>();
            _currentAction = new Hashtable();
            _manifestContents = new Hashtable();
            _manifestFile = new Hashtable();
            _manifestIndex = new Hashtable();
            _manifestBtn = new Hashtable();
            _objectIndex = new Hashtable();


            ImageList HeightControlImageList = new System.Windows.Forms.ImageList(this.components);
            HeightControlImageList.ImageSize = new System.Drawing.Size(1, 25);
            HeightControlImageList.TransparentColor = System.Drawing.Color.Transparent;
            logrList.SmallImageList = HeightControlImageList;          
        }               

        private void strtBtn_Click(object sender, EventArgs e)
        {
            AddLogger();
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
                AddToListView(txtboxStr, count++);
                
                Logger log = new Logger(txtboxStr);
                log.LoggerStatusChanged +=new Logger.LoggerStatusChangedEventHandler(log_LoggerStatusChanged);
                _objectIndex.Add(txtboxStr[0], log);
            }           
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

        public void CompressOldFile(string fileName)
        {
            try
            {
                string allData = File.ReadAllText(fileName);
                Methods.CompressStringToFile(fileName + ".gz", allData);
            }
            catch{ }
        }
        
        public void AddToListView(string[] contents,int cnt)
        {            
            string logrName = contents[0];
            string ipAddr = contents[1];
            string fileSize = contents[3];
            string prtNo = contents[2];
            string fldrName = contents[4];
            _manifestContents.Add(logrName, contents);
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
            int index = (int)_manifestIndex[n];
            b.Text = "Stop";                        
            logrList.Items[index].ForeColor = Color.Green;
            logrList.Items[index].SubItems[3].Text = "Active"; 
        }

        public void LoggerStop(string n)
        {
            _currentAction[n] = 0; 
            Button b = (Button)_manifestBtn[n];
            int index = (int)_manifestIndex[n];
            b.BeginInvoke((MethodInvoker)(() => b.Enabled = true));
            b.BeginInvoke((MethodInvoker)(() => b.Text = "Start"));            
            logrList.BeginInvoke((MethodInvoker)(() => logrList.Items[index].SubItems[3].Text = "Stopped"));
            logrList.BeginInvoke((MethodInvoker)(() => logrList.Items[index].ForeColor = Color.Red));            
        }

        public void LoggerWaiting(string n)
        {
            Button b = (Button)_manifestBtn[n];
            int index = (int)_manifestIndex[n];
            b.BeginInvoke((MethodInvoker)(() => b.Enabled = false));
            b.BeginInvoke((MethodInvoker)(() => b.Text = "Waiting"));            
            logrList.BeginInvoke((MethodInvoker)(() => logrList.Items[index].SubItems[3].Text = "Waiting"));
            logrList.BeginInvoke((MethodInvoker)(() => logrList.Items[index].ForeColor = Color.DarkOrange));                        
        }  
    }
}