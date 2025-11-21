using System.Net;
using System.Net.Http.Json;
using Bernhoeft.GRT.Teste.Application.Requests.Commands.v1;
using Bernhoeft.GRT.Teste.Application.Responses.Commands.v1;
using Bernhoeft.GRT.Teste.Application.Responses.Queries.v1;
using Bernhoeft.GRT.Teste.IntegrationTests.Fixtures;
using Bernhoeft.GRT.Teste.IntegrationTests.Helpers;
using FluentAssertions;
using Xunit;

namespace Bernhoeft.GRT.Teste.IntegrationTests.Controllers
{
    public class AvisosControllerTests : IClassFixture<WebApplicationFactoryFixture>
    {
        private readonly HttpClient _client;

        public AvisosControllerTests(WebApplicationFactoryFixture factory)
        {
            _client = factory.CreateClient();
        }

        #region GET /avisos/{id}

        [Fact]
        public async Task GetAviso_WhenIdIsValid_ShouldReturnOk()
        {
            // Arrange
            var id = await TestHelpers.CreateAvisoAndGetIdAsync(_client);

            // Act
            var response = await _client.GetAsync($"/api/v1/avisos/{id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var aviso = await TestHelpers.GetAvisoByIdAsync(_client, id);
            aviso.Should().NotBeNull();
            aviso!.Id.Should().Be(id);
        }

        [Fact]
        public async Task GetAviso_WhenIdIsZero_ShouldReturnBadRequest()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/avisos/0");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetAviso_WhenIdIsNegative_ShouldReturnBadRequest()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/avisos/-1");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetAviso_WhenIdDoesNotExist_ShouldReturnNotFound()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/avisos/99999");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion

        #region GET /avisos

        [Fact]
        public async Task GetAvisos_ShouldReturnOk()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/avisos");

            // Assert
            response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task GetAvisos_AfterCreatingAviso_ShouldReturnCreatedAviso()
        {
            // Arrange
            string title = "Título Listagem";
            var createRequest = TestHelpers.CreateValidCreateAvisoRequest(title, "Mensagem Listagem");
            var createResponse = await _client.PostAsJsonAsync("/api/v1/avisos", createRequest);
            createResponse.EnsureSuccessStatusCode();

            // Act
            var response = await _client.GetAsync("/api/v1/avisos");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await TestHelpers.DeserializeResponse<RestResultWrapper<GetAvisosResponse[]>>(response);

            //Verifica se a lista retornada contém ao menos um aviso
            result.Dados.Should().NotBeNull();
            result.Dados.Should().HaveCountGreaterThanOrEqualTo(1);
            result.Dados.Should().Contain(aviso => aviso.Titulo == title);
        }

        #endregion

        #region POST /avisos

        [Fact]
        public async Task CreateAviso_WhenRequestIsValid_ShouldReturnCreated()
        {
            // Arrange
            var request = TestHelpers.CreateValidCreateAvisoRequest("Título Teste", "Mensagem Teste");

            // Act
            var response = await _client.PostAsJsonAsync("/api/v1/avisos", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Verificar que a resposta contém os dados da entidade criada
            var result = await TestHelpers.DeserializeResponse<RestResultWrapper<CreateAvisoResponse>>(response);
            result.Should().NotBeNull();
            result.Dados.Should().NotBeNull();
            result.Dados.Id.Should().BeGreaterThan(0);
            result.Dados.Titulo.Should().Be("Título Teste");
            result.Dados.Mensagem.Should().Be("Mensagem Teste");
            result.Dados.Ativo.Should().BeTrue();
            result.Dados.CriadoEm.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public async Task CreateAviso_ShouldReturnEntityDataWithId()
        {
            // Arrange
            var titulo = "Título com ID";
            var mensagem = "Mensagem com ID";

            // Act
            var createResponse = await TestHelpers.CreateAvisoAndGetResponseAsync(_client, titulo, mensagem);

            // Assert
            createResponse.Should().NotBeNull();
            createResponse!.Id.Should().BeGreaterThan(0);
            createResponse.Titulo.Should().Be(titulo);
            createResponse.Mensagem.Should().Be(mensagem);
            createResponse.Ativo.Should().BeTrue();
            createResponse.CriadoEm.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));

            // Verificar que o ID retornado pode ser usado para buscar o aviso
            var getResponse = await TestHelpers.GetAvisoByIdAsync(_client, createResponse.Id);
            getResponse.Should().NotBeNull();
            getResponse!.Id.Should().Be(createResponse.Id);
            getResponse.Titulo.Should().Be(titulo);
            getResponse.Mensagem.Should().Be(mensagem);
        }

        [Fact]
        public async Task CreateAviso_WhenTituloIsEmpty_ShouldReturnBadRequest()
        {
            // Arrange
            var request = new CreateAvisoRequest
            {
                Titulo = string.Empty,
                Mensagem = "Mensagem válida"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/v1/avisos", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CreateAviso_WhenTituloIsNull_ShouldReturnBadRequest()
        {
            // Arrange
            var request = new CreateAvisoRequest
            {
                Titulo = null!,
                Mensagem = "Mensagem válida"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/v1/avisos", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CreateAviso_WhenMensagemIsEmpty_ShouldReturnBadRequest()
        {
            // Arrange
            var request = new CreateAvisoRequest
            {
                Titulo = "Título válido",
                Mensagem = string.Empty
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/v1/avisos", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CreateAviso_WhenMensagemIsNull_ShouldReturnBadRequest()
        {
            // Arrange
            var request = new CreateAvisoRequest
            {
                Titulo = "Título válido",
                Mensagem = null!
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/v1/avisos", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CreateAviso_WhenTituloExceedsMaxLength_ShouldReturnBadRequest()
        {
            // Arrange
            var request = new CreateAvisoRequest
            {
                Titulo = new string('A', 51), // 51 caracteres
                Mensagem = "Mensagem válida"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/v1/avisos", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        #endregion

        #region PUT /avisos

        [Fact]
        public async Task UpdateAviso_WhenRequestIsValid_ShouldReturnOk()
        {
            // Arrange - Criar um aviso primeiro
            var id = await TestHelpers.CreateAvisoAndGetIdAsync(_client);
            var updateRequest = TestHelpers.CreateValidUpdateAvisoRequest(id, "Mensagem Atualizada pelo Teste");

            // Act
            var response = await _client.PutAsJsonAsync("/api/v1/avisos", updateRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Verificar que a mensagem foi atualizada
            var aviso = await TestHelpers.GetAvisoByIdAsync(_client, id);
            aviso.Should().NotBeNull();
            aviso!.Mensagem.Should().Be("Mensagem Atualizada pelo Teste");
        }

        [Fact]
        public async Task UpdateAviso_WhenIdIsZero_ShouldReturnBadRequest()
        {
            // Arrange
            var request = new UpdateAvisoRequest
            {
                Id = 0,
                Mensagem = "Mensagem válida"
            };

            // Act
            var response = await _client.PutAsJsonAsync("/api/v1/avisos", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UpdateAviso_WhenIdIsNegative_ShouldReturnBadRequest()
        {
            // Arrange
            var request = new UpdateAvisoRequest
            {
                Id = -1,
                Mensagem = "Mensagem válida"
            };

            // Act
            var response = await _client.PutAsJsonAsync("/api/v1/avisos", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UpdateAviso_WhenMensagemIsEmpty_ShouldReturnBadRequest()
        {
            // Arrange
            var request = new UpdateAvisoRequest
            {
                Id = 1,
                Mensagem = string.Empty
            };

            // Act
            var response = await _client.PutAsJsonAsync("/api/v1/avisos", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UpdateAviso_WhenIdDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var request = new UpdateAvisoRequest
            {
                Id = 99999,
                Mensagem = "Mensagem válida"
            };

            // Act
            var response = await _client.PutAsJsonAsync("/api/v1/avisos", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion

        #region DELETE /avisos/{id}

        [Fact]
        public async Task DeleteAviso_WhenIdIsValid_ShouldReturnNoContent()
        {
            // Arrange - Criar um aviso primeiro
            var id = await TestHelpers.CreateAvisoAndGetIdAsync(_client);

            // Act
            var response = await _client.DeleteAsync($"/api/v1/avisos/{id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Verificar que o aviso não aparece mais nas buscas (soft delete)
            var getResponse = await _client.GetAsync($"/api/v1/avisos/{id}");
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteAviso_WhenIdIsZero_ShouldReturnBadRequest()
        {
            // Act
            var response = await _client.DeleteAsync("/api/v1/avisos/0");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task DeleteAviso_WhenIdIsNegative_ShouldReturnBadRequest()
        {
            // Act
            var response = await _client.DeleteAsync("/api/v1/avisos/-1");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task DeleteAviso_WhenIdDoesNotExist_ShouldReturnNotFound()
        {
            // Act
            var response = await _client.DeleteAsync("/api/v1/avisos/99999");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion
    }
}

