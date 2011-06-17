using System;
using System.Collections.Generic;
using System.Text;

namespace DataLogger
{
    class validation
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
    }
}
