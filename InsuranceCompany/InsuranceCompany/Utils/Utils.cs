using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceCompany
{
    public class Utils
    {
        public static bool IsBetween(DateTime input, DateTime date1, DateTime date2)
        {
            return (input > date1 && input < date2);
        }

        public static bool IsBetweenInclusive(DateTime input, DateTime date1, DateTime date2)
        {
            return (input >= date1 && input <= date2);
        }

        public static bool IsInPast(DateTime dateTime)
        {            
            return DateTime.Compare(DateTime.Now, dateTime) > 0;
        }

        public static bool IsNullOrEmpty<T>(IList<T> list)
        {            
            return list is null || list.Count.Equals(0);
        }

        public static decimal CalculatePremiumFromRisksAndMonths(IList<Risk> selectedRisks, short validMonths)
        {
            decimal sum = 0M;
            foreach (Risk risk in selectedRisks)
            {
                sum += risk.YearlyPrice;
            }
            return sum / 12 * validMonths;
        }
    }
}
