using DBIID.Shared.Features.Companies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Shared.Features.Users
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string GivenName { get; set; } = string.Empty;
        public string FamilyName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

    }

    public class UserWithAssignedCompaniesDto : UserDto
    {
        public List<CompanyDto> Companies { get; set; }
    }
}
