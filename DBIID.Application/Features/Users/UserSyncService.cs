using DBIID.Application.Features.Applications;
using DBIID.Application.Features.Companies;
using DBIID.Domain.Entities;
using DBIID.Shared.Features.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Application.Features.Users
{
    public class UserSyncService : IUserSyncService
    {
        private readonly IApplicationRepository applicationRepository;
        private readonly ILinkUserCompanyRepository linkUserCompanyRepository;

        public UserSyncService(IApplicationRepository applicationRepository, ILinkUserCompanyRepository linkUserCompanyRepository)
        {
            this.applicationRepository = applicationRepository;
            this.linkUserCompanyRepository = linkUserCompanyRepository;
        }

        public async Task CompanyAddedToApplication(Company company, Domain.Entities.Application application)
        {
            if(application.EnablePush && !string.IsNullOrWhiteSpace(application.PushUrl))
            {
                var users = linkUserCompanyRepository.GetAllIncludeCompanies().Where(x => x.CompanyId == company.Id).Select(x => x.User).ToList();

                foreach (var user in users)
                {
                    await TrySend(application, user, UserSyncActionType.Create);
                }
            }
        }

        public async Task CompanyRemovedFromApplication(Company company, Domain.Entities.Application application)
        {
            if (application.EnablePush && !string.IsNullOrWhiteSpace(application.PushUrl))
            {
                var users = linkUserCompanyRepository.GetAllIncludeCompanies().Where(x => x.CompanyId == company.Id).Select(x => x.User).ToList();

                foreach (var user in users)
                {
                    await TrySend(application, user, UserSyncActionType.Delete);
                }
            }
        }

        public async Task PushUserChangesToClients(User user, UserSyncActionType actionType)
        {
            // Virksomheder som brugeren er tilknyttet
            List<int> companyIds = linkUserCompanyRepository.GetAllIncludeCompanies().Where(x => x.UserId == user.Id).Select(x => x.CompanyId).ToList();

            // Applikationer som brugeren har adgang til
            List<Domain.Entities.Application> applications = applicationRepository.GetAllIncludeCompanies().Where(x => x.Links.Any(t => companyIds.Contains(t.CompanyId))).ToList();

            foreach (var app in applications.Where(x => !string.IsNullOrWhiteSpace(x.PushUrl) && x.EnablePush))
            {
                await TrySend(app, user, actionType);
            }
        }

        public async Task UserAddedToCompany(User user, Company company)
        {
            List<Domain.Entities.Application> applications = applicationRepository.GetAllIncludeCompanies().Where(x => x.Links.Any(t => t.CompanyId == company.Id)).ToList();

            foreach (var app in applications.Where(x => !string.IsNullOrWhiteSpace(x.PushUrl) && x.EnablePush))
            {
                await TrySend(app, user, UserSyncActionType.Create);
            }
        }

        public async Task UserRemovedCompany(User user, Company company)
        {
            List<Domain.Entities.Application> applications = applicationRepository.GetAllIncludeCompanies().Where(x => x.Links.Any(t => t.CompanyId == company.Id)).ToList();

            foreach (var app in applications.Where(x => !string.IsNullOrWhiteSpace(x.PushUrl) && x.EnablePush))
            {
                await TrySend(app, user, UserSyncActionType.Delete);
            }
        }

        private async Task TrySend(Domain.Entities.Application app, User user, UserSyncActionType actionType)
        {
            try
            {

                HttpClient httpClient = new HttpClient();
                var content = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    Id = user.Id,
                    GivenName = user.GivenName,
                    Familyname = user.FamilyName,
                    Email = user.Email,
                    Phone = user.Phone,
                });

                var request = actionType switch
                {
                    UserSyncActionType.Create => new HttpRequestMessage(HttpMethod.Post, $"{app.PushUrl}"),
                    UserSyncActionType.Update => new HttpRequestMessage(HttpMethod.Put, $"{app.PushUrl}"),
                    UserSyncActionType.Delete => new HttpRequestMessage(HttpMethod.Delete, $"{app.PushUrl}/{user.Id}"),
                    _ => throw new ArgumentOutOfRangeException(nameof(actionType), actionType, null)
                };

                if (actionType != UserSyncActionType.Delete)
                {
                    request.Content = new StringContent(content, Encoding.UTF8, "application/json"); // Fixed: Use StringContent instead of HttpContent
                }

                request.Headers.Add("X-Api-Key", app.Token);

                var response = await httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    // Log or handle the error as needed
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Failed to sync user changes to client: {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                Console.WriteLine($"Exception occurred while syncing user changes to client: {ex.Message}");
            }
        }
    }
}
