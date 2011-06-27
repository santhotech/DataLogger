using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace DataLogger
{
    class Logger
    {
        private bool setFlag;
        private string[] contents;                
        private Methods val;
        private string currentFileName;
        private string logrName;
        private string fldrName;
        private long fileSize;
        private string fileName;
        private int prt;
        private int stateFlag;
        private string ipAddr;       

        public delegate void LoggerStatusChangedEventHandler(int curFlag, string name);
        public event LoggerStatusChangedEventHandler LoggerStatusChanged;
        public int StateFlag
        {
            get { return this.stateFlag; }
            set
            {
                this.stateFlag = value;
                if (LoggerStatusChanged != null)
                {
                    this.LoggerStatusChanged(stateFlag,contents[0]);
                }
            }
        }


        public Logger() { }        
        
        public Logger(string[] contents)
        {
            this.contents = contents;                        
            val = new Methods();            
            logrName = contents[5];
            ipAddr = contents[1];
            prt = Convert.ToInt32(contents[2]);
            fileSize = val.GetBytesAsLong(contents[3]);
            fldrName = contents[4];                                            
        }

        public void StartLogging()
        {
            this.StateFlag = 2;
            setFlag = true;            
            Thread t = new Thread(new ThreadStart(LogData));
            t.IsBackground = true;            
            t.Start();            
        }

        public void StopLoggin()
        {
            this.StateFlag = 1;
            setFlag = false;            
        }
        
        public void LogData()
        {
            TcpClient tc = GetTcpClient();
            fileName = val.GetFileName(fldrName, logrName);                                        
            while (setFlag)
            {               
                long s1 = ReturnFileSize(fileName);
                if (s1 < fileSize)
                {
                    try
                    {
                        currentFileName = fileName;
                        byte[] instream = new byte[tc.ReceiveBufferSize];
                        int actuallyRead = tc.GetStream().Read(instream, 0, tc.ReceiveBufferSize);
                        if (actuallyRead == 0) break;
                        string decodedData = string.Empty;
                        decodedData = System.Text.Encoding.ASCII.GetString(instream, 0, actuallyRead);
                        decodedData = decodedData.Trim('\0');
                        decodedData = decodedData.Replace("\n", "\r\n");
                        File.AppendAllText(fileName, decodedData);
                    }
                    catch { break; }
                }
                else
                {
                    CompressOldFile(fileName);
                    fileName = val.GetFileName(fldrName, logrName);    
                }               
            }
            tc.Close();
            CompressOldFile(currentFileName);
            if (setFlag)
            {                                
                LogData();
            }
            else
            {                
                this.StateFlag = 0;
            }                        
        }

        public TcpClient GetTcpClient()
        {
            TcpClient tc = new TcpClient();
            tc.ReceiveTimeout = 10000;
            while (setFlag)
            {
                try
                {
                    tc.Connect(ipAddr, prt);
                    break;
                }
                catch { Thread.Sleep(5000); }
            }
            return tc;
        }

        public void KeepItRunning()
        {
            while (setFlag)
            {
                if (this.StateFlag == 0)
                {
                    this.StateFlag = 1;
                    //t.Start();
                }
            }
        }
        

        public long ReturnFileSize(string fileName)
        {
            long s1;
            if (File.Exists(fileName))
            {
                FileInfo f = new FileInfo(fileName);
                s1 = f.Length;
            }
            else
            {
                s1 = 0;
            }
            return s1;
        }

        public void CompressOldFile(string fileName)
        {
            try
            {
                string allData = File.ReadAllText(fileName);
                File.Delete(fileName);
                Methods.CompressStringToFile(fileName + ".gz", allData);
            }
            catch {}
        }
    }
}