using DBIID.Application.Common.Data;
using DBIID.Application.Common.Handlers;
using DBIID.Application.Features.Companies;
using DBIID.Shared.Features.Users;
using DBIID.Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Application.Features.Users
{
    public class AssignUserToCompanyCommandHandler : ICommandHandler<AssignUserToCompanyCommand, Result>
    {
        private readonly IUserRepository userRepository;
        private readonly ICompanyRepository companyRepository;
        private readonly ILinkUserCompanyRepository linkUserCompanyRepository;
        private readonly IUnitOfWork unitOfWork;

        public AssignUserToCompanyCommandHandler(IUserRepository userRepository,
                                             ICompanyRepository companyRepository,
                                             ILinkUserCompanyRepository linkUserCompanyRepository,
                                             IUnitOfWork unitOfWork)
        {
            this.userRepository = userRepository;
            this.companyRepository = companyRepository;
            this.linkUserCompanyRepository = linkUserCompanyRepository;
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(AssignUserToCompanyCommand request, CancellationToken cancellationToken)
        {
            var user = userRepository.GetById(request.UserId);
            if (user == null)
            {
                return Result.Error("User not found");
            }

            var company = companyRepository.GetById(request.CompanyId);
            if (company == null)
            {
                return Result.Error("Company not found");
            }

            var link = linkUserCompanyRepository.GetAllAsNonTracking()
                .FirstOrDefault(x => x.UserId == request.UserId && x.CompanyId == request.CompanyId);

            if (link == null)
            {
                link = new Domain.Entities.LinkUserCompany()
                {
                    CompanyId = request.CompanyId,
                    UserId = request.UserId
                };
                await linkUserCompanyRepository.AddAsync(link);
                await unitOfWork.SaveChangesAsync();
            }

            return Result.Success("User assigned to company successfully");
        }
    }
}
