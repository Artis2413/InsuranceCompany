using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using InsuranceCompany;

namespace UnitTests.Utils
{
    [TestFixture]
    class UtilsTests
    {
        [Test]
        public void IsInPast_WithPastDate_ReturnsTrue()
        {
            var input = DateTime.Now.AddMinutes(-1);

            var output = InsuranceCompany.Utils.IsInPast(input);

            Assert.IsTrue(output);
        }

        [Test]
        public void IsInPast_WithFutureDate_ReturnsFalse()
        {
            var input = DateTime.Now.AddMinutes(1);

            var output = InsuranceCompany.Utils.IsInPast(input);

            Assert.IsFalse(output);
        }
        [Test]
        public void IsBetween_BetweenDates_ReturnsTrue()
        {
            var output = 
                InsuranceCompany.Utils.IsBetween(
                    DateTime.Now, 
                    DateTime.Now.AddDays(-1), 
                    DateTime.Now.AddDays(1));

            Assert.IsTrue(output);
        }
        [Test]
        public void IsBetween_NotBetweenDates_ReturnsFalse()
        {
            var output =
                    InsuranceCompany.Utils.IsBetween(
                        DateTime.Now,
                        DateTime.Now.AddDays(1),
                        DateTime.Now.AddDays(2));

            Assert.IsFalse(output);
        }

        [Test]
        public void CalculatePremiumFromRisksAndMonthsTest()
        {
            List<Risk> risks = new List<Risk>()
            {
                new Risk("A", 12M),
                new Risk("B", 24M)
            };
            decimal output = InsuranceCompany.Utils.CalculatePremiumFromRisksAndMonths(risks, 1);

            Assert.AreEqual(output, 3M);
        }
    }
}
