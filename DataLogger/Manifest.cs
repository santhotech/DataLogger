using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace DataLogger
{
    public class Manifest
    {
        public string[] allContents { get; set; }
        public int currentAction { get; set; }
        public ListViewItem listIndex { get; set; }
        public Button ctrlBtn { get; set; }
        public Button delBtn { get; set; }
        public Logger logger { get; set; }
        public string[] selectedContents { get; set; }
    }
}
