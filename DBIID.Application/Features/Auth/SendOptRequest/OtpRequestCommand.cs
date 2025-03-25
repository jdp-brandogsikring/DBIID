using DBIID.Shared.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Application.Features.Auth.SendOptRequest
{
    public class OtpRequestCommand : IRequest<Result>
    {
        public int UserId { get; set; }
        public Guid OtpTransactionId { get; set; }
        public string ContactMethodId { get; set; } = string.Empty;
    }
}
