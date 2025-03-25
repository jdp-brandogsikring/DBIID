using DBIID.Shared.Features.Users;
using DBIID.Shared.Results;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Application.Features.Auth.VerifyOtpRequest
{
    public class VerifyOtpRequestCommand : IRequest<Result<UserDto>>
    {
        public int UserId { get; set; }
        public Guid OtpTransactionId { get; set; }
        public string OtpCode { get; set; } = string.Empty;
    }
}
