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
    public class DeleteAvisoHandlerTests
    {
        private readonly Mock<IAvisoRepository> _repositoryMock;
        private readonly DeleteAvisoHandler _handler;

        public DeleteAvisoHandlerTests()
        {
            _repositoryMock = new Mock<IAvisoRepository>();
            _handler = new DeleteAvisoHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_WhenAvisoExists_ShouldReturnNoContent()
        {
            // Arrange
            var existingAviso = new AvisoEntity
            {
                Id = 1,
                Titulo = "Título",
                Mensagem = "Mensagem",
                Ativo = true
            };

            var request = new DeleteAvisoRequest
            {
                Id = 1
            };

            _repositoryMock
                .Setup(r => r.GetByIdAsync(request.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingAviso);

            _repositoryMock
                .Setup(r => r.SoftDeleteAvisoAsync(It.IsAny<AvisoEntity>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(204); // No Content
            _repositoryMock.Verify(r => r.GetByIdAsync(request.Id, It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(r => r.SoftDeleteAvisoAsync(existingAviso, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenAvisoDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var request = new DeleteAvisoRequest
            {
                Id = 999
            };

            _repositoryMock
                .Setup(r => r.GetByIdAsync(request.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((AvisoEntity)null!);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(404); // Not Found
            result.Messages.Should().Contain(m => m.Contains("não encontrada"));
            _repositoryMock.Verify(r => r.SoftDeleteAvisoAsync(It.IsAny<AvisoEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldPerformSoftDelete()
        {
            // Arrange
            var existingAviso = new AvisoEntity
            {
                Id = 1,
                Titulo = "Título",
                Mensagem = "Mensagem",
                Ativo = true
            };

            var request = new DeleteAvisoRequest
            {
                Id = 1
            };

            AvisoEntity deletedEntity = null!;
            _repositoryMock
                .Setup(r => r.GetByIdAsync(request.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingAviso);

            _repositoryMock
                .Setup(r => r.SoftDeleteAvisoAsync(It.IsAny<AvisoEntity>(), It.IsAny<CancellationToken>()))
                .Callback<AvisoEntity, CancellationToken>((entity, ct) => deletedEntity = entity)
                .Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(request, CancellationToken.None);

            // Assert
            deletedEntity.Should().NotBeNull();
            deletedEntity.Should().BeSameAs(existingAviso);
            // O SoftDeleteAvisoAsync deve setar Ativo = false no repositório
        }
    }
}

