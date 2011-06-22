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

        private int stateFlag;

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
        //Thread keepAlive;
        public Logger(string[] contents)
        {
            this.contents = contents;            
            
            val = new Methods();
            
        }

        public void StartLogging()
        {
            /*
            keepAlive = new Thread(new ThreadStart(KeepItRunning));
            keepAlive.IsBackground = true;
            keepAlive.Start();
             * 
             * 
             * */                      

            setFlag = true;

            this.StateFlag = 2;

            Thread t = new Thread(new ThreadStart(LogData));
            t.IsBackground = true;            
            t.Start();            
        }

        public void StopLoggin()
        {
            setFlag = false;
            this.StateFlag = 1;
        }

        public void LogData()
        {
            this.StateFlag = 2;
            string logrName = contents[0];
            string fldrName = contents[4];
            long fileSize = val.GetBytesAsLong(contents[3]);            
            string fileName = val.GetFileName(fldrName, logrName);
            int prt = Convert.ToInt32(contents[2]);

            TcpClient tc = new TcpClient();
            tc.ReceiveTimeout = 5000;
            try
            {
                tc.Connect(contents[1], prt);
            }
            catch 
            {
                
            }
            
                  
            while (setFlag)
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
                        currentFileName = fileName;
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
                            break;
                        }
                    }
                    else
                    {
                        CompressOldFile(fileName);
                        fileName = val.GetFileName(fldrName, logrName);    
                    }
                }
                catch
                {                                               
                    break;
                }
            }
            tc.Close();            
            if (setFlag)
            {                
                CompressOldFile(currentFileName);
                LogData();
            }
            else
            {                
                this.StateFlag = 0;
            }            
            CompressOldFile(currentFileName);
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