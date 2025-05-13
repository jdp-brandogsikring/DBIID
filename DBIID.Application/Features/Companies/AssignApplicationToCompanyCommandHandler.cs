using DBIID.Application.Common.Data;
using DBIID.Application.Common.Handlers;
using DBIID.Application.Features.Applications;
using DBIID.Shared.Features.Companies;
using DBIID.Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Application.Features.Companies
{
    public class AssignApplicationToCompanyCommandHandler : ICommandHandler<AssignApplicationToCompanyCommand, Result>
    {
        private readonly IApplicationRepository applicationRepository;
        private readonly ICompanyRepository companyRepository;
        private readonly ILinkApplicationCompanyRepository linkApplicationCompanyRepository;
        private readonly IUnitOfWork unitOfWork;

        public AssignApplicationToCompanyCommandHandler(IApplicationRepository applicationRepository,
                                                        ICompanyRepository companyRepository,
                                                        ILinkApplicationCompanyRepository linkApplicationCompanyRepository,
                                                        IUnitOfWork unitOfWork)
        {
            this.applicationRepository = applicationRepository;
            this.companyRepository = companyRepository;
            this.linkApplicationCompanyRepository = linkApplicationCompanyRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(AssignApplicationToCompanyCommand request, CancellationToken cancellationToken)
        {
            var application = applicationRepository.GetById(request.ApplicationId);
            if (application == null)
            {
                return Result.Error("Application not found");
            }

            var company = companyRepository.GetById(request.CompanyId);
            if (company == null)
            {
                return Result.Error("Company not found");
            }

            var link = linkApplicationCompanyRepository.GetAllAsNonTracking()
                .FirstOrDefault(x => x.ApplicationId == request.ApplicationId && x.CompanyId == request.CompanyId);

            if (link == null)
            {
                link = new Domain.Entities.LinkApplicationCompany()
                {
                    CompanyId = request.CompanyId,
                    ApplicationId = request.ApplicationId
                };
                await linkApplicationCompanyRepository.AddAsync(link);
                await unitOfWork.SaveChangesAsync();
            }

            return Result.Success("Application assigned to company successfully");
        }
    }
}
