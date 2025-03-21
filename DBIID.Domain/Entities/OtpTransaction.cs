using DBIID.Domain.Commen;
using DBIID.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Domain.Entities
{
    public class OtpTransaction : BaseEntity
    {
        public int UserId { get; set; }
        public Guid TransactionId { get; set; }
        public string OtpCode { get; set; } = string.Empty;
        public DateTime Expire { get; set; }
        public bool IsUsed { get; set; }
        public OtpTransactionType Type { get; set; } = OtpTransactionType.NotSet;

        public User? User { get; set; }
    }
}
