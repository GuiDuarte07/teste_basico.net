using Bernhoeft.GRT.ContractWeb.Domain.SqlServer.ContractStore.Entities;
using Bernhoeft.GRT.ContractWeb.Domain.SqlServer.ContractStore.Interfaces.Repositories;
using Bernhoeft.GRT.Core.Enums;
using Bernhoeft.GRT.Core.Interfaces.Results;
using Bernhoeft.GRT.Teste.Application.Handlers.Queries.v1;
using Bernhoeft.GRT.Teste.Application.Requests.Queries.v1;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace Bernhoeft.GRT.Teste.UnitTests.Handlers
{
    public class GetAvisosHandlerTests
    {
        private readonly Mock<IServiceProvider> _serviceProviderMock;
        private readonly Mock<IAvisoRepository> _repositoryMock;
        private readonly GetAvisosHandler _handler;

        public GetAvisosHandlerTests()
        {
            _serviceProviderMock = new Mock<IServiceProvider>();
            _repositoryMock = new Mock<IAvisoRepository>();
            
            _serviceProviderMock
                .Setup(sp => sp.GetService(typeof(IAvisoRepository)))
                .Returns(_repositoryMock.Object);

            _handler = new GetAvisosHandler(_serviceProviderMock.Object);
        }

        [Fact]
        public async Task Handle_WhenAvisosExist_ShouldReturnOk()
        {
            // Arrange
            var avisos = new List<AvisoEntity>
            {
                AvisoEntity.CreateForTests(1, "Título 1", "Mensagem 1"),
                AvisoEntity.CreateForTests(2, "Título 2", "Mensagem 2")
            };

            var request = new GetAvisosRequest();

            _repositoryMock
                .Setup(r => r.ObterTodosAvisosAsync(TrackingBehavior.NoTracking, It.IsAny<CancellationToken>()))
                .ReturnsAsync(avisos);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccessTypeResult.Should().BeTrue();
            result.StatusCode.Should().Be(CustomHttpStatusCode.Ok);
            result.Data.Should().NotBeNull();
            result.Data!.Should().HaveCount(2);
            _repositoryMock.Verify(r => r.ObterTodosAvisosAsync(TrackingBehavior.NoTracking, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenNoAvisosExist_ShouldReturnNoContent()
        {
            // Arrange
            var request = new GetAvisosRequest();

            _repositoryMock
                .Setup(r => r.ObterTodosAvisosAsync(TrackingBehavior.NoTracking, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AvisoEntity>());

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccessTypeResult.Should().BeTrue();
            result.StatusCode.Should().Be(CustomHttpStatusCode.NoContent);
            _repositoryMock.Verify(r => r.ObterTodosAvisosAsync(TrackingBehavior.NoTracking, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldUseNoTracking()
        {
            // Arrange
            var avisos = new List<AvisoEntity>
            {
                new AvisoEntity { Titulo = "Título 1", Mensagem = "Mensagem 1", Ativo = true }
            };

            var request = new GetAvisosRequest();

            _repositoryMock
                .Setup(r => r.ObterTodosAvisosAsync(TrackingBehavior.NoTracking, It.IsAny<CancellationToken>()))
                .ReturnsAsync(avisos);

            // Act
            await _handler.Handle(request, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(r => r.ObterTodosAvisosAsync(
                TrackingBehavior.NoTracking, 
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldMapEntitiesToResponses()
        {
            // Arrange
            var avisos = new List<AvisoEntity>
            {
               AvisoEntity.CreateForTests(1, "Título 1", "Mensagem 1"),
               AvisoEntity.CreateForTests(2, "Título 2", "Mensagem 2")
            };

            var request = new GetAvisosRequest();

            _repositoryMock
                .Setup(r => r.ObterTodosAvisosAsync(TrackingBehavior.NoTracking, It.IsAny<CancellationToken>()))
                .ReturnsAsync(avisos);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Data.Should().NotBeNull();
            var responses = result.Data!.ToList();
            responses.Should().HaveCount(2);
            responses[0].Titulo.Should().Be("Título 1");
            responses[1].Titulo.Should().Be("Título 2");
        }
    }
}

