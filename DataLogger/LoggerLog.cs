using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace DataLogger
{
    class LoggerLog
    {
        private string loggerName;
        private string fileName;
        private string loggerPath;
        public LoggerLog(string logrName, string logrPath)
        {
            loggerName = logrName;
            loggerPath = logrPath;
            fileName = loggerPath + "\\loggerlog.txt";
        }

        public void WriteLog(string statement)
        {
            string ts = DateTime.UtcNow.ToString("s");
            statement = "[logger] " + "[" + ts +"]" + loggerName + "[logentry] " + statement + "[EOL]" + Environment.NewLine;
            File.AppendAllText(fileName, statement);
        }

    }
}
