using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;


namespace DataLogger
{
    [TestFixture]
    public class MethodsTest
    {
        [Test]
        public void CheckValidationFail()
        {
            Methods validate = new Methods();
            string[] a = new string[4] { "test", "test1", "test2", "" };
            bool k = validate.ValidateForm(a);
            Assert.False(k);
            string[] b = new string[4] { "test", "", "  ", "test3" };
            bool j = validate.ValidateForm(b);
            Assert.False(j);
        }
        [Test]
        public void CheckValidationPass()
        {
            Methods validate = new Methods();
            string[] a = new string[4] { "asdd", "asa", "asdd", "dnjn" };
            bool k = validate.ValidateForm(a);
            Assert.True(k);
        }        
    }
}
