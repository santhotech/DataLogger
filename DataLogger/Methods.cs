using System;
using System.Collections.Generic;
using System.Text;

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

    }
}
