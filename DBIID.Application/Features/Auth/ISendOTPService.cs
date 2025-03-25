using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Application.Features.Auth
{
    public interface ISendOTPService
    {
        Task SendOtpEmail(string Otp, string Email);
        Task SendOtpPhone(string Otp, string PhoneNumber);
    }
}
