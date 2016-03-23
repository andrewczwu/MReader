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
    public class StatCalcFactoryTests
    {
        [TestMethod()]
        [ExpectedException(typeof(StatCalcNotFoundException))]
        public void StatCalc_Invalid_Calculator_Should_Throw_Exception()
        {
            new StatCalcFactory().GetInstance("test");
        }

        [TestMethod()]
        public void StatCalc_Minvalue_Command_Returns_MinValueCalc()
        {
            iStatCalc actualClass = new StatCalcFactory().GetInstance("minvalue");
            Assert.IsInstanceOfType(actualClass, typeof(MinValueCalc));
        }

        [TestMethod()]
        public void StatCalc_Minvalue_Case_Insensitive_Command_Returns_MinValueCalc()
        {
            iStatCalc actualClass = new StatCalcFactory().GetInstance("MiNvalue");
            Assert.IsInstanceOfType(actualClass, typeof(MinValueCalc));
        }

        [TestMethod()]
        public void StatCalc_Maxvalue_Command_Returns_MaxValueCalc()
        {
            iStatCalc actualClass = new StatCalcFactory().GetInstance("maxvalue");
            Assert.IsInstanceOfType(actualClass, typeof(MaxValueCalc));
        }

        [TestMethod()]
        public void StatCalc_Average_Command_Returns_AverageCalc()
        {
            iStatCalc actualClass = new StatCalcFactory().GetInstance("average");
            Assert.IsInstanceOfType(actualClass, typeof(AverageCalc));
        }
    }
}