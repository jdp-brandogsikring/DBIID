using DBIID.Application.Common.Handlers;
using DBIID.Shared.Features.Applications;
using DBIID.Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Application.Features.Applications
{
    public class GetAllApplicationsQueryHandler : IQueryHandler<GetAllApplicationsQuery, Result<List<ApplicationDto>>>
    {
        private readonly IApplicationRepository applicationRepository;

        public GetAllApplicationsQueryHandler(IApplicationRepository applicationRepository)
        {
            this.applicationRepository = applicationRepository;
        }
        public async Task<Result<List<ApplicationDto>>> Handle(GetAllApplicationsQuery request, CancellationToken cancellationToken)
        {
            var applications = applicationRepository.GetAllAsNonTracking();
            var applicationDtos = applications.Select(c => new ApplicationDto
            {
                Id = c.Id,
                Name = c.Name,
                Token = c.Token,
                Url = c.Url,
            }).ToList();
            return Result<List<ApplicationDto>>.Success(applicationDtos);
        }
    }
}
