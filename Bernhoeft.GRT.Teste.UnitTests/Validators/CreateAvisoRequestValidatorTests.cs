using Bernhoeft.GRT.Teste.Application.Requests.Commands.v1;
using Bernhoeft.GRT.Teste.Application.Requests.Commands.v1.Validations;
using FluentAssertions;
using Xunit;

namespace Bernhoeft.GRT.Teste.UnitTests.Validators
{
    public class CreateAvisoRequestValidatorTests
    {
        private readonly CreateAvisoRequestValidator _validator;

        public CreateAvisoRequestValidatorTests()
        {
            _validator = new CreateAvisoRequestValidator();
        }

        [Fact]
        public void Validate_WhenRequestIsValid_ShouldReturnValid()
        {
            // Arrange
            var request = new CreateAvisoRequest
            {
                Titulo = "Título válido",
                Mensagem = "Mensagem válida"
            };

            // Act
            var result = _validator.Validate(request);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validate_WhenTituloIsEmpty_ShouldReturnInvalid()
        {
            // Arrange
            var request = new CreateAvisoRequest
            {
                Titulo = string.Empty,
                Mensagem = "Mensagem válida"
            };

            // Act
            var result = _validator.Validate(request);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Titulo" && e.ErrorMessage.Contains("obrigatório"));
        }

        [Fact]
        public void Validate_WhenTituloIsNull_ShouldReturnInvalid()
        {
            // Arrange
            var request = new CreateAvisoRequest
            {
                Titulo = null!,
                Mensagem = "Mensagem válida"
            };

            // Act
            var result = _validator.Validate(request);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Titulo" && e.ErrorMessage.Contains("obrigatório"));
        }

        [Fact]
        public void Validate_WhenTituloExceedsMaxLength_ShouldReturnInvalid()
        {
            // Arrange
            var request = new CreateAvisoRequest
            {
                Titulo = new string('A', 51), // 51 caracteres
                Mensagem = "Mensagem válida"
            };

            // Act
            var result = _validator.Validate(request);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Titulo" && e.ErrorMessage.Contains("máximo 50 caracteres"));
        }

        [Fact]
        public void Validate_WhenTituloIsExactlyMaxLength_ShouldReturnValid()
        {
            // Arrange
            var request = new CreateAvisoRequest
            {
                Titulo = new string('A', 50), // Exatamente 50 caracteres
                Mensagem = "Mensagem válida"
            };

            // Act
            var result = _validator.Validate(request);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validate_WhenMensagemIsEmpty_ShouldReturnInvalid()
        {
            // Arrange
            var request = new CreateAvisoRequest
            {
                Titulo = "Título válido",
                Mensagem = string.Empty
            };

            // Act
            var result = _validator.Validate(request);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Mensagem" && e.ErrorMessage.Contains("obrigatória"));
        }

        [Fact]
        public void Validate_WhenMensagemIsNull_ShouldReturnInvalid()
        {
            // Arrange
            var request = new CreateAvisoRequest
            {
                Titulo = "Título válido",
                Mensagem = null!
            };

            // Act
            var result = _validator.Validate(request);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Mensagem" && e.ErrorMessage.Contains("obrigatória"));
        }

        [Fact]
        public void Validate_WhenBothTituloAndMensagemAreInvalid_ShouldReturnMultipleErrors()
        {
            // Arrange
            var request = new CreateAvisoRequest
            {
                Titulo = null!,
                Mensagem = null!
            };

            // Act
            var result = _validator.Validate(request);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCountGreaterThan(1);
            result.Errors.Should().Contain(e => e.PropertyName == "Titulo");
            result.Errors.Should().Contain(e => e.PropertyName == "Mensagem");
        }
    }
}

