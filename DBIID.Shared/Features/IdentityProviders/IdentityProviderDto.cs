using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Shared.Features.IdentityProviders
{
    public class IdentityProviderDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string TenantId { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
    }
}
