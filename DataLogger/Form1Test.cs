using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;


namespace DataLogger
{
    [TestFixture]
    public class Form1Test
    {
        [Test]
        public void CheckValidationFail()
        {
            validation validate = new validation();
            string[] a = new string[4] { "asdd", "asa", "asdd", "" };
            bool k = validate.ValidateForm(a);
            Assert.False(k);
        }
        [Test]
        public void CheckValidationPass()
        {
            validation validate = new validation();
            string[] a = new string[4] { "asdd", "asa", "asdd", "dnjn" };
            bool k = validate.ValidateForm(a);
            Assert.True(k);
        }
        
    }
}
