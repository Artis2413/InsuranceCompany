using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceCompany
{
    public interface IPolicy
    {
        string NameOfInsuredObject { get; set; }

        DateTime ValidFrom { get; set; }

        DateTime ValidTill { get; set; }

        decimal Premium { get; set; }

        IList<Risk> InsuredRisks { get; set; }
    }
}
