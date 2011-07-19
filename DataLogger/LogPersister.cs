using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace DataLogger
{
    class LogPersister
    {
        private ArrayList _persisterLoad;
        private ArrayList _persisterSave;
        public LogPersister()
        {
            _persisterLoad = new ArrayList();
            _persisterSave = new ArrayList();

        }
        public ArrayList LoadExisitingLoggers()
        {
            string prevLoggers = Properties.Settings.Default.loggers;
            if (prevLoggers != string.Empty)
            {
                string[] splitLogger = prevLoggers.Split('|');
                foreach (string str in splitLogger)
                {
                    string[] logrInfo = str.Split(',');
                    if (logrInfo.Length == 6)
                    {
                        _persisterLoad.Add(logrInfo);                                                
                    }
                }
            }
            return _persisterLoad;
        }

        public void SaveLogger(string[] sc, int action)
        {
            string persistLoggers = string.Empty;
            if (action == 1)
            {
                _persisterSave.Add(sc);
            }
            else if (action == 2)
            {
                _persisterSave.Remove(sc);
            }
            foreach (string[] sCol in _persisterSave)
            {
                persistLoggers += string.Join(",", sCol) + "|";
            }
            Properties.Settings.Default.loggers = persistLoggers;
            Properties.Settings.Default.Save();
        }
    }
}
