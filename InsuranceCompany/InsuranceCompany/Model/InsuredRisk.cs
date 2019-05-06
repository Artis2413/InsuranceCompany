using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceCompany.Model
{
    public class InsuredRisk
    {
        public InsuredRisk(Risk risk, DateTime validFrom, DateTime validTill)
        {
            this.Risk = risk;
            this.ValidFrom = validFrom;
            this.ValidTill = validTill;
        }

        public Risk Risk { get; set; }
        public DateTime ValidFrom{ get; set; }
        public DateTime ValidTill { get; set; }
    }
}
