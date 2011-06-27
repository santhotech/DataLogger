using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using NUnit.Framework;

namespace DataLogger
{
    [TestFixture]
    class LoggerTest
    {
        [Test]
        public void CheckFileSizeReturnZero()
        {
            Logger l = new Logger();
            long a = l.ReturnFileSize("asd");
            Assert.AreEqual(a, 0);
            long b = l.ReturnFileSize("");
            Assert.AreEqual(b, 0);
        }
        [Test]
        public void CheckFileSizeReturnSize()
        {
            Logger lg = new Logger();
            System.IO.File.WriteAllBytes("fileTestLogger.txt", new byte[100]);
            FileInfo f = new FileInfo("fileTestLogger.txt");
            long s1 = f.Length;
            long a = lg.ReturnFileSize("fileTestLogger.txt");
            File.Delete("fileTestLogger.txt");
            Assert.AreEqual(s1, a);            
        }
        
    }
}
