using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MatlabConnection;


namespace MatlabConnectionTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            object[] res = Connection.Instance.ExecuteCommand();
            double? result = 1;
            result = res[0] as double?;
            Assert.AreEqual(2.0, res[0]);
        }
    }
}
