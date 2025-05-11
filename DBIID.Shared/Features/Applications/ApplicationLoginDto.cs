using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Shared.Features.Applications
{
    public class ApplicationLoginDto
    {
        public int ApplicationId { get; set; }
        public string ApplicationName { get; set; } = string.Empty;
        public string ApplicationUrl { get; set; } = string.Empty;

        public string SSOToken { get; set; } = string.Empty;

        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;

        public DateTime? Expire { get; set; }
    }
}
