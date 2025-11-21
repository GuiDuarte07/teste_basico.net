using Bernhoeft.GRT.ContractWeb.Domain.SqlServer.ContractStore.Entities;
using Bernhoeft.GRT.ContractWeb.Domain.SqlServer.ContractStore.Interfaces.Repositories;
using Bernhoeft.GRT.Core.Interfaces.Results;
using Bernhoeft.GRT.Teste.Application.Handlers.Queries.v1;
using Bernhoeft.GRT.Teste.Application.Requests.Commands.v1;
using FluentAssertions;
using Moq;
using Xunit;

namespace Bernhoeft.GRT.Teste.UnitTests.Handlers
{
    public class CreateAvisoHandlerTests
    {
        private readonly Mock<IAvisoRepository> _repositoryMock;
        private readonly CreateAvisoHandler _handler;

        public CreateAvisoHandlerTests()
        {
            _repositoryMock = new Mock<IAvisoRepository>();
            _handler = new CreateAvisoHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_WhenRequestIsValid_ShouldReturnCreated()
        {
            // Arrange
            var request = new CreateAvisoRequest
            {
                Titulo = "Título de Teste",
                Mensagem = "Mensagem de Teste"
            };

            var createdEntity = new AvisoEntity
            {
                Titulo = request.Titulo,
                Mensagem = request.Mensagem,
                Ativo = true
            };

            _repositoryMock
                .Setup(r => r.AddAvisoAsync(It.IsAny<AvisoEntity>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(createdEntity);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.StatusCode.ToString().Should().Contain("Created");
            result.Data.Should().NotBeNull();
            result.Data!.Titulo.Should().Be(request.Titulo);
            result.Data.Mensagem.Should().Be(request.Mensagem);
            result.Data.Ativo.Should().BeTrue();
            _repositoryMock.Verify(r => r.AddAvisoAsync(It.Is<AvisoEntity>(e =>
                e.Titulo == request.Titulo &&
                e.Mensagem == request.Mensagem),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenRepositoryReturnsNull_ShouldReturnNotFound()
        {
            // Arrange
            var request = new CreateAvisoRequest
            {
                Titulo = "Título de Teste",
                Mensagem = "Mensagem de Teste"
            };

            _repositoryMock
                .Setup(r => r.AddAvisoAsync(It.IsAny<AvisoEntity>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((AvisoEntity)null!);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.StatusCode.ToString().Should().Contain("NotFound");
        }

        [Fact]
        public async Task Handle_ShouldSetAtivoToTrueByDefault()
        {
            // Arrange
            var request = new CreateAvisoRequest
            {
                Titulo = "Título de Teste",
                Mensagem = "Mensagem de Teste"
            };

            AvisoEntity capturedEntity = null!;
            _repositoryMock
                .Setup(r => r.AddAvisoAsync(It.IsAny<AvisoEntity>(), It.IsAny<CancellationToken>()))
                .Callback<AvisoEntity, CancellationToken>((entity, ct) => capturedEntity = entity)
                .ReturnsAsync((AvisoEntity entity, CancellationToken ct) => entity);

            // Act
            await _handler.Handle(request, CancellationToken.None);

            // Assert
            capturedEntity.Should().NotBeNull();
            capturedEntity.Ativo.Should().BeTrue();
        }
    }
}

