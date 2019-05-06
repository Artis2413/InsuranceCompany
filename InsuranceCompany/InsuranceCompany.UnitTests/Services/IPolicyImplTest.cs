using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceCompany.UnitTests.Services
{
    [TestFixture]
    class IPolicyImplTest
    {
        private IPolicy service;

        protected IPolicy CreateInstance()
        {
            string nameOfInsuredObject = "policy";
            DateTime validFrom = DateTime.Now;
            List<Risk> risks = new List<Risk>()
            {
                new Risk("1", 1.00M),
                new Risk("2", 2.00M),
                new Risk("3", 3.00M)
            };
            Model.Policy policy = new Model.Policy(nameOfInsuredObject, validFrom, 2, risks);
            return new IPolicyImpl(policy);
        }

        [SetUp]
        public void Setup()
        {
            service = CreateInstance();
        }

        [Test]
        public void NameOfInsuredObject_Get_ReturnsString()
        {
            String name = service.NameOfInsuredObject;

            Assert.IsNotEmpty(name);
        }

        [Test]
        public void NameOfInsuredObject_Set_SetsNewName()
        {
            service.NameOfInsuredObject = "test";

            String name = service.NameOfInsuredObject;

            Assert.AreEqual("test", name);
            // cleanup for other tsts
            service.NameOfInsuredObject = "policy";
        }

        [Test]
        public void ValidFromGet_IsNotNull()
        {
            Assert.IsNotNull(service.ValidFrom);
        }

        [Test]
        public void ValidFromSet_DateTimeInFuture_SetsDateTime()
        {
            DateTime validFrom = DateTime.Now.AddDays(1);
            service.ValidFrom = validFrom;
            Assert.AreEqual(validFrom, service.ValidFrom);
        }

        [Test]
        public void ValidFromSet_DateTimeInPast_ThrowsException()
        {
            DateTime validFrom = DateTime.Now.AddDays(-1);
            try
            {
                service.ValidFrom = validFrom;
            } catch(Exceptions.ValidationException)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void ValidTillSet_DateTimeInFuture_SetsDateTime()
        {
            DateTime validTill = DateTime.Now.AddDays(5);
            service.ValidTill = validTill;
            Assert.AreEqual(validTill, service.ValidTill);
        }

        [Test]
        public void ValidTillSet_DateTimeInPast_ThrowsException()
        {
            DateTime validTill = DateTime.Now.AddDays(-1);
            try
            {
                service.ValidTill = validTill;
            }
            catch (Exceptions.ValidationException)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void PremiumGet_IsNotDefault()
        {
            Assert.AreNotEqual(service.Premium, 0M);
        }

        [Test]
        public void PremiumSet_SetsPremium()
        {
            service.Premium = 15M;
            Assert.AreEqual(service.Premium, 15M);
        }

        [Test]
        public void InsuredRisksGet_IsNotNull()
        {
            Assert.IsNotNull(service.InsuredRisks);
        }

        [Test]
        public void InsuredRisksSet_ValidRisks_SetsInsuredRisks()
        {
            List<Risk> risks = new List<Risk>()
            {
                new Risk("risk", 5.00M),
                new Risk("risk1", 6.00M),
                new Risk("risk2", 7.00M)
            };

            service.InsuredRisks = risks;

            Assert.AreEqual(service.InsuredRisks, risks);
        }

        [Test]
        public void InsuredRisksSet_InValidRisks_ThrowsException()
        {
            try
            {
                service.InsuredRisks = new List<Risk>();
            } catch (Exceptions.ValidationException)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }
    }
}
