using Bernhoeft.GRT.Teste.Application.Requests.Queries.v1;
using Bernhoeft.GRT.Teste.Application.Requests.Queries.v1.Validations;
using FluentAssertions;
using Xunit;

namespace Bernhoeft.GRT.Teste.UnitTests.Validators
{
    public class GetAvisoByIdRequestValidatorTests
    {
        private readonly GetAvisoByIdRequestValidator _validator;

        public GetAvisoByIdRequestValidatorTests()
        {
            _validator = new GetAvisoByIdRequestValidator();
        }

        [Fact]
        public void Validate_WhenIdIsValid_ShouldReturnValid()
        {
            // Arrange
            var request = new GetAvisoByIdRequest
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
            var request = new GetAvisoByIdRequest
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
            var request = new GetAvisoByIdRequest
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
            var request = new GetAvisoByIdRequest
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

