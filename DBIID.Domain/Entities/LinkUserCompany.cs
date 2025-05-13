using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Domain.Entities
{
    public class LinkUserCompany
    {
        public int UserId { get; set; }
        public int CompanyId { get; set; }

        public User? User { get; set; }
        public Company? Company { get; set; }
    }
}
