using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceCompany.Model
{
    public class Policy
    {
        public Policy(string nameOfInsuredObject, DateTime validFrom, short validMonths, IList<Risk> selectedRisks)
        {
            this.NameOfInsuredObject = nameOfInsuredObject;
            this.ValidFrom = validFrom;         
            this.ValidTill = validFrom.AddMonths(validMonths);           
            this.Premium = Utils.CalculatePremiumFromRisksAndMonths(selectedRisks, validMonths);
            CreteInsuredRisksFromRisks(selectedRisks, validFrom, this.ValidTill);
        }

        private void CreteInsuredRisksFromRisks(IList<Risk> risks, DateTime validFrom, DateTime validTill)
        {
            List<InsuredRisk> insuredRisks = new List<InsuredRisk>();
            foreach (Risk risk in risks)
            {
                insuredRisks.Add(new InsuredRisk(risk, validFrom, this.ValidTill));
            }
            this.InsuredRisks = insuredRisks;
        }

        public IPolicy GetIPolicyService()
        {             
            return new IPolicyImpl(this);
        }

        public string NameOfInsuredObject { get; set; }

        public DateTime ValidFrom { get; set; }

        public DateTime ValidTill { get; set; }

        public decimal Premium { get; set; }

        public List<InsuredRisk> InsuredRisks { get; set; }
    }
}
