using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace DataLogger
{
    public class Logger
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
        private bool pongSrchBuffer;
        private bool isPingNeeded;
        private LoggerLog logEntry;

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
            isPingNeeded = Convert.ToBoolean(contents[6]);
            logEntry = new LoggerLog(logrName, fldrName);   
        }

        public void StartLogging()
        {
            fileName = val.GetFileName(fldrName, logrName);
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

        public void SendPing(object tClient)
        {
            TcpClient tc = (TcpClient)tClient;
            Byte[] outStream = System.Text.Encoding.ASCII.GetBytes("* PING");
            while (setFlag)
            {
                try
                {
                    tc.GetStream().Write(outStream, 0, outStream.Length);
                    logEntry.WriteLog("Ping Sent");
                    Thread.Sleep(20000);
                    if (pongSrchBuffer) {
                        logEntry.WriteLog("Pong Recieved");
                        pongSrchBuffer = false; 
                    }
                    else { setFlag = false; break; }
                }
                catch { break; }
            }            
        }
        
        public void LogData()
        {
            TcpClient tc = GetTcpClient();
            if (isPingNeeded)
            {
                Thread t = new Thread(new ParameterizedThreadStart(SendPing));
                t.IsBackground = true;
                t.Start(tc);
            }                          
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
                        // this is where it was timing out on its own, after ur recent comment ive commented this out.
                        //if (actuallyRead == 0) break;
                        string decodedData = string.Empty;
                        decodedData = System.Text.Encoding.ASCII.GetString(instream, 0, actuallyRead);
                        decodedData = decodedData.Trim('\0');
                        decodedData = decodedData.Replace("\n", "\r\n");
                        if (decodedData.Contains("PONG")) { pongSrchBuffer = true; }
                        File.AppendAllText(fileName, decodedData);
                    }
                    catch(Exception ex) 
                    {
                        logEntry.WriteLog("Disconnected from Adapter - " + ex.ToString());
                        break;  
                    }
                }
                else
                {                    
                    fileName = val.GetFileName(fldrName, logrName);    
                }               
            }
            logEntry.WriteLog("Disconnected from adapter because adapter closed the connection");
            tc.Close();            
            if (setFlag)
            {                                
                LogData();
            }
            else
            {
                CompressOldFile(currentFileName);
                this.StateFlag = 0;
            }                        
        }

        public TcpClient GetTcpClient()
        {
            TcpClient tc = new TcpClient();
            tc.ReceiveTimeout = 30000;
            while (setFlag)
            {
                try
                {
                    logEntry.WriteLog("Trying to connect to the adapter");
                    tc.Connect(ipAddr, prt);
                    logEntry.WriteLog("Connection successful");
                    break;
                }
                catch(Exception ex) 
                {
                    logEntry.WriteLog("Connection Failed - reason - " + ex.ToString());
                    Thread.Sleep(5000); 
                }
            }
            return tc;
        }
                
        public long ReturnFileSize(string fileName)
        {
            long s1;
            if (File.Exists(fileName))
            {
                FileInfo f = new FileInfo(fileName);
                s1 = f.Length;
            }
            else { s1 = 0; }
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