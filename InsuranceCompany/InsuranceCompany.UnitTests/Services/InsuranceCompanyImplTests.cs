using InsuranceCompany;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Tests
{
    [TestFixture]
    class InsuranceCompanyImplTests
    {
        protected InsuranceCompany.InsuranceCompany CreateInstance()
        {
            return new InsuranceCompany.InsuranceCompanyImpl();
        }

        private InsuranceCompany.InsuranceCompany service;
        [SetUp]
        public void Setup()
        {
            service = CreateInstance();

            service.AviaableRisks = new System.Collections.Generic.List<Risk>()
            {
                new Risk("A", 1.0M),
                new Risk("B", 2.0M),
                new Risk("C", 3.0M)
            };
        }

        [Test]
        public void Name_ValidName_ReturnsName()
        {        
            Assert.IsNotEmpty(service.Name);
        }

        [Test]
        public void GetAviableRisks_ReturnsRisks()
        {
            System.Collections.Generic.IList<Risk> risks = service.AviaableRisks;

            Assert.IsNotEmpty(risks);
        }

        [Test]
        public void SetAviableRisks_InvalidRisks_ThrowsException()
        {
            try
            {
                service.AviaableRisks = new System.Collections.Generic.List<Risk>()
                {
                    new Risk("A", 1.0M),
                    new Risk("B", 2.0M),
                    new Risk("A", 3.0M)
                };
            } catch (InsuranceCompany.Exceptions.ValidationException)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void SetAviableRisks_ValidRisks()
        {
            System.Collections.Generic.IList<Risk> risks = new System.Collections.Generic.List<Risk>()
            {
                new Risk("D", 4.0M),
                new Risk("E", 5.0M),
                new Risk("F", 6.0M)
            };

            service.AviaableRisks = risks;

            Assert.AreEqual(risks, service.AviaableRisks);
        }
        [Test]
        public void SellPolicy_Valid_ReturnsIPolicy()
        {
            IPolicy output = service.SellPolicy("A", System.DateTime.Now, 2, service.AviaableRisks);

            Assert.IsNotNull(output);
        }

        [Test]
        public void SellPolicy_InValid_ThrowsException()
        {
            try
            {
                service.SellPolicy("A", System.DateTime.Now, 2, service.AviaableRisks);
                IPolicy output = service.SellPolicy("A", System.DateTime.Now, 2, service.AviaableRisks);
            } catch(InsuranceCompany.Exceptions.ValidationException)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void AddRisk_ValidRisk_AddsRisk()
        {
            List<Risk> list = new List<Risk>(service.AviaableRisks);
            List<Risk> risksToInsure = list.GetRange(1, list.Count - 1);
            Risk riskToAdd = list[0];

            DateTime effectiveDate = DateTime.Now;
            // make policy
            service.SellPolicy("Policy", effectiveDate, 2, risksToInsure);
            // add risk to policy
            service.AddRisk("Policy", riskToAdd, effectiveDate.AddSeconds(1) ,effectiveDate);
            // pause thread sorisk becomes active
            System.Threading.Thread.Sleep(2000);
            // check if risk was added
            bool output = service.GetPolicy("Policy", effectiveDate).InsuredRisks.Contains(riskToAdd);

            Assert.IsTrue(output);
        }

        [Test]
        public void AddRisk_InValidRisk_ThrowsExceptionk()
        {
            List<Risk> list = new List<Risk>(service.AviaableRisks);
            List<Risk> risksToInsure = list.GetRange(1, list.Count - 1);
            Risk riskToAdd = list[0];

            DateTime effectiveDate = DateTime.Now;
            // make policy
            service.SellPolicy("Policy1", effectiveDate, 2, service.AviaableRisks);
            try
            {
                service.AddRisk("Policy1", riskToAdd, effectiveDate.AddDays(-1), effectiveDate);
            } catch(InsuranceCompany.Exceptions.ValidationException)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void RemoveRisk_ValidRisk_AddsDateTillActiveToRisk()
        {
            DateTime effectiveDate = DateTime.Now;
            DateTime validTill = DateTime.Now.AddSeconds(1);

            // make policy
            service.SellPolicy("Policy2", effectiveDate, 2, service.AviaableRisks);
            // remove risk
            service.RemoveRisk("Policy2", service.AviaableRisks[0], validTill, effectiveDate);
            // sleep so policy could become inactive
            System.Threading.Thread.Sleep(2000);
            // check if risk is in active policies risks
            bool output = service.GetPolicy("Policy2", effectiveDate).InsuredRisks
                                .Contains(service.AviaableRisks[0]);

            Assert.IsFalse(output);
        }

        [Test]
        public void RemoveRisk_InValidRisk_ThrowsValidationException()
        {
            DateTime effectiveDate = DateTime.Now;

            // make policy
            service.SellPolicy("Policy3", effectiveDate, 2, service.AviaableRisks);
            try
            {
                service.RemoveRisk("Policy3", service.AviaableRisks[0], DateTime.Now.AddDays(-1), effectiveDate);
            } catch(InsuranceCompany.Exceptions.ValidationException)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void RemoveRisk_InValidPolicy_ThrowsNotFoundException()
        {
            try
            {
                service.RemoveRisk("Policy3", service.AviaableRisks[0], DateTime.Now.AddDays(+1), DateTime.Now);
            }
            catch (InsuranceCompany.Exceptions.NotFoundException)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void RemoveRisk_InValidRisk_ThrowsNotFoundException()
        {
            try
            {
                service.RemoveRisk("Policy3", new Risk("not in list", 25M), DateTime.Now.AddDays(+1), DateTime.Now);
            }
            catch (InsuranceCompany.Exceptions.NotFoundException)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void GetPolicy_ExsistingPolicy_ReturnsPolicy()
        {
            DateTime effectiveDate = DateTime.Now;
            service.SellPolicy("Policy4", effectiveDate, 2, service.AviaableRisks);

            IPolicy policy = service.GetPolicy("Policy4", effectiveDate);

            Assert.IsNotNull(policy);
        }

        [Test]
        public void GetPolicy_NotExsistigPolicy_ThrowsNotFoundException()
        {
            try
            {
                IPolicy policy = service.GetPolicy("Not exsisting", new DateTime());
            } catch(InsuranceCompany.Exceptions.NotFoundException)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }
    }
}