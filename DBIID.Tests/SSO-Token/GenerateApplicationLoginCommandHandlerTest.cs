using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBIID.Application.Features.Applications;
using DBIID.Application.Features.Auth;
using DBIID.Application.Features.Companies;
using DBIID.Application.Features.Users;
using DBIID.Domain.Entities;
using DBIID.Shared.Features.Applications;
using FluentAssertions;
using Moq;
using Xunit;

namespace DBIID.Tests.SSO_Token
{
    public class GenerateApplicationLoginCommandHandlerTest
    {
        [Fact]
        public async void GenerateApplicationLoginCommandHandler_ShouldReturnSuccess_WhenValidRequest()
        {
            // Arrange
            string token = Guid.NewGuid().ToString().Replace("-", "");
            int currentUserId = 1;

            var request = new GenerateApplicationLoginCommand
            {
                Token = token,
            };

            Mock<IApplicationRepository> applicationRepositoryMock = new Mock<IApplicationRepository>();
            applicationRepositoryMock.Setup(x => x.GetAllIncludeCompanies())
                .Returns(new List<Domain.Entities.Application>()
                {
                    new Domain.Entities.Application()
                    {
                        Id = 1,
                        Token = token,
                        Name = "Test Application",
                        Url = "https://testapplication.com",
                        Links = new List<LinkApplicationCompany>()
                        {
                            new LinkApplicationCompany()
                            {
                                CompanyId = 1,
                                ApplicationId = 1,
                            }
                        }
                    }
                }.AsQueryable());

            Mock<IUserRepository> userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetAllIncludeCompanies())
                .Returns(new List<User>()
                {
                    new User()
                    {
                        Id = currentUserId,
                        FamilyName = "Doe",
                        GivenName = "John",

                        Links = new List<LinkUserCompany>()
                        {
                            new LinkUserCompany()
                            {
                                CompanyId = 1,
                                UserId = currentUserId,
                            }
                        }
                    }
                }.AsQueryable());

            Mock<ICompanyRepository> companyRepositoryMock = new Mock<ICompanyRepository>();
            companyRepositoryMock.Setup(x => x.GetById(It.IsAny<int>()))
                .Returns(new Domain.Entities.Company()
                {
                    Id = 1,
                    Name = "Test Company",
                   
                });

            Mock<ICurrentUser> currentUserMock = new Mock<ICurrentUser>();
            currentUserMock.Setup(x => x.UserId)
                .Returns(currentUserId);


            var handler = new GenerateApplicationLoginCommandHandler(applicationRepositoryMock.Object,
                                                                        companyRepositoryMock.Object,
                                                                        userRepositoryMock.Object,
                                                                        currentUserMock.Object);
            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async void GenerateApplicationLoginCommandHandler_ShouldReturnFalse_WhenUserDontHaveAccessToApplikationByCompany()
        {
            // Arrange
            string token = Guid.NewGuid().ToString().Replace("-", "");
            int currentUserId = 1;

            var request = new GenerateApplicationLoginCommand
            {
                Token = token,
            };

            Mock<IApplicationRepository> applicationRepositoryMock = new Mock<IApplicationRepository>();
            applicationRepositoryMock.Setup(x => x.GetAllIncludeCompanies())
                .Returns(new List<Domain.Entities.Application>()
                {
                    new Domain.Entities.Application()
                    {
                        Id = 1,
                        Token = token,
                        Name = "Test Application",
                        Url = "https://testapplication.com",
                        Links = new List<LinkApplicationCompany>()
                        {
                            new LinkApplicationCompany()
                            {
                                CompanyId = 1,
                                ApplicationId = 1,
                            }
                        }
                    }
                }.AsQueryable());

            Mock<IUserRepository> userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetAllIncludeCompanies())
                .Returns(new List<User>()
                {
                    new User()
                    {
                        Id = currentUserId,
                        FamilyName = "Doe",
                        GivenName = "John",

                        Links = new List<LinkUserCompany>()
                        {
                            new LinkUserCompany()
                            {
                                CompanyId = 2,
                                UserId = currentUserId,
                            }
                        }
                    }
                }.AsQueryable());

            Mock<ICompanyRepository> companyRepositoryMock = new Mock<ICompanyRepository>();
            companyRepositoryMock.Setup(x => x.GetById(It.IsAny<int>()))
                .Returns(new Domain.Entities.Company()
                {
                    Id = 1,
                    Name = "Test Company",

                });

            Mock<ICurrentUser> currentUserMock = new Mock<ICurrentUser>();
            currentUserMock.Setup(x => x.UserId)
                .Returns(currentUserId);


            var handler = new GenerateApplicationLoginCommandHandler(applicationRepositoryMock.Object,
                                                                        companyRepositoryMock.Object,
                                                                        userRepositoryMock.Object,
                                                                        currentUserMock.Object);
            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public async void GenerateApplicationLoginCommandHandler_ShouldReturnFalse_WhencompanyDontHaveAccessToApplikation()
        {
            // Arrange
            string token = Guid.NewGuid().ToString().Replace("-", "");
            int currentUserId = 1;

            var request = new GenerateApplicationLoginCommand
            {
                Token = token,
            };

            Mock<IApplicationRepository> applicationRepositoryMock = new Mock<IApplicationRepository>();
            applicationRepositoryMock.Setup(x => x.GetAllIncludeCompanies())
                .Returns(new List<Domain.Entities.Application>()
                {
                    new Domain.Entities.Application()
                    {
                        Id = 1,
                        Token = token,
                        Name = "Test Application",
                        Url = "https://testapplication.com",
                        Links = new List<LinkApplicationCompany>()
                        {
                            new LinkApplicationCompany()
                            {
                                CompanyId = 2,
                                ApplicationId = 1,
                            }
                        }
                    }
                }.AsQueryable());

            Mock<IUserRepository> userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetAllIncludeCompanies())
                .Returns(new List<User>()
                {
                    new User()
                    {
                        Id = currentUserId,
                        FamilyName = "Doe",
                        GivenName = "John",

                        Links = new List<LinkUserCompany>()
                        {
                            new LinkUserCompany()
                            {
                                CompanyId = 1,
                                UserId = currentUserId,
                            }
                        }
                    }
                }.AsQueryable());

            Mock<ICompanyRepository> companyRepositoryMock = new Mock<ICompanyRepository>();
            companyRepositoryMock.Setup(x => x.GetById(It.IsAny<int>()))
                .Returns(new Domain.Entities.Company()
                {
                    Id = 1,
                    Name = "Test Company",

                });

            Mock<ICurrentUser> currentUserMock = new Mock<ICurrentUser>();
            currentUserMock.Setup(x => x.UserId)
                .Returns(currentUserId);


            var handler = new GenerateApplicationLoginCommandHandler(applicationRepositoryMock.Object,
                                                                        companyRepositoryMock.Object,
                                                                        userRepositoryMock.Object,
                                                                        currentUserMock.Object);
            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
        }


        [Fact]
        public async void GenerateApplicationLoginCommandHandler_ShouldReturnFalse_WhenApplicationNotFountByToken()
        {
            // Arrange
            string token = Guid.NewGuid().ToString().Replace("-", "");
            int currentUserId = 1;

            var request = new GenerateApplicationLoginCommand
            {
                Token = token,
            };

            Mock<IApplicationRepository> applicationRepositoryMock = new Mock<IApplicationRepository>();
            applicationRepositoryMock.Setup(x => x.GetAllIncludeCompanies())
                .Returns(new List<Domain.Entities.Application>()
                {
                    new Domain.Entities.Application()
                    {
                        Id = 1,
                        Token = "token",
                        Name = "Test Application",
                        Url = "https://testapplication.com",
                        Links = new List<LinkApplicationCompany>()
                        {
                            new LinkApplicationCompany()
                            {
                                CompanyId = 2,
                                ApplicationId = 1,
                            }
                        }
                    }
                }.AsQueryable());

            Mock<IUserRepository> userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetAllIncludeCompanies())
                .Returns(new List<User>()
                {
                    new User()
                    {
                        Id = currentUserId,
                        FamilyName = "Doe",
                        GivenName = "John",

                        Links = new List<LinkUserCompany>()
                        {
                            new LinkUserCompany()
                            {
                                CompanyId = 1,
                                UserId = currentUserId,
                            }
                        }
                    }
                }.AsQueryable());

            Mock<ICompanyRepository> companyRepositoryMock = new Mock<ICompanyRepository>();
            companyRepositoryMock.Setup(x => x.GetById(It.IsAny<int>()))
                .Returns(new Domain.Entities.Company()
                {
                    Id = 1,
                    Name = "Test Company",

                });

            Mock<ICurrentUser> currentUserMock = new Mock<ICurrentUser>();
            currentUserMock.Setup(x => x.UserId)
                .Returns(currentUserId);


            var handler = new GenerateApplicationLoginCommandHandler(applicationRepositoryMock.Object,
                                                                        companyRepositoryMock.Object,
                                                                        userRepositoryMock.Object,
                                                                        currentUserMock.Object);
            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
        }
    }
}
