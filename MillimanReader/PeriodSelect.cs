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



    public interface iPeriodSelect
    {
        double SelectPeriod(List<double> context);
    }

    public class FirstValuePeriodSelect : iPeriodSelect
    {
        public double SelectPeriod(List<double> context)
        {
            return context.First();
        }
    }

    public class LastValuePeriodSelect : iPeriodSelect
    {
        public double SelectPeriod(List<double> context)
        {
            return context.Last();
        }
    }

    public class MaxValuePeriodSelect : iPeriodSelect
    {
        public double SelectPeriod(List<double> context)
        {
            return context.Max();
        }
    }

    public class MinValuePeriodSelect : iPeriodSelect
    {
        public double SelectPeriod(List<double> context)
        {
            return context.Min();
        }
    }

    public class PeriodSelectNotFoundException : Exception
    {
        public PeriodSelectNotFoundException() : base() { }
        public PeriodSelectNotFoundException(string message) : base(message) { }
    }

    public interface IPeriodSelectFactory
    {
        iPeriodSelect GetInstance(string type);
    }

    public class PeriodSelectFactory : IPeriodSelectFactory
    {
        private static Dictionary<string, iPeriodSelect> _selectors = new Dictionary<string, iPeriodSelect>();
        static PeriodSelectFactory()
        {
            _selectors.Add("firstvalue", new FirstValuePeriodSelect());
            _selectors.Add("lastvalue", new LastValuePeriodSelect());
            _selectors.Add("minvalue", new MinValuePeriodSelect());
            _selectors.Add("maxvalue", new MaxValuePeriodSelect());
        }

        public iPeriodSelect GetInstance(string type)
        {
            iPeriodSelect selectorFound = null;
            _selectors.TryGetValue(type.ToLower(), out selectorFound);
            if (selectorFound == null)
            {
                throw new PeriodSelectNotFoundException("period selector not found");
            }

            return selectorFound;
        }
    }

}
