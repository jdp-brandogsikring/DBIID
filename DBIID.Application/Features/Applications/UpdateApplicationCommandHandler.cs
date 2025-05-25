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
    public class UpdateApplicationCommandHandler : ICommandHandler<UpdateApplicationCommand, Result<ApplicationDto>>
    {
        private readonly IApplicationRepository applicationRepository;
        private readonly IUnitOfWork unitOfWork;

        public UpdateApplicationCommandHandler(IApplicationRepository applicationRepository,
            IUnitOfWork unitOfWork)
        {
            this.applicationRepository = applicationRepository;
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<ApplicationDto>> Handle(UpdateApplicationCommand request, CancellationToken cancellationToken)
        {
            var application = applicationRepository.GetById(request.Id);
            if (application == null)
            {
                return Result<ApplicationDto>.Error("Application not found");
            }
            application.Name = request.Name;
            application.Url = request.Url;
            application.EnablePush = request.EnablePush;
            application.PushUrl = request.PushUrl;

            applicationRepository.Update(application);
            await unitOfWork.SaveChangesAsync();
            var applicationDto = new ApplicationDto
            {
                Id = application.Id,
                Name = application.Name,
                Token = application.Token,
                Url = application.Url,
                EnablePush = application.EnablePush,
                PushUrl = application.PushUrl,

            };
            return Result<ApplicationDto>.Success(applicationDto);
        }
    }

}
