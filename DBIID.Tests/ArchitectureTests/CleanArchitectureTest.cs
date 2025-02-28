using FluentAssertions;
using NetArchTest.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Tests.ArchitectureTests
{
    public class CleanArchitectureTest
    {
        private string ApiNamespace = string.Empty;
        private string ApiClientNamespace = string.Empty;
        private string ApplicationNamespace = string.Empty;
        private string DomainNamespace = string.Empty;
        private string InfrastructureNamespace = string.Empty;
        private string SharedNamespace = string.Empty;

        public CleanArchitectureTest()
        {
            ApiNamespace = typeof(DBIID.API.AssemblyReference).Namespace;
            ApiClientNamespace = typeof(DBIID.API.Client.AssemblyReference).Namespace;
            ApplicationNamespace = typeof(DBIID.Application.AssemblyReference).Namespace;
            DomainNamespace = typeof(DBIID.Domain.AssemblyReference).Namespace;
            InfrastructureNamespace = typeof(DBIID.Infrastructure.AssemblyReference).Namespace;
            SharedNamespace = typeof(DBIID.Shared.AssemblyReference).Namespace;
        }


        [Fact]
        public void Domain_Shouldnt_Have_Any_DependenciesOnOtherProjects()
        {
            //Arrange
            var type = typeof(Domain.AssemblyReference);
            // Act

            var otherProjects = new[]
            {
                ApiNamespace,
                ApiClientNamespace,
                ApplicationNamespace,
                InfrastructureNamespace,
                SharedNamespace
            };

            var testResult = Types
                .InAssembly(type.Assembly)
                .That()
                .ResideInNamespace(type.Namespace)
                .ShouldNot()
                .HaveDependencyOnAll(otherProjects)
                .GetResult();

            //Assert
            testResult.FailingTypes.Should().BeNullOrEmpty();
            testResult.IsSuccessful.Should().BeTrue();
        }
        [Fact]
        public void Application_Should_Have_One_DependenciesOnOtherProjects()
        {
            //Arrange
            var type = typeof(Application.AssemblyReference);
            // Act

            var otherProjects = new[]
            {
                ApiNamespace,
                ApiClientNamespace,
                ApplicationNamespace,
                InfrastructureNamespace,
            };

            var testResult = Types
                .InAssembly(type.Assembly)
                .That()
                .ResideInNamespace(type.Namespace)
                .ShouldNot()
                .HaveDependencyOnAll(otherProjects)
                .GetResult();

            //Assert
            testResult.FailingTypes.Should().BeNullOrEmpty();
            testResult.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Infrastructure_Should_Have_Two_DependenciesOnOtherProjects()
        {
            //Arrange
            var type = typeof(Infrastructure.AssemblyReference);
            // Act

            var shouldNotDependOnProjects = new[]
{
                ApiNamespace,
                ApiClientNamespace,
            };

            var shouldDependOnProjects = new[]
            {
                ApplicationNamespace,
                DomainNamespace
            };

            var testResult = Types
                .InAssembly(type.Assembly)
                .That()
                .ResideInNamespace(type.Namespace)
                .ShouldNot()
                .HaveDependencyOnAll(shouldNotDependOnProjects)
                .And()
                .HaveDependencyOnAll(shouldDependOnProjects)
                .GetResult();

            //Assert
            testResult.FailingTypes.Should().BeNullOrEmpty();
            testResult.IsSuccessful.Should().BeTrue();
        }

    }
}
