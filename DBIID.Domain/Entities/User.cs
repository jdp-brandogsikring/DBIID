using DBIID.Domain.Commen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Domain.Entities
{
    public class User : BaseEntity
    {
        public string GivenName { get; set; } = string.Empty;
        public string FamilyName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public string Password { get; set; } = string.Empty;

        public List<LinkUserCompany> Links { get; set; } = new();
    }
}
