using DBIID.Application.Shared.Attributes;
using DBIID.Shared.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Shared.Features.Companies
{
    [HttpRequest(HttpMethodType.GET, "Companies/All")]

    public class GetAllCompaniesQuery : IRequest<Result<List<CompanyDto>>>
    {
    }
}
