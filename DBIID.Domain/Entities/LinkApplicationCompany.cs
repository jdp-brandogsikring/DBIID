using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Domain.Entities
{
    public class LinkApplicationCompany
    {
        public int ApplicationId { get; set; }
        public int CompanyId { get; set; }

        public Application? Application { get; set; }
        public Company? Company { get; set; }
    }
}
