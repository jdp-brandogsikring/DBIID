using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Application.Features.Auth
{
    public class OtpService : IOtpService
    {
        private readonly IOtpTransactionRepository otpTransactionRepository;

        public OtpService(IOtpTransactionRepository otpTransactionRepository)
        {
            this.otpTransactionRepository = otpTransactionRepository;
        }

        public string GenerateOtp()
        {
            string otp = new Random().Next(100000, 999999).ToString();

            var otpInDB = otpTransactionRepository.GetAll().Where(x => x.OtpCode == otp && x.IsUsed == false).FirstOrDefault();
            while (otpInDB != null)
            {
                otp = new Random().Next(100000, 999999).ToString();
                otpInDB = otpTransactionRepository.GetAll().Where(x => x.OtpCode == otp && x.IsUsed == false).FirstOrDefault();
            }

            return otp;
        }
    }
}
