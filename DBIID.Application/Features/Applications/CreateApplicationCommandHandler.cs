using DBIID.Application.Common.Data;
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
    public class CreateApplicationCommandHandler : ICommandHandler<CreateApplicationCommand, Result<ApplicationDto>>
    {
        private readonly IApplicationRepository applicationRepository;
        private readonly IUnitOfWork unitOfWork;

        public CreateApplicationCommandHandler(IApplicationRepository applicationRepository,
                                               IUnitOfWork unitOfWork)
        {
            this.applicationRepository = applicationRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<ApplicationDto>> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
        {
            var application = new Domain.Entities.Application
            {
                Name = request.Name,
            };
            await applicationRepository.AddAsync(application);
            await unitOfWork.SaveChangesAsync();
            return Result<ApplicationDto>.Success(new ApplicationDto
            {
                Id = application.Id,
                Name = application.Name,
            });
        }
    }
}
