using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milliman
{
    /*
        This uses the Strategy design pattern to encapsulate logic and make it easy to add rules 
        without other parts of the application needing to know about it.
        We also use a Factory method pattern to get rid of a large ugly case statement
    */

    public interface iStatCalc
    {
        double ExecuteCalc(List<double> context);
    }

    public class MinValueCalc : iStatCalc
    {
        public double ExecuteCalc(List<double> context)
        {
            return context.Min();
        }
    }

    public class MaxValueCalc : iStatCalc
    {
        public double ExecuteCalc(List<double> context)
        {
            return context.Max();
        }
    }

    public class AverageCalc : iStatCalc
    {
        public double ExecuteCalc(List<double> context)
        {
            return context.Average();
        }
    }

    public class StatCalcNotFoundException : Exception
    {
        public StatCalcNotFoundException() : base() { }
        public StatCalcNotFoundException(string message) : base(message) { }
    }

    public interface IStatCalcFactory
    {
        iStatCalc GetInstance(string type);
    }

    public class StatCalcFactory : IStatCalcFactory
    {
        private static Dictionary<string, iStatCalc> _calculators = new Dictionary<string, iStatCalc>();
        static StatCalcFactory()
        {
            _calculators.Add("minvalue", new MinValueCalc());
            _calculators.Add("maxvalue", new MaxValueCalc());
            _calculators.Add("average", new AverageCalc());
        }

        public iStatCalc GetInstance(string type)
        {
            iStatCalc calcFound = null;
            _calculators.TryGetValue(type.ToLower(), out calcFound);
            if (calcFound == null)
            {
                throw new StatCalcNotFoundException("calculator not found");
            }

            return calcFound;
        }
    }


}
