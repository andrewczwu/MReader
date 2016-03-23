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
    public class PeriodSelectTests
    {
        [TestMethod()]
        public void FirstValuePeriodSelect_Returns_First_Column()
        {            
            var testContext = new List<double> { 3.01d, 1.02d, 5.0d, 2235.33d };
            double expectedResult = 3.01d;
            double actualResult = new FirstValuePeriodSelect().SelectPeriod(testContext);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void LastValuePeriodSelect_Returns_Last_Column()
        {           
            var testContext = new List<double> { 3.01d, 1.02d, 5.0d, 2235.33d };
            double expectedResult = 2235.33d;
            double actualResult = new LastValuePeriodSelect().SelectPeriod(testContext);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void MaxValuePeriodSelect_Returns_Max_Column()
        {            
            var testContext = new List<double> { 3.01d, 1.02d, 53212.0d, 2235.33d };
            double expectedResult = 53212.0d;
            double actualResult = new MaxValuePeriodSelect().SelectPeriod(testContext);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void MinValuePeriodSelect_Returns_Min_Column()
        {
            var testContext = new List<double> { 3.01d, 1.02d, 53212.0d, 2235.33d };
            double expectedResult = 1.02d;
            double actualResult = new MinValuePeriodSelect().SelectPeriod(testContext);
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}