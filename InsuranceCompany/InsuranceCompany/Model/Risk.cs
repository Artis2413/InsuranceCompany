using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceCompany
{
    public struct Risk
    {
        public Risk(string name, decimal yearlyPrice) : this()
        {
            this.Name = name;
            this.YearlyPrice = yearlyPrice;
        }

        public string Name { get; set; }
        public decimal YearlyPrice { get; set; }
    }
}
