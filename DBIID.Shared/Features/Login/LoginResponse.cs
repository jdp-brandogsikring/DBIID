using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Shared.Features.Login
{
    public class LoginResponse
    {
        public int UserId { get; set; }
        public Guid OtpTransactionId { get; set; }
        public List<ContactMethodDto> Types { get; set; } = new List<ContactMethodDto>();
    }
}
