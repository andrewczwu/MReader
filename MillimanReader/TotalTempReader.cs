using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Milliman
{
    /*
        Container for set of methods that deal with reading and parsing the TotalTemp file
    */

    public class TotalTempReader
    {
        private readonly iRepository _repository;

        public TotalTempReader(iRepository repository)
        {
            _repository = repository;
        }
        /*
            ParseReturnCodes is used to avoid magic numbers
        */
        public enum ParseReturnCodes
        {
            ParseError = -1,
            Skipped = 0,
            Processed = 1
        }

        public void Read(List<ConfigRule> configRules)
        {
            int lineNumber = 0;
            int columns = 0;
            try
            {
                foreach (string line in _repository.ReadTotalTemp())
                {
                    if (lineNumber == 0)
                    {
                        // process header
                        ParseReturnCodes ret = ParseHeader(line, out columns);
                        if (ret == ParseReturnCodes.ParseError)
                        {
                            Console.WriteLine("Can't process header, exiting");
                            return;
                        }
                    }
                    else
                    {                        
                        foreach (ConfigRule rule in configRules)
                        {
                            ParseLine(line, rule, columns, lineNumber);
                        }
                    }
                    lineNumber++;
                }
            }
            catch (IOException ex)
            {
                // do some logging
                Console.WriteLine("Error in on line " + lineNumber.ToString() + " in file " + _repository.TotalTempName);
                throw;
            }            
        }
        
        public ParseReturnCodes ParseHeader(string line, out int columns)
        {
            columns = 0;
            string[] parsedLine = line.Split('\t');
            if (parsedLine.Length < 3)
            {
                Console.WriteLine("Header does not have enough columns");
                return ParseReturnCodes.ParseError;
            }

            if (parsedLine[0].ToLower() != "scenid")
            {
                Console.WriteLine("First column must be ScenId");
                return ParseReturnCodes.ParseError;
            }

            if (parsedLine[1].ToLower() != "varname")
            {
                Console.WriteLine("Second column must be VarName");
                return ParseReturnCodes.ParseError;
            }
            columns = parsedLine.Length;
            return ParseReturnCodes.Processed;
        }

        public ParseReturnCodes ParseLine(string line, ConfigRule rule, int expectedColumnCount, int lineNumber)
        {
            // split the line on tabs
            string[] parsedLine = line.Split('\t');

            if (parsedLine.Length < 3)
            {
                Console.WriteLine("Error in line number: " + lineNumber + " Line must have at least 3 columns");
                return ParseReturnCodes.ParseError;
            }

            if (expectedColumnCount != parsedLine.Count())
            {
                Console.WriteLine("Error in line number: " + lineNumber + " Line does not have the same number of columns as header");
                return ParseReturnCodes.ParseError;
            }

            // only process if variable name matches
            string variableName = parsedLine[1];
            if (rule.VariableName.ToLower() != variableName.ToLower()) //make sure case insensitive
            {
                return ParseReturnCodes.Skipped;
            }
            
            List<double> lineValues = null;
            // convert the rest of the columns into a double list
            try
            {
                lineValues = parsedLine.Skip(2).Select(s => double.Parse(s)).ToList();
            }
            catch
            {
                Console.WriteLine("Error in line number: " + lineNumber + " Values can't be converted to double number");
                return ParseReturnCodes.ParseError;
            }

            rule.ValuesToCompute.Add(rule.PeriodSelect.SelectPeriod(lineValues));

            return ParseReturnCodes.Processed;
        }
    }
}
