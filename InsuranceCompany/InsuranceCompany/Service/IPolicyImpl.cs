using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceCompany
{
    public class IPolicyImpl: IPolicy
    {
        private Model.Policy policy;

        public IPolicyImpl(Model.Policy policy)
        {
            this.policy = policy;
        }

        public string NameOfInsuredObject
        {
            get { return policy.NameOfInsuredObject; }
            set { policy.NameOfInsuredObject = value; }
        }
        public DateTime ValidFrom
        {   get { return policy.ValidFrom; }
            set
            {
                if(Utils.IsInPast(value) || policy.ValidTill < value)
                {
                    throw new Exceptions.ValidationException("Valid from cannot be in past or after valid till");
                }
                policy.ValidFrom = value;
            }
        }
        public DateTime ValidTill
        {
            get { return policy.ValidTill; }
            set
            {
                if(Utils.IsInPast(value) || policy.ValidFrom > value)
                {
                    throw new Exceptions.ValidationException("Value cannot be in past or before valid from");
                }
                policy.ValidTill = value;
            }
        }

        public decimal Premium
        {
            get { return policy.Premium; }
            set { policy.Premium = value; }
        }

        public IList<Risk> InsuredRisks
        {
            get
            {
                List<Model.InsuredRisk> insuredRisksFilterd = policy.InsuredRisks.FindAll(p => Utils.IsBetween(DateTime.Now, p.ValidFrom, p.ValidTill));

                List<Risk> risks = new List<Risk>();
                insuredRisksFilterd.ForEach(p => risks.Add(p.Risk));
                return risks;
            }
            set
            {
                if(!Utils.IsNullOrEmpty(value) && Validators.IsValidRisks(value))
                {
                    // make exsisting Risks inactive
                    policy.InsuredRisks.ForEach(p => p.ValidTill = DateTime.Now);

                    // set new risks from now till policy end
                    foreach(Risk risk in value)
                    {
                        if (policy.InsuredRisks.Exists(p => p.Risk.Equals(risk)))
                        {
                            policy.InsuredRisks.Find(p => p.Risk.Equals(risk)).ValidTill = policy.ValidTill;
                        } else
                        {
                            policy.InsuredRisks.Add(new Model.InsuredRisk(risk, DateTime.Now, policy.ValidTill));
                        }
                    }

                } else
                {
                    throw new Exceptions.ValidationException("Invalid risks");
                }
            }
        }
    }
}
