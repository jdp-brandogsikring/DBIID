using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Shared.Features.Login
{
    public class VerifyOtpResponse
    {
        public string Message { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; } 
    }
}
