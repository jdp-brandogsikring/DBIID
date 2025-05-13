using DBIID.Shared.Features.Applications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Shared.Features.Companies
{
    public class CompanyDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class CompanyWithApplicationsDto : CompanyDto
    {
        public List<ApplicationDto> Applications { get; set; } = new List<ApplicationDto>();
    }
}
