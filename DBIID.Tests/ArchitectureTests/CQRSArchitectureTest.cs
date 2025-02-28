using DBIID.Application.Common.Handlers;
using FluentAssertions;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Tests.ArchitectureTests
{
    public class CQRSArchitectureTest
    {
        private const string _interfacePrefix = "I";

        public CQRSArchitectureTest()
        {

        }


        [Fact]
        public void Check_All_CommandHandlers_Uses_ICommandHandlerInterfaceTest()
        {
            //Arrange
            var assembly = typeof(Application.AssemblyReference).Assembly;
            var commandHandlerSuffix = "CommandHandler";
            // Act
            var commandHandlers = assembly.GetTypes()
                .Where(t => t.Name.EndsWith(commandHandlerSuffix) && t.IsClass && !t.IsAbstract)
                .ToList();

            var handlersNotImplementingInterface = commandHandlers.
                                                    Where(x => !x.GetInterfaces()
                                                          .Any(x => x.Name.Contains(_interfacePrefix + commandHandlerSuffix)));


            // Assert
            handlersNotImplementingInterface.Should().BeEmpty();


        }


        [Fact]
        public void Check_All_Validators_Have_A_Named_Ending_With_Validator()
        {
            //Arrange
            var assembly = typeof(Application.AssemblyReference).Assembly;
            var validatorSuffix = "Validator";

            var validatorTypes = assembly.GetTypes()
               .Where(t => t.BaseType != null
                           && t.BaseType.IsGenericType
                           && t.BaseType.GetGenericTypeDefinition() == typeof(AbstractValidator<>))
               .ToList();

            //Act
            var namesNotEndingWithValidator = validatorTypes.Where(validatorType =>
                                                       !validatorType.Name.EndsWith(validatorSuffix));


            //Assert
            namesNotEndingWithValidator.Should()
                .BeEmpty("each validator should have a name ending with Validator");
        }


        [Fact]
        public void Check_All_QueryHandlers_Uses_IQueryHandlerInterfaceTest()
        {
            //Arrange
            var assembly = typeof(Application.AssemblyReference).Assembly;
            var iQueryType = typeof(IQueryHandler<,>);
            string queryHandlerSuffix = "QueryHandler";
            // Act
            var queryHandlers = assembly.GetTypes()
                .Where(t => t.Name.EndsWith(queryHandlerSuffix) && t.IsClass && !t.IsAbstract)
                .ToList();

            var handlersNotImplementingInterface = queryHandlers.
                                                   Where(x => !x.GetInterfaces()
                                                         .Any(x => x.Name.Contains(_interfacePrefix + queryHandlerSuffix)));

            // Assert
            handlersNotImplementingInterface.Should().BeEmpty();

        }
    }
}
