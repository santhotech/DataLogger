using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace DataLogger
{
    class Methods
    {
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

        public string GetUniqueId(Hashtable ht)
        {
            string uniqueId = RandomString(10, true);
            while (ht.ContainsKey(uniqueId))
            {
                uniqueId = RandomString(10, true);
            }
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
    }
}
