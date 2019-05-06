using InsuranceCompany.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceCompany
{
    public class Validators
    {
        public static bool IsValidRisks(IList<Risk> risks)
        {
            foreach (Risk risk in risks)
            {
                foreach (Risk risk1 in risks)
                {
                    if (risk.Name.Equals(risk1.Name) && !risk.YearlyPrice.Equals(risk1.YearlyPrice))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool CanSellPolicy(IList<Policy> Policies, string nameOfInsuredObject, DateTime validFrom)
        {
            foreach (Policy policy in Policies)
            {
                if (policy.NameOfInsuredObject.Equals(nameOfInsuredObject) &&
                    Utils.IsBetweenInclusive(validFrom, policy.ValidFrom, policy.ValidTill))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
