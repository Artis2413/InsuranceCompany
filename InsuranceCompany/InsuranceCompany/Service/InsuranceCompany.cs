using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceCompany
{
    public interface InsuranceCompany
    {
        string Name { get; }

        IList<Risk> AviaableRisks { get; set; }

        IPolicy SellPolicy(string nameOfInsuredObject, DateTime validFrom, short validMonths, IList<Risk> selectedRisks);

        void AddRisk(string nameOfInsuredObject, Risk risk, DateTime validFrom, DateTime effectiveDate);

        void RemoveRisk(string nameOfInsuredObject, Risk risk, DateTime validTill, DateTime effectiveDate);

        IPolicy GetPolicy(string nameOfInsuredObject, DateTime validFrom);
    }
}
