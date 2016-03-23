using Microsoft.VisualStudio.TestTools.UnitTesting;
using Milliman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milliman.Tests
{
    /*
        This is a set of integration tests for the whole program that extracts out the actual
        file reading and writing
    */
    public class TestRepository : iRepository
    {
        private string[] _config;
        private string[] _totalTemp;
        private string[] _output;

        public TestRepository(string[] config, string[] totalTemp)
        {
            _config = config;
            _totalTemp = totalTemp;
        }

        public string ConfigName
        {
            get
            {
                return "configtest";
            }
        }

        public string TotalTempName
        {
            get
            {
                return "totaltemptest";
            }
        }

        public string[] ReadConfiguration()
        {
            return _config;
        }

        public string[] ReadTotalTemp()
        {
            return _totalTemp;
        }

        public void WriteOutput(string[] lines)
        {
            _output = lines;
        }

        public string[] OutputResult
        {
            get { return _output; }
        }        

    }


    [TestClass()]
    public class MillimanReaderTests
    {
        ConfigRuleFactory configRuleFactory = new ConfigRuleFactory(new PeriodSelectFactory(), new StatCalcFactory());
        string[] totalTempFile = {
            "ScenId	VarName	Value000	Value001	Value002	Value003	Value004	Value005",
            "1	AvePolLoanYield	0	0.04	0.04	0.04	0.04	0.03",
            "1	CashPrem	0	165215335.4	130922548.8	107196660	92462698.42	84655947.13",
            "1	ResvAssumed	-27923645.44	-28437248.89	-29893491.3	-31761676.09	-34092668.16	-36815307.05",
            "2	AvePolLoanYield	0	0.04	0.04	0.04	0.04	0.03",
            "2	CashPrem	0	0	130922548.8	107196444.4	92462698.42	84655914.86",
            "2	ResvAssumed	-27923645.44	-28437248.89	-29893531.02	-31762115.98	-34094542.44	-36821010.24",
            "3	AvePolLoanYield	0	0.04	0.04	0.04	0.04	0.03",
            "3	CashPrem	0	0	0	107196660	92462698.42	84655947.13",
            "3	ResvAssumed	-27923645.44	-28437248.89	-29893482.02	-31761477.7	-34091316.73	-36811494.23"
        };
       
        [TestMethod()]
        public void MillimanReader_SingleRule_Average_MaxValue_Integration_Test()
        {
            string[] singleRuleConfig = { "CashPrem	Average	MaxValue" };

            string[] expectedOutput =
            {
                "CashPrem	Average	134444848.07"
            };

            TestRepository testRepository = new TestRepository(singleRuleConfig, totalTempFile);
            ConfigReader configReader = new ConfigReader(testRepository, configRuleFactory);
            TotalTempReader totalTempReader = new TotalTempReader(testRepository);
            var program = new MillimanReader(testRepository, configReader, totalTempReader);
            program.RunReader();
            CollectionAssert.AreEqual(testRepository.OutputResult, expectedOutput);
        }

        [TestMethod()]
        public void MillimanReader_MultipleRule_Integration_Test()
        {
            string[] multipleRuleConfig = {
                "CashPrem	MinValue	LastValue",
                "AvePolLoanYield	MaxValue	MaxValue",
                "ResvAssumed	Minvalue	MinValue",
                "ResvAssumed	Average	MinValue",
            };

            string[] expectedOutput =
            {
                "CashPrem	MinValue	84655914.86",
                "AvePolLoanYield	MaxValue	0.04",
                "ResvAssumed	Minvalue	-36821010.24",
                "ResvAssumed	Average	-36815937.17"
            };

            TestRepository testRepository = new TestRepository(multipleRuleConfig, totalTempFile);
            ConfigReader configReader = new ConfigReader(testRepository, configRuleFactory);
            TotalTempReader totalTempReader = new TotalTempReader(testRepository);
            var program = new MillimanReader(testRepository, configReader, totalTempReader);
            program.RunReader();
            CollectionAssert.AreEqual(testRepository.OutputResult, expectedOutput);
        }

        [TestMethod()]
        public void MillimanReader_MultipleRule_VariableNotExist_Integration_Test()
        {
            string[] multipleRuleConfig = {
                "CashPrem	MinValue	LastValue",
                "AvePolLoanYield	MaxValue	MaxValue",
                "test	Minvalue	MinValue",
                "ResvAssumed	Average	MinValue",
            };

            string[] expectedOutput =
            {
                "CashPrem	MinValue	84655914.86",
                "AvePolLoanYield	MaxValue	0.04",
                "test	Minvalue	No values found",
                "ResvAssumed	Average	-36815937.17"
            };

            TestRepository testRepository = new TestRepository(multipleRuleConfig, totalTempFile);
            ConfigReader configReader = new ConfigReader(testRepository, configRuleFactory);
            TotalTempReader totalTempReader = new TotalTempReader(testRepository);
            var program = new MillimanReader(testRepository, configReader, totalTempReader);
            program.RunReader();
            CollectionAssert.AreEqual(testRepository.OutputResult, expectedOutput);
        }
    }
}