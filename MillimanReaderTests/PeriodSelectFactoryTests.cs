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
    public class PeriodSelectFactoryTests
    {
        [TestMethod()]
        [ExpectedException(typeof(PeriodSelectNotFoundException))]
        public void PeriodSelect_Invalid_Period_Should_Throw_Exception()
        {
            new PeriodSelectFactory().GetInstance("invalid");
        }

        [TestMethod()]
        public void PeriodSelect_FirstValue_Selector_Returns_FirstValue()
        {
            iPeriodSelect actualClass = new PeriodSelectFactory().GetInstance("firstvalue");
            Assert.IsInstanceOfType(actualClass, typeof(FirstValuePeriodSelect));
        }

        [TestMethod()]
        public void PeriodSelect_Case_Insensitive_FirstValue_Selector_Returns_FirstValue()
        {
            iPeriodSelect actualClass = new PeriodSelectFactory().GetInstance("FirstvaLue");
            Assert.IsInstanceOfType(actualClass, typeof(FirstValuePeriodSelect));
        }

        [TestMethod()]
        public void PeriodSelect_LastValue_Selector_Returns_LastValue()
        {
            iPeriodSelect actualClass = new PeriodSelectFactory().GetInstance("lastvalue");
            Assert.IsInstanceOfType(actualClass, typeof(LastValuePeriodSelect));
        }

        [TestMethod()]
        public void PeriodSelect_MinValue_Selector_Returns_MinValue()
        {
            iPeriodSelect actualClass = new PeriodSelectFactory().GetInstance("minvalue");
            Assert.IsInstanceOfType(actualClass, typeof(MinValuePeriodSelect));
        }

        [TestMethod()]
        public void PeriodSelect_MaxValue_Selector_Returns_MaxValue()
        {
            iPeriodSelect actualClass = new PeriodSelectFactory().GetInstance("maxvalue");
            Assert.IsInstanceOfType(actualClass, typeof(MaxValuePeriodSelect));
        }
    }
}