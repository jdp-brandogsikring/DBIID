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
    public class GetApplicationByIdQueryHandler : IQueryHandler<GetApplicationByIdQuery, Result<ApplicationDto>>
    {
        private readonly IApplicationRepository applicationRepository;
        public GetApplicationByIdQueryHandler(IApplicationRepository applicationRepository)
        {
            this.applicationRepository = applicationRepository;
        }
        public async Task<Result<ApplicationDto>> Handle(GetApplicationByIdQuery request, CancellationToken cancellationToken)
        {
            var application = applicationRepository.GetById(request.Id);
            if (application == null)
            {
                return Result<ApplicationDto>.Error("Application not found");
            }
            var applicationDto = new ApplicationDto
            {
                Id = application.Id,
                Name = application.Name,
                Token = application.Token,
                Url = application.Url,
                PushUrl = application.PushUrl,
                EnablePush = application.EnablePush
            };
            return Result<ApplicationDto>.Success(applicationDto);
        }
    }
}
