using Microsoft.VisualStudio.TestTools.UnitTesting;
using Milliman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milliman.Tests
{
    [TestClass()]
    public class StatCalcTests
    {
        [TestMethod()]
        public void MinValue_Returns_Minimum_For_Simple_List()
        {            
            var testContext = new List<double> { 3.01d, 1.02d, 5.0d };
            double expectedResult = 1.02d;
            double actualResult = new MinValueCalc().ExecuteCalc(testContext);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void MaxValue_Returns_Maximum_For_Simple_List()
        {            
            var testContext = new List<double> { 3.01d, 1.02d, 5.0d, 9.23d, 4.22d };
            double expectedResult = 9.23d;
            double actualResult = new MaxValueCalc().ExecuteCalc(testContext);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void Average_Returns_Average_For_Simple_List()
        {            
            var testContext = new List<double> { 3.01d, 1.02d, 5.0d, 9.23d, 4.22d };
            double expectedResult = (3.01d + 1.02d + 5.0d + 9.23d + 4.22d) / 5;
            double actualResult = new AverageCalc().ExecuteCalc(testContext);
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}