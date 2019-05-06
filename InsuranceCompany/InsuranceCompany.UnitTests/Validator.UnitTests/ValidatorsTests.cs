using NUnit.Framework;
using System;
using System.Collections;
using System.Text;
using InsuranceCompany;
using InsuranceCompany.Model;
using System.Collections.Generic;

namespace Tests
{
    [TestFixture]
    class ValidatorsTests
    {
        [Test]
        public void IsValidRisks_ValidRisks_returnsTrue()
        {
            // variables
            System.Collections.Generic.IList<Risk> risks = new System.Collections.Generic.List<Risk>();
            risks.Add(new Risk("A", 1.00M));
            risks.Add(new Risk("B", 1.00M));
            risks.Add(new Risk("C", 2.00M));
            // act
            bool output = Validators.IsValidRisks(risks);
            // assert
            Assert.AreEqual(output, true);
        }

        [Test]
        public void IsValidRisks_InvalidRisks_returnsFalse()
        {
            // variables
            System.Collections.Generic.IList<Risk> risks = new System.Collections.Generic.List<Risk>();
            risks.Add(new Risk("A", 1.00M));
            risks.Add(new Risk("B", 1.00M));
            risks.Add(new Risk("A", 2.00M));
            // act
            bool output = Validators.IsValidRisks(risks);
            // assert
            Assert.AreEqual(output, false);
        }

        [Test]
        public void CanSellPolicy_ValidPolicySell_ReturnsTrue()
        {
            IList<Policy> soldPolicies = new List<Policy>() { };
            string newInsuredObjectName = "A";
            DateTime validFrom = DateTime.Now;

            var output = Validators.CanSellPolicy(soldPolicies, newInsuredObjectName, validFrom);

            Assert.IsTrue(output);
        }

        [Test]
        public void CanSellPolicy_InValidPolicySell_ReturnsFalse()
        {
            IList<Policy> soldPolicies = new List<Policy>()
            {
                new Policy("A", DateTime.Now, 2, new List<Risk>())
            };
            string newInsuredObjectName = "A";
            DateTime validFrom = DateTime.Now;
            
            var output = Validators.CanSellPolicy(soldPolicies, newInsuredObjectName, validFrom);

            Assert.IsFalse(output);
        }
    }
}
