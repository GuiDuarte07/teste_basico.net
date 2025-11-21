using Bernhoeft.GRT.ContractWeb.Domain.SqlServer.ContractStore.Entities;
using Bernhoeft.GRT.ContractWeb.Domain.SqlServer.ContractStore.Interfaces.Repositories;
using Bernhoeft.GRT.Core.Enums;
using Bernhoeft.GRT.Core.Interfaces.Results;
using Bernhoeft.GRT.Teste.Application.Handlers.Queries.v1;
using Bernhoeft.GRT.Teste.Application.Requests.Commands.v1;
using FluentAssertions;
using Moq;
using Xunit;

namespace Bernhoeft.GRT.Teste.UnitTests.Handlers
{
    public class UpdateAvisoHandlerTests
    {
        private readonly Mock<IAvisoRepository> _repositoryMock;
        private readonly UpdateAvisoHandler _handler;

        public UpdateAvisoHandlerTests()
        {
            _repositoryMock = new Mock<IAvisoRepository>();
            _handler = new UpdateAvisoHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_WhenAvisoExists_ShouldReturnOk()
        {
            // Arrange
            var existingAviso = AvisoEntity.CreateForTests(1, "Título Original", "Mensagem Original");

            var request = new UpdateAvisoRequest
            {
                Id = 1,
                Mensagem = "Mensagem Atualizada"
            };

            _repositoryMock
                .Setup(r => r.GetByIdAsync(request.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingAviso);

            _repositoryMock
                .Setup(r => r.UpdateAvisoAsync(It.IsAny<AvisoEntity>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((AvisoEntity entity, CancellationToken ct) => entity);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(CustomHttpStatusCode.Ok); // OK
            _repositoryMock.Verify(r => r.GetByIdAsync(request.Id, It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(r => r.UpdateAvisoAsync(It.IsAny<AvisoEntity>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenAvisoDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var request = new UpdateAvisoRequest
            {
                Id = 999,
                Mensagem = "Mensagem Atualizada"
            };

            _repositoryMock
                .Setup(r => r.GetByIdAsync(request.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((AvisoEntity)null!);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(CustomHttpStatusCode.NotFound); // Not Found
            result.Messages.Should().Contain(m => m.Contains("não encontrada"));
            _repositoryMock.Verify(r => r.UpdateAvisoAsync(It.IsAny<AvisoEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenMensagemIsProvided_ShouldUpdateMensagem()
        {
            // Arrange
            var existingAviso = AvisoEntity.CreateForTests(1, "Título Original", "Mensagem Original");

            var request = new UpdateAvisoRequest
            {
                Id = 1,
                Mensagem = "Nova Mensagem"
            };

            AvisoEntity updatedEntity = null!;
            _repositoryMock
                .Setup(r => r.GetByIdAsync(request.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingAviso);

            _repositoryMock
                .Setup(r => r.UpdateAvisoAsync(It.IsAny<AvisoEntity>(), It.IsAny<CancellationToken>()))
                .Callback<AvisoEntity, CancellationToken>((entity, ct) => updatedEntity = entity)
                .ReturnsAsync((AvisoEntity entity, CancellationToken ct) => entity);

            // Act
            await _handler.Handle(request, CancellationToken.None);

            // Assert
            updatedEntity.Should().NotBeNull();
            updatedEntity.Mensagem.Should().Be("Nova Mensagem");
            updatedEntity.Titulo.Should().Be("Título Original"); // Não deve mudar
        }

        [Fact]
        public async Task Handle_WhenMensagemIsProvided_ShouldCallSetUpdatedNow()
        {
            // Arrange
            var existingAviso = AvisoEntity.CreateForTests(1, "Título Original", "Mensagem Original");

            var request = new UpdateAvisoRequest
            {
                Id = 1,
                Mensagem = "Nova Mensagem"
            };

            AvisoEntity updatedEntity = null!;
            _repositoryMock
                .Setup(r => r.GetByIdAsync(request.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingAviso);

            _repositoryMock
                .Setup(r => r.UpdateAvisoAsync(It.IsAny<AvisoEntity>(), It.IsAny<CancellationToken>()))
                .Callback<AvisoEntity, CancellationToken>((entity, ct) => updatedEntity = entity)
                .ReturnsAsync((AvisoEntity entity, CancellationToken ct) => entity);

            // Act
            await _handler.Handle(request, CancellationToken.None);

            // Assert
            updatedEntity.Should().NotBeNull();
            updatedEntity.AtualizadoEm.Should().NotBeNull(); // SetUpdatedNow foi chamado
        }

        [Fact]
        public async Task Handle_WhenMensagemIsNull_ShouldNotUpdate()
        {
            // Arrange
            var existingAviso = AvisoEntity.CreateForTests(1, "Título Original", "Mensagem Original");

            var request = new UpdateAvisoRequest
            {
                Id = 1,
                Mensagem = null
            };

            AvisoEntity updatedEntity = null!;
            _repositoryMock
                .Setup(r => r.GetByIdAsync(request.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingAviso);

            _repositoryMock
                .Setup(r => r.UpdateAvisoAsync(It.IsAny<AvisoEntity>(), It.IsAny<CancellationToken>()))
                .Callback<AvisoEntity, CancellationToken>((entity, ct) => updatedEntity = entity)
                .ReturnsAsync((AvisoEntity entity, CancellationToken ct) => entity);

            // Act
            await _handler.Handle(request, CancellationToken.None);

            // Assert
            updatedEntity.Should().NotBeNull();
            updatedEntity.Mensagem.Should().Be("Mensagem Original"); // Não deve mudar quando null
        }
    }
}

