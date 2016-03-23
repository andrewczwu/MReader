using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milliman
{     
    public static class ProgramEntry
    {
        /*
            For simplicity, specify 1st arg as configfile, 2nd as totaltemp file, 3rd as output file
            if parameters are not specified, default to Configuration.txt TotalTemp.txt Output.txt
            no named parameters for this simple solution
        */
        public static void Main(string[] args)
        {
            string configFileName = "Configuration.txt";
            if (args.Length > 0)
            {
                configFileName = args[0];
            }
            string totalTempFileName = "TotalTemp.txt";
            if (args.Length > 1)
            {
                totalTempFileName = args[1];
            }
            string outputFileName = "Output.txt";
            if (args.Length > 2)
            {
                outputFileName = args[2];
            }
            
            var container = new UnityContainer();
            container.RegisterType<iRepository>(new InjectionFactory(_ => new FileRepository(configFileName, totalTempFileName, outputFileName)));
            container.RegisterType<IStatCalcFactory, StatCalcFactory>();
            container.RegisterType<IPeriodSelectFactory, PeriodSelectFactory>();
            container.RegisterType<IConfigRuleFactory, ConfigRuleFactory>();
            var program = container.Resolve<MillimanReader>();            
            program.RunReader();           
        }
    }
    public class MillimanReader
    {
        protected readonly iRepository _repository;
        protected readonly ConfigReader _configReader;
        protected readonly TotalTempReader _totalTempReader;
        public MillimanReader(iRepository repository, ConfigReader configReader, TotalTempReader totalTempReader)
        {
            this._repository = repository;
            this._configReader = configReader;
            this._totalTempReader = totalTempReader;
        }

        public void RunReader()
        {
            Console.WriteLine("Reading Config file");
            List<ConfigRule> rules = _configReader.Read();

            Console.WriteLine("Reading TotalTemp file");
            _totalTempReader.Read(rules);

            Console.WriteLine("Writing Output file");
            List<string> output = new List<string>();
            // finally output results
            foreach (ConfigRule rule in rules)
            {
                if (rule.ValuesToCompute.Count > 0)
                {
                    double calcValue = rule.StatCalc.ExecuteCalc(rule.ValuesToCompute);
                    output.Add(rule.VariableName + "\t" + rule.StatCalcName + "\t" + calcValue.ToString("F2"));
                }
                else
                {
                    output.Add(rule.VariableName + "\t" + rule.StatCalcName + "\tNo values found");
                }
            }
            _repository.WriteOutput(output.ToArray());
        }
    }
}
