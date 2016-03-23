using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Milliman
{
    /*
        Just a container for methods that deal with the processing of the configuration file
    */

    public class ConfigReader
    {
        private readonly iRepository _repository;
        private readonly IConfigRuleFactory _configRuleFactory;
        public ConfigReader(iRepository repository, IConfigRuleFactory configRuleFactory)
        {
            this._repository = repository;
            this._configRuleFactory = configRuleFactory;
        }
        public List<ConfigRule> Read()
        {
            List<ConfigRule> configRules = new List<ConfigRule>();
            try
            {
                foreach (string line in _repository.ReadConfiguration())
                {
                    ConfigRule rule = ParseLine(line);
                    if (rule != null)
                    {
                        configRules.Add(rule);
                    }
                }
            }
            catch (IOException ex)
            {
                // do some logging with exception
                Console.WriteLine("Error in reading " + _repository.ConfigName);
                throw;
            }
            return configRules;
        }

        protected ConfigRule ParseLine(string line)
        {
            ConfigRule configRule = null;
            string[] parsedLine = line.Split('\t');
            if (parsedLine.Length < 3)
            {
                Console.WriteLine("Line should have 3 fields, line content:" + line);
                return null;
            }
            try
            {
                configRule = _configRuleFactory.CreateConfigRule(parsedLine[0], parsedLine[1], parsedLine[2]);
            }
            catch (Exception ex)
            {
                // do some logging with exception
                Console.WriteLine("Error in parsing line content: " + line);
                throw;
            }
            return configRule;
        }
    }
}
