using Microsoft.VisualStudio.TestTools.UnitTesting;
using Milliman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milliman.Tests
{
    public class TestConfigReader : ConfigReader
    {
        public TestConfigReader(iRepository repository, IConfigRuleFactory configRuleFactory) : base (repository, configRuleFactory) { }

        public new ConfigRule ParseLine(string line) { return base.ParseLine(line); }
    }

    [TestClass()]
    public class ConfigReaderTests 
    {
        protected iRepository _testRepository;
        protected IConfigRuleFactory _iConfigRuleFactory;
        protected TestConfigReader _testConfigReader;


        [TestInitialize()]
        public void Initialize()
        {
            string[] testConfigBuffer = { "" };
            string[] testReaderBuffer = { "" };
            _testRepository = new TestRepository(testConfigBuffer, testReaderBuffer);
            _iConfigRuleFactory = new ConfigRuleFactory(new PeriodSelectFactory(), new StatCalcFactory());
            _testConfigReader = new TestConfigReader(_testRepository, _iConfigRuleFactory);
        }

        [TestMethod()]
        public void ConfigReader_ParseLine_LessThan3_TabDelimited_Should_Return_Null()
        {
            string testline = "";
            ConfigRule expected = _testConfigReader.ParseLine(testline);
            Assert.IsNull(expected);

            testline = "abab";
            expected = _testConfigReader.ParseLine(testline);
            Assert.IsNull(expected);

            testline = "test1\ttest2";
            expected = _testConfigReader.ParseLine(testline);
            Assert.IsNull(expected);
        }

        [TestMethod()]
        public void ConfigReader_ParseLine_ValidCommand_Should_Return_Proper_ConfigRule()
        {
            string testline = "CashPrem\tAverage\tMaxValue";
            ConfigRule expected = _testConfigReader.ParseLine(testline);
            Assert.IsInstanceOfType(expected, typeof(ConfigRule));
        }

        
    }
}