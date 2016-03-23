using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Milliman
{
    /*
        Extract out the dependency on the file system so we can write tests against the interface        
    */

    public interface iRepository
    {
        string ConfigName { get; } // used for logging or error output
        string TotalTempName { get; }

        string[] ReadConfiguration();
        string[] ReadTotalTemp();
        void WriteOutput(string[] lines);
    }

    /*
        The actual file system reading and writing
    */

    public class FileRepository : iRepository
    {
        string _configFileName = null;
        string _totalTempFileName = null;
        string _outputFileName = null;

        public FileRepository(string configFileName, string totalTempFileName, string outputFileName)
        {
            _configFileName = configFileName;
            _totalTempFileName = totalTempFileName;
            _outputFileName = outputFileName;
        }

        public string ConfigName { get { return _configFileName; } }
        public string TotalTempName { get { return _totalTempFileName; } }

        public string[] ReadConfiguration()
        {
            return File.ReadAllLines(_configFileName);
        }

        public string[] ReadTotalTemp()
        {
            return File.ReadAllLines(_totalTempFileName);
        }

        public void WriteOutput(string[] lines)
        {
            using (StreamWriter outputFile = new StreamWriter(_outputFileName))
            {
                // finally output results
                foreach (string line in lines)
                {
                    outputFile.WriteLine(line);
                }
            }
        }
    }
}
