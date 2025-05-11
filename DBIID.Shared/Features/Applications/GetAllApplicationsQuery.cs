using DBIID.Application.Shared.Attributes;
using DBIID.Shared.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Shared.Features.Applications
{
    [HttpRequest(HttpMethodType.GET, "Applications/All")]
    public class GetAllApplicationsQuery : IRequest<Result<List<ApplicationDto>>>
    {
    }
}
