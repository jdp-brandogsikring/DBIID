﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Shared.Features.Login
{
    public enum OtpTransactionType
    {
        NotSet = 0,
        EmailVerification = 1,
        SmsVerification = 2,
    }
}
