using Bernhoeft.GRT.Teste.Application.Requests.Commands.v1;
using Bernhoeft.GRT.Teste.Application.Requests.Commands.v1.Validations;
using FluentAssertions;
using Xunit;

namespace Bernhoeft.GRT.Teste.UnitTests.Validators
{
    public class UpdateAvisoRequestValidatorTests
    {
        private readonly UpdateAvisoRequestValidator _validator;

        public UpdateAvisoRequestValidatorTests()
        {
            _validator = new UpdateAvisoRequestValidator();
        }

        [Fact]
        public void Validate_WhenRequestIsValid_ShouldReturnValid()
        {
            // Arrange
            var request = new UpdateAvisoRequest
            {
                Id = 1,
                Mensagem = "Mensagem válida"
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
            var request = new UpdateAvisoRequest
            {
                Id = 0,
                Mensagem = "Mensagem válida"
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
            var request = new UpdateAvisoRequest
            {
                Id = -1,
                Mensagem = "Mensagem válida"
            };

            // Act
            var result = _validator.Validate(request);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Id" && e.ErrorMessage.Contains("maior que zero"));
        }

        [Fact]
        public void Validate_WhenMensagemIsEmpty_ShouldReturnInvalid()
        {
            // Arrange
            var request = new UpdateAvisoRequest
            {
                Id = 1,
                Mensagem = string.Empty
            };

            // Act
            var result = _validator.Validate(request);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Mensagem" && e.ErrorMessage.Contains("não pode ser vazia"));
        }

        [Fact]
        public void Validate_WhenMensagemIsNull_ShouldReturnValid()
        {
            // Arrange
            var request = new UpdateAvisoRequest
            {
                Id = 1,
                Mensagem = null
            };

            // Act
            var result = _validator.Validate(request);

            // Assert
            result.IsValid.Should().BeTrue(); // Mensagem é opcional, só valida se informada
        }

        [Fact]
        public void Validate_WhenMensagemIsWhitespace_ShouldReturnInvalid()
        {
            // Arrange
            var request = new UpdateAvisoRequest
            {
                Id = 1,
                Mensagem = "   "
            };

            // Act
            var result = _validator.Validate(request);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Mensagem" && e.ErrorMessage.Contains("não pode ser vazia"));
        }
    }
}

