using Bernhoeft.GRT.Teste.Application.Requests.Commands.v1;
using Bernhoeft.GRT.Teste.Application.Requests.Commands.v1.Validations;
using FluentAssertions;
using Xunit;

namespace Bernhoeft.GRT.Teste.UnitTests.Validators
{
    public class DeleteAvisoRequestValidatorTests
    {
        private readonly DeleteAvisoRequestValidator _validator;

        public DeleteAvisoRequestValidatorTests()
        {
            _validator = new DeleteAvisoRequestValidator();
        }

        [Fact]
        public void Validate_WhenIdIsValid_ShouldReturnValid()
        {
            // Arrange
            var request = new DeleteAvisoRequest
            {
                Id = 1
            };

            // Act
            var result = _validator.Validate(request);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validate_WhenIdIsZero_ShouldReturnInvalid()
        {
            // Arrange
            var request = new DeleteAvisoRequest
            {
                Id = 0
            };

            // Act
            var result = _validator.Validate(request);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Id" && e.ErrorMessage.Contains("maior que zero"));
        }

        [Fact]
        public void Validate_WhenIdIsNegative_ShouldReturnInvalid()
        {
            // Arrange
            var request = new DeleteAvisoRequest
            {
                Id = -1
            };

            // Act
            var result = _validator.Validate(request);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Id" && e.ErrorMessage.Contains("maior que zero"));
        }

        [Fact]
        public void Validate_WhenIdIsPositive_ShouldReturnValid()
        {
            // Arrange
            var request = new DeleteAvisoRequest
            {
                Id = 999
            };

            // Act
            var result = _validator.Validate(request);

            // Assert
            result.IsValid.Should().BeTrue();
        }
    }
}

