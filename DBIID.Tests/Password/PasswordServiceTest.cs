using DBIID.Application.Services;
using FluentAssertions;
using NetArchTest.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Tests.Password
{
    public class PasswordServiceTest
    {
        [Fact]
        public void Run_Password_Incrypt_Password()
        {
            //Arrange

            PasswordService passwordService = new PasswordService();
            // Act

            string password = "password";
            string hashedPassword = passwordService.IncryptPassword(password).Item1;

            //Assert
            hashedPassword.StartsWith("X").Should().BeTrue();
        }

        [Fact]
        public void Run_Password_Incrypt_Pass()
        {
            //Arrange

            PasswordService passwordService = new PasswordService();
            // Act

            string password = "P@ssw0rd";
            string hashedPassword = passwordService.IncryptPassword(password).Item1;

            //Assert
            hashedPassword.StartsWith("X").Should().BeTrue();
        }

        [Fact]
        public void Run_Password_Incrypt_1234()
        {
            //Arrange

            PasswordService passwordService = new PasswordService();
            // Act

            string password = "1234";
            string hashedPassword = passwordService.IncryptPassword(password).Item1;

            //Assert
            hashedPassword.StartsWith("X").Should().BeTrue();
        }

        [Fact]
        public void Run_Password_Incrypt_Ladida()
        {
            //Arrange

            PasswordService passwordService = new PasswordService();
            // Act

            string password = "Ladida";
            string hashedPassword = passwordService.IncryptPassword(password).Item1;

            //Assert
            hashedPassword.StartsWith("X").Should().BeTrue();
        }

        [Fact]
        public void Run_Password_Incrypt_Langt()
        {
            //Arrange

            PasswordService passwordService = new PasswordService();
            // Act

            string password = "P@ssw0rd";
            string hashedPassword = passwordService.IncryptPassword(password).Item1;

            //Assert
            hashedPassword.StartsWith("X").Should().BeTrue();
        }
    }
}
