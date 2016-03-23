using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milliman
{
    /*
        This holds the PeriodSelector and the StatCalculator for a line of configuration. 
        It also holds the values it needs to calculate from the TotalTempFile. 
    */

    public interface IConfigRuleFactory
    {
        ConfigRule CreateConfigRule(string variableName, string statCalcName, string periodSelectName);
    }

    public class ConfigRuleFactory : IConfigRuleFactory
    {
        IPeriodSelectFactory _selectFactory;
        IStatCalcFactory _statCalcFactory;
        public ConfigRuleFactory(IPeriodSelectFactory selectFactory, IStatCalcFactory statCalcFactory)
        {
            _selectFactory = selectFactory;
            _statCalcFactory = statCalcFactory;
        }
        public ConfigRule CreateConfigRule(string variableName, string statCalcName, string periodSelectName)
        {
            ConfigRule rule = new ConfigRule(variableName, statCalcName, periodSelectName);
            rule.PeriodSelect = _selectFactory.GetInstance(periodSelectName);
            rule.StatCalc = _statCalcFactory.GetInstance(statCalcName);
            return rule;
        }
    }

    public class ConfigRule
    {
        private string _variableName;
        private string _statCalcName;
        private iStatCalc _statCalc;
        private iPeriodSelect _periodSelect;
        private List<double> _valuesToCompute;

        public ConfigRule( string VariableName, string StatCalcName, string PeriodSelectName)
        {
            _variableName = VariableName;
            _statCalcName = StatCalcName;

            _valuesToCompute = new List<double>();
        }
        public string VariableName { get { return _variableName; } }
        public string StatCalcName { get { return _statCalcName; } }
        public iStatCalc StatCalc
        {
            get { return _statCalc; }
            set { _statCalc = value; }
        }
        public iPeriodSelect PeriodSelect
        {
            get { return _periodSelect; }
            set { _periodSelect = value; }
        }
        public List<double> ValuesToCompute { get { return _valuesToCompute; } }
    }
}
