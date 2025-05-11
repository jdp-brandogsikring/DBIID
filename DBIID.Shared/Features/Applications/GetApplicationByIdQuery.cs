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
    [HttpRequest(HttpMethodType.GET, "Applications/{Id}")]
    public class GetApplicationByIdQuery : IRequest<Result<ApplicationDto>>
    {
        public int Id { get; set; }
    }
}
