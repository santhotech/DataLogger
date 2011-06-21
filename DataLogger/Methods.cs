using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO.Compression;
using System.IO;

namespace DataLogger
{
    class Methods
    {
        private ArrayList _loggerNames;
        private ArrayList _threadNames;
        public Methods()
        {
            _loggerNames = new ArrayList();
            _threadNames = new ArrayList();
        }
        public bool ValidateForm(string[] testStrings)
        {           
            foreach (string t in testStrings)
            {
                if (t.Trim() == "")
                {                    
                    return false;
                }
                
            }
            return true;
        }

        public bool CheckLoggerNameExist(string loggerName)
        {
            if (_loggerNames.Contains(loggerName))
            {
                return false;
            }
            else
            {
                _loggerNames.Add(loggerName);
                return true;
            }
        }

        public long GetBytesAsLong(string mb)
        {
            long flSize = Convert.ToInt32(mb) * 1000000;
            return flSize;
        }        

        public bool IsAllDigits(string s)
        {
            foreach (char c in s)
            {
                if (!Char.IsDigit(c))
                    return false;
            }
            return true;
        }

        public string GetUniqueId()
        {
            string uniqueId = RandomString(10, true);
            while (_threadNames.Contains(uniqueId))
            {
                uniqueId = RandomString(10, true);
            }
            _threadNames.Add(uniqueId);
            return uniqueId;
        }
        private string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        public string GetFileName(string fldrName, string logrName)
        {
            string fileName = DateTime.UtcNow.ToString("s");
            fileName = logrName + "-" + fileName;
            fileName = fileName.Replace(':', '-');
            fileName = fileName.Replace(' ', '-');
            fileName = fldrName + "\\" + fileName + ".txt";
            return fileName;
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
            using (FileStream f2 = new FileStream(fileName, FileMode.Create))
            using (GZipStream gz = new GZipStream(f2, CompressionMode.Compress, false))
            {
                gz.Write(b, 0, b.Length);
            }
        }

    }
}
