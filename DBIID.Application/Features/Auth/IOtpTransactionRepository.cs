﻿using DBIID.Application.Common.Data;
using DBIID.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Application.Features.Auth
{
    public interface IOtpTransactionRepository : IGenericRepository<OtpTransaction>
    {
    }
}
