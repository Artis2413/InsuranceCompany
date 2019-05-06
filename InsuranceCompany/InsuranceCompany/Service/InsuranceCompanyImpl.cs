using InsuranceCompany.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceCompany
{
    public class InsuranceCompanyImpl : InsuranceCompany
    {
        private List<Policy> Policies;

        public InsuranceCompanyImpl()
        {
            this.Policies = new List<Policy>();
        }

        public string Name
        {
            get
            {
                return Constancts.Constants.COMPANY_NAME;
            }
        }
        private List<Risk> aviaableRisks;
        public IList<Risk> AviaableRisks
        {
            get { return this.aviaableRisks; }
            set
            {
                // Filter identical Risk items if given
                IList<Risk> risksUniq = value.Distinct().ToList();
                // check for duplicate risk names
                if(Validators.IsValidRisks(risksUniq))
                {
                    this.aviaableRisks = risksUniq.ToList();
                }
                else
                {
                    throw new Exceptions.ValidationException("Given risks are not valid");
                }
            }
        }

        public IPolicy SellPolicy(string nameOfInsuredObject, DateTime validFrom, short validMonths, IList<Risk> selectedRisks)
        {
            if (!Validators.CanSellPolicy(Policies, nameOfInsuredObject, validFrom) || Utils.IsNullOrEmpty(selectedRisks))
            {
                throw new Exceptions.ValidationException("Policy cannot be sold");
            }
            Policies.Add(new Policy(nameOfInsuredObject, validFrom, validMonths, selectedRisks));
            return Policies.Last().GetIPolicyService();            
        }

        public void AddRisk(string nameOfInsuredObject, Risk risk, DateTime validFrom, DateTime effectiveDate)
        {
            if(!AviaableRisks.Contains(risk))
            {
                throw new Exceptions.ValidationException("Given risk is not in aviable risks");
            }
            if(validFrom < DateTime.Now || validFrom < effectiveDate)
            {
                throw new Exceptions.ValidationException("Risk activation date has to be in future and greater than effective date");
            }
            Policy policy = GetPolicyModel(nameOfInsuredObject, effectiveDate);           
            policy.InsuredRisks.Add(new InsuredRisk(risk, validFrom, policy.ValidTill));
        }

        /// <exception cref="Exceptions.NotFoundException">If policy or removable risk not found</exception>
        /// <exception cref="Exceptions.ValidationException">If valid till less than EfectiveDate or in past</exception>
        public void RemoveRisk(string nameOfInsuredObject, Risk risk, DateTime validTill, DateTime effectiveDate)
        {
            if(validTill < effectiveDate || validTill < DateTime.Now)
            {
                throw new Exceptions.ValidationException("ValidTill date cannot be less than EfectiveDate or in past");
            }else
            {
                Policy policy = GetPolicyModel(nameOfInsuredObject, effectiveDate);
                try
                {
                    InsuredRisk insuredRisk = policy.InsuredRisks.Find(p => p.Risk.Equals(risk));
                    insuredRisk.ValidTill = validTill;
                } catch(ArgumentNullException)
                {
                    throw new Exceptions.NotFoundException("Risk was not found in policy");
                }
            }
        }

        /// <exception cref="Exceptions.NotFoundException">If policy not found</exception>
        public IPolicy GetPolicy(string nameOfInsuredObject, DateTime effectiveDate)
        {
            return GetPolicyModel(nameOfInsuredObject, effectiveDate).GetIPolicyService();
        }

        /// <exception cref="Exceptions.NotFoundException">If policy not found</exception>
        public Policy GetPolicyModel(string nameOfInsuredObject, DateTime effectiveDate)
        {
            try
            {
                Policy policy = Policies.Find
                                    (p => p.NameOfInsuredObject.Equals(nameOfInsuredObject) && 
                                          p.ValidFrom.Equals(effectiveDate));
                if(policy is null)
                {
                    throw new Exceptions.NotFoundException("Policy not found");
                }
                return policy;
            } catch(ArgumentNullException)
            {
                throw new Exceptions.NotFoundException("Policy not found");
            }
        }
    }
}
