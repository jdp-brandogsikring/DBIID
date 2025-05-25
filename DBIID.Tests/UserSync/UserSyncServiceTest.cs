
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DBIID.Application.Features.Applications;
using DBIID.Application.Features.Users;
using DBIID.Domain.Entities;
using FluentAssertions;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Xunit;

namespace DBIID.Tests.UserSync
{
    public class UserSyncServiceTest
    {
        [Fact]
        public async Task CompanyAddedToApplication_ShouldSendPost_WhenPushEnabled()
        {
            // Arrange
            var mockApplicationRepo = new Mock<IApplicationRepository>();
            var mockLinkUserCompanyRepo = new Mock<ILinkUserCompanyRepository>();

            var company = new Company { Id = 1, Name = "TestCompany" };
            var application = new DBIID.Domain.Entities.Application
            {
                Id = 1,
                Name = "TestApp",
                PushUrl = "http://localhost/api/users",
                EnablePush = true,
                Token = "secret-key"
            };
            var user = new User
            {
                Id = 42,
                GivenName = "Jonas",
                FamilyName = "Pedersen",
                Email = "jonas@example.com",
                Phone = "12345678"
            };

            mockLinkUserCompanyRepo.Setup(x => x.GetAllIncludeCompanies())
                .Returns(new List<LinkUserCompany>
                {
                    new LinkUserCompany { CompanyId = company.Id, User = user }
                }.AsQueryable());

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK))
                .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object);

            var service = new UserSyncService(
                mockApplicationRepo.Object,
                mockLinkUserCompanyRepo.Object,
                httpClient
            );

            // Act
            await service.CompanyAddedToApplication(company, application);

            // Assert
            handlerMock.Protected().Verify("SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri.ToString() == application.PushUrl &&
                    req.Headers.Contains("X-Api-Key")
                ),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task CompanyRemovedFromApplication_ShouldSendDelete_WhenPushEnabled()
        {
            // Arrange
            var mockApplicationRepo = new Mock<IApplicationRepository>();
            var mockLinkUserCompanyRepo = new Mock<ILinkUserCompanyRepository>();

            var company = new Company { Id = 1 };
            var application = new DBIID.Domain.Entities.Application
            {
                Id = 1,
                Name = "TestApp",
                PushUrl = "http://localhost/api/users",
                EnablePush = true,
                Token = "secret-key"
            };
            var user = new User { Id = 100, GivenName = "Anna", FamilyName = "Andersen" };

            mockLinkUserCompanyRepo.Setup(x => x.GetAllIncludeCompanies())
                .Returns(new List<LinkUserCompany>
                {
                    new LinkUserCompany { CompanyId = company.Id, User = user }
                }.AsQueryable());

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Delete),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK))
                .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object);

            var service = new UserSyncService(
                mockApplicationRepo.Object,
                mockLinkUserCompanyRepo.Object, httpClient
            );

            // Act
            await service.CompanyRemovedFromApplication(company, application);

            // Assert
            handlerMock.Protected().Verify("SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Delete &&
                    req.RequestUri.ToString() == $"{application.PushUrl}/{user.Id}"
                ),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task TrySend_ShouldCatchHttpExceptions()
        {
            // Arrange
            var mockApplicationRepo = new Mock<IApplicationRepository>();
            var mockLinkUserCompanyRepo = new Mock<ILinkUserCompanyRepository>();

            var user = new User { Id = 1, GivenName = "Eva", FamilyName = "Nielsen", Email = "eva@example.com", Phone = "12121212" };
            var application = new DBIID.Domain.Entities.Application
            {
                PushUrl = "http://localhost/api/users",
                EnablePush = true,
                Token = "secret-key"
            };
            // Replace internal HTTP call with one that throws
            using var httpClient = new HttpClient(new FailingHandler());

            var service = new UserSyncService(mockApplicationRepo.Object, mockLinkUserCompanyRepo.Object, httpClient);


            // Act & Assert
            var exception = await Record.ExceptionAsync(() =>
                service.CompanyAddedToApplication(new Company { Id = 1 }, application)
            );

            exception.Should().BeNull(); // Exception should be caught internally
        }

        private class FailingHandler : HttpMessageHandler
        {
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                throw new HttpRequestException("Simulated network failure");
            }
        }

        [Fact]
        public async Task UserAddedToCompany_ShouldSendPostToLinkedApplications()
        {
            // Arrange
            var company = new Company { Id = 1 };
            var user = new User { Id = 7, GivenName = "Klara", FamilyName = "Knudsen", Email = "klara@example.com", Phone = "90909090" };

            var applications = new List<DBIID.Domain.Entities.Application>
            {
                new DBIID.Domain.Entities.Application
                {
                    Id = 1,
                    Name = "CRM",
                    EnablePush = true,
                    PushUrl = "http://localhost/api/users",
                    Token = "abc123",
                    Links = new List<LinkApplicationCompany>
                    {
                        new LinkApplicationCompany { CompanyId = company.Id }
                    }
                }
            };

            var mockApplicationRepo = new Mock<IApplicationRepository>();
            mockApplicationRepo.Setup(x => x.GetAllIncludeCompanies()).Returns(applications.AsQueryable());

            var mockLinkUserCompanyRepo = new Mock<ILinkUserCompanyRepository>();

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Delete),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK))
                .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object);

            var service = new UserSyncService(mockApplicationRepo.Object, mockLinkUserCompanyRepo.Object, httpClient);

            // Act
            var exception = await Record.ExceptionAsync(() =>
                service.UserAddedToCompany(user, company)
            );

            // Assert
            exception.Should().BeNull();
        }

        [Fact]
        public async Task PushUserChangesToClients_ShouldSendToAllLinkedApplications()
        {
            // Arrange
            var user = new User { Id = 8, GivenName = "Bent", FamilyName = "Bach", Email = "bent@example.com", Phone = "45454545" };
            var companies = new List<LinkUserCompany>
            {
                new LinkUserCompany { UserId = user.Id, CompanyId = 1 },
                new LinkUserCompany { UserId = user.Id, CompanyId = 2 }
            };

            var applications = new List<DBIID.Domain.Entities.Application>
            {
                new DBIID.Domain.Entities.Application
                {
                    Id = 1,
                    Name = "App1",
                    EnablePush = true,
                    PushUrl = "http://localhost/api/users",
                    Token = "abc123",
                    Links = new List<LinkApplicationCompany>
                    {
                        new LinkApplicationCompany { CompanyId = 1 },
                        new LinkApplicationCompany { CompanyId = 2 }
                    }
                }
            };

            var mockApplicationRepo = new Mock<IApplicationRepository>();
            mockApplicationRepo.Setup(x => x.GetAllIncludeCompanies()).Returns(applications.AsQueryable());

            var mockLinkUserCompanyRepo = new Mock<ILinkUserCompanyRepository>();
            mockLinkUserCompanyRepo.Setup(x => x.GetAllIncludeCompanies()).Returns(companies.AsQueryable());

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Delete),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK))
                .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object);

            var service = new UserSyncService(mockApplicationRepo.Object, mockLinkUserCompanyRepo.Object, httpClient);

            // Act
            var exception = await Record.ExceptionAsync(() =>
                service.PushUserChangesToClients(user, UserSyncActionType.Create)
            );

            // Assert
            exception.Should().BeNull();
        }
    }
}
