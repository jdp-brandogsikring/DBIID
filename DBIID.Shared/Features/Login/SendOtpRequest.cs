using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Shared.Features.Login
{
    public class SendOtpRequest
    {
        public int UserId { get; set; } 
        public Guid OtpTransactionId { get; set; } 
        public string ContactMethodId { get; set; } = string.Empty;
    }
}
