using Bernhoeft.GRT.ContractWeb.Domain.SqlServer.ContractStore.Entities;
using Bernhoeft.GRT.ContractWeb.Domain.SqlServer.ContractStore.Interfaces.Repositories;
using Bernhoeft.GRT.Core.Enums;
using Bernhoeft.GRT.Core.Interfaces.Results;
using Bernhoeft.GRT.Teste.Application.Handlers.Queries.v1;
using Bernhoeft.GRT.Teste.Application.Requests.Queries.v1;
using FluentAssertions;
using Moq;
using Xunit;

namespace Bernhoeft.GRT.Teste.UnitTests.Handlers
{
    public class GetAvisoByIdHandlerTests
    {
        private readonly Mock<IAvisoRepository> _repositoryMock;
        private readonly GetAvisoByIdHandler _handler;

        public GetAvisoByIdHandlerTests()
        {
            _repositoryMock = new Mock<IAvisoRepository>();
            _handler = new GetAvisoByIdHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_WhenAvisoExists_ShouldReturnOk()
        {
            // Arrange
            var aviso = new AvisoEntity
            {
                Id = 1,
                Titulo = "Título de Teste",
                Mensagem = "Mensagem de Teste",
                Ativo = true
            };

            var request = new GetAvisoByIdRequest
            {
                Id = 1
            };

            _repositoryMock
                .Setup(r => r.ObterAvisoByIdAsync(request.Id, TrackingBehavior.NoTracking, It.IsAny<CancellationToken>()))
                .ReturnsAsync(aviso);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200); // OK
            result.Data.Should().NotBeNull();
            result.Data!.Id.Should().Be(aviso.Id);
            result.Data.Titulo.Should().Be(aviso.Titulo);
            result.Data.Mensagem.Should().Be(aviso.Mensagem);
            _repositoryMock.Verify(r => r.ObterAvisoByIdAsync(request.Id, TrackingBehavior.NoTracking, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenAvisoDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var request = new GetAvisoByIdRequest
            {
                Id = 999
            };

            _repositoryMock
                .Setup(r => r.ObterAvisoByIdAsync(request.Id, TrackingBehavior.NoTracking, It.IsAny<CancellationToken>()))
                .ReturnsAsync((AvisoEntity)null!);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(404); // Not Found
            result.Data.Should().BeNull();
            _repositoryMock.Verify(r => r.ObterAvisoByIdAsync(request.Id, TrackingBehavior.NoTracking, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldUseNoTracking()
        {
            // Arrange
            var aviso = new AvisoEntity
            {
                Id = 1,
                Titulo = "Título de Teste",
                Mensagem = "Mensagem de Teste",
                Ativo = true
            };

            var request = new GetAvisoByIdRequest
            {
                Id = 1
            };

            _repositoryMock
                .Setup(r => r.ObterAvisoByIdAsync(request.Id, TrackingBehavior.NoTracking, It.IsAny<CancellationToken>()))
                .ReturnsAsync(aviso);

            // Act
            await _handler.Handle(request, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(r => r.ObterAvisoByIdAsync(
                request.Id, 
                TrackingBehavior.NoTracking, 
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

