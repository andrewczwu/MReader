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
    public class TotalTempReaderTests
    {
        TestRepository _testRepository;
        TotalTempReader _reader;
        ConfigRuleFactory configRuleFactory = new ConfigRuleFactory(new PeriodSelectFactory(), new StatCalcFactory());
        [TestInitialize()]
        public void Initialize()
        {
            string[] testConfigBuffer = { "" };
            string[] testReaderBuffer = { "" };
            _testRepository = new TestRepository(testConfigBuffer, testReaderBuffer);
            _reader = new TotalTempReader(_testRepository);
        }

        [TestMethod()]
        public void ParseHeader_Less_Than3_Should_Fail()
        {
            string header = "";
            int columns;
            TotalTempReader.ParseReturnCodes actual = _reader.ParseHeader(header, out columns);
            TotalTempReader.ParseReturnCodes expected = TotalTempReader.ParseReturnCodes.ParseError;
            Assert.AreEqual(actual, expected);

            header = "column1\tcolumn2";
            actual = _reader.ParseHeader(header, out columns);
            Assert.AreEqual(actual, expected);
        }

        [TestMethod()]
        public void ParseHeader_First_Column_Not_Scenid_Should_Fail()
        {
            string header = "column1\tcolumn2\tcolumn3";
            int columns;
            TotalTempReader.ParseReturnCodes actual = _reader.ParseHeader(header, out columns);
            TotalTempReader.ParseReturnCodes expected = TotalTempReader.ParseReturnCodes.ParseError;
            Assert.AreEqual(actual, expected);
        }

        [TestMethod()]
        public void ParseHeader_Second_Column_Not_Scenid_Should_Fail()
        {
            string header = "scenid\tcolumn2\tcolumn3";
            int columns;
            TotalTempReader.ParseReturnCodes actual = _reader.ParseHeader(header, out columns);
            TotalTempReader.ParseReturnCodes expected = TotalTempReader.ParseReturnCodes.ParseError;
            Assert.AreEqual(actual, expected);
        }

        [TestMethod()]
        public void ParseLine_ExpectedColumns_Not_Match_Actual_Fails()
        {
            string line = "column1\tcolumn2\tcolumn3";
            int expectedColumns = 4;
            ConfigRule rule = configRuleFactory.CreateConfigRule("var1", "maxvalue", "maxvalue");
            TotalTempReader.ParseReturnCodes actual = _reader.ParseLine(line, rule, expectedColumns, 1);
            Assert.AreEqual(TotalTempReader.ParseReturnCodes.ParseError, actual);

        }

        [TestMethod()]
        public void ParseLine_NotMatched_Variable_Should_Skip()
        {
            string line = "1\tVariable1\tValue1";
            ConfigRule rule = configRuleFactory.CreateConfigRule("var1", "maxvalue", "maxvalue");
            int expectedColumns = 3;
            TotalTempReader.ParseReturnCodes actual = _reader.ParseLine(line, rule, expectedColumns, 1);
            Assert.AreEqual(TotalTempReader.ParseReturnCodes.Skipped, actual);
        }

        [TestMethod()]
        public void ParseLine_Parses_Variable_Normal_Case()
        {
            string line = "1\tVariable1\t5.63\t4.33";
            ConfigRule rule = configRuleFactory.CreateConfigRule("Variable1", "MaxValue", "MaxValue");
            int expectedColumns = 4;
            TotalTempReader.ParseReturnCodes actual = _reader.ParseLine(line, rule, expectedColumns, 1);
            Assert.AreEqual(rule.ValuesToCompute[0], 5.63d);
            Assert.AreEqual(TotalTempReader.ParseReturnCodes.Processed, actual);
        }

        [TestMethod()]
        public void ParseLine_Parses_Variable_Invalid_Data_Returns_Error()
        {
            string line = "1\tVariable1\t5.63\t4.3b3";
            ConfigRule rule = new ConfigRule("Variable1", "MaxValue", "MaxValue");
            int expectedColumns = 4;
            TotalTempReader.ParseReturnCodes actual = _reader.ParseLine(line, rule, expectedColumns, 1);
            Assert.AreEqual(rule.ValuesToCompute.Count(), 0);
            Assert.AreEqual(TotalTempReader.ParseReturnCodes.ParseError, actual);
        }
    }
}