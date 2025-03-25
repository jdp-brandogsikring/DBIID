using DBIID.Application.Features.Auth;

namespace DBIID.API.Service
{
    public class SendOTPService : ISendOTPService
    {
        public async Task SendOtpEmail(string Otp, string Email)
        {
            Console.WriteLine("-------------------------------------");
            Console.WriteLine($"Sending OTP to {Email} - {Otp}");
            Console.WriteLine("-------------------------------------");

        }

        public async Task SendOtpPhone(string Otp, string PhoneNumber)
        {
            Console.WriteLine("-------------------------------------");
            Console.WriteLine($"Sending OTP to {PhoneNumber} - {Otp}");
            Console.WriteLine("-------------------------------------");
        }
    }
}
