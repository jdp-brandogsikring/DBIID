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
    [HttpRequest(HttpMethodType.GET, "Companies/{Id}")]
    public class GetCompanyByIdQuery : IRequest<Result<CompanyDto>>
    {
        public int Id { get; set; }
    }
}
