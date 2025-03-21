using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Shared.Dtos
{
    public class LoginResult
    {
        public int UserId { get; set; }
        public Guid OtpTransactionId { get; set; }
        public List<OptTransactionTypeDto> Types { get; set; } = new List<OptTransactionTypeDto>();
    }
}
