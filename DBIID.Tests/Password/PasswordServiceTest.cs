using DBIID.Application.Features.Auth;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Tests.Password
{
    public class PasswordServiceTest
    {
        private readonly IPasswordService _passwordService;
        public PasswordServiceTest()
        {
            _passwordService = new PasswordService();
        }
        [Fact]
        public void HashPassword_ShouldReturnHashedPassword()
        {
            // Arrange
            string password = "TestPassword123";
            // Act
            string hashedPassword = _passwordService.HashPassword(password);
            // Assert
            hashedPassword.Should().NotBeNullOrEmpty();
            hashedPassword.Should().NotBe(password);
        }
        [Fact]
        public void ValidatePassword_ShouldReturnTrueForValidPassword()
        {
            // Arrange
            string password = "TestPassword123";
            string hashedPassword = _passwordService.HashPassword(password);
            // Act
            bool isValid = _passwordService.ValidatePassword(password, hashedPassword);
            // Assert
            isValid.Should().BeTrue();
        }
        [Fact]
        public void ValidatePassword_ShouldReturnFalseForInvalidPassword()
        {
            // Arrange
            string password = "TestPassword123";
            string wrongPassword = "WrongPassword456";
            string hashedPassword = _passwordService.HashPassword(password);
            // Act
            bool isValid = _passwordService.ValidatePassword(wrongPassword, hashedPassword);
            // Assert
            isValid.Should().BeFalse();
        }
        [Fact]
        public void PasswordMeetsRequirements_ShouldReturnNullForValidPassword()
        {
            // Arrange
            string password = "Test@123";
            // Act
            string result = _passwordService.PasswordMeetsRequirements(password);
            // Assert
            result.Should().BeNullOrWhiteSpace();
        }


        [Fact]
        public void PasswordMeetsRequirements_ShouldReturnErrorMessageForEmptyPassword()
        {
            // Arrange
            string password = string.Empty;
            // Act
            string result = _passwordService.PasswordMeetsRequirements(password);
            // Assert
            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void PasswordMeetsRequirements_ShouldReturnErrorMessageForShortPassword()
        {
            // Arrange
            string password = "short";
            // Act
            string result = _passwordService.PasswordMeetsRequirements(password);
            // Assert
            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void PasswordMeetsRequirements_ShouldReturnErrorMessageForPasswordWithoutUppercase()
        {
            // Arrange
            string password = "lowercase123!";
            // Act
            string result = _passwordService.PasswordMeetsRequirements(password);
            // Assert
            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void PasswordMeetsRequirements_ShouldReturnErrorMessageForPasswordWithoutLowercase()
        {
            // Arrange
            string password = "UPPERCASE123!";
            // Act
            string result = _passwordService.PasswordMeetsRequirements(password);
            // Assert
            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void PasswordMeetsRequirements_ShouldReturnErrorMessageForPasswordWithoutNumbers()
        {
            // Arrange
            string password = "PASSword!";
            // Act
            string result = _passwordService.PasswordMeetsRequirements(password);
            // Assert
            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void PasswordMeetsRequirements_ShouldReturnErrorMessageForPasswordSpecial()
        {
            // Arrange
            string password = "PASSword123";
            // Act
            string result = _passwordService.PasswordMeetsRequirements(password);
            // Assert
            result.Should().NotBeNullOrEmpty();
        }



    }
}

