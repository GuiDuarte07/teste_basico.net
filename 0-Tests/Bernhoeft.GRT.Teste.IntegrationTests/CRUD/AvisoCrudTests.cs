using System.Net;
using System.Net.Http.Json;
using Bernhoeft.GRT.Teste.Application.Requests.Commands.v1;
using Bernhoeft.GRT.Teste.IntegrationTests.Fixtures;
using Bernhoeft.GRT.Teste.IntegrationTests.Helpers;
using FluentAssertions;
using Xunit;

namespace Bernhoeft.GRT.Teste.IntegrationTests.CRUD
{
    public class AvisoCrudTests : IClassFixture<WebApplicationFactoryFixture>
    {
        private readonly HttpClient _client;

        public AvisoCrudTests(WebApplicationFactoryFixture factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CompleteCrudFlow_ShouldWorkCorrectly()
        {
            // CREATE
            var createResponse = await TestHelpers.CreateAvisoAndGetResponseAsync(_client, "Título CRUD", "Mensagem CRUD");
            createResponse.Should().NotBeNull();
            createResponse!.Titulo.Should().Be("Título CRUD");
            createResponse.Mensagem.Should().Be("Mensagem CRUD");
            createResponse.Ativo.Should().BeTrue();
            createResponse.CriadoEm.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));

            // Obter o ID do aviso criado
            var id = createResponse.Id;

            // READ - Get All
            var getAllResponse = await _client.GetAsync("/api/v1/avisos");
            getAllResponse.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NoContent);

            // READ - Get By Id
            var getByIdResponse = await _client.GetAsync($"/api/v1/avisos/{id}");
            getByIdResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var aviso = await TestHelpers.GetAvisoByIdAsync(_client, id);
            aviso.Should().NotBeNull();
            aviso!.Titulo.Should().Be("Título CRUD");
            aviso.Mensagem.Should().Be("Mensagem CRUD");
            aviso.Ativo.Should().BeTrue();

            // UPDATE
            var updateRequest = TestHelpers.CreateValidUpdateAvisoRequest(id, "Mensagem Atualizada no CRUD");
            var updateResponse = await _client.PutAsJsonAsync("/api/v1/avisos", updateRequest);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // Verificar atualização
            var updatedAviso = await TestHelpers.GetAvisoByIdAsync(_client, id);
            updatedAviso.Should().NotBeNull();
            updatedAviso!.Mensagem.Should().Be("Mensagem Atualizada no CRUD");

            // DELETE
            var deleteResponse = await _client.DeleteAsync($"/api/v1/avisos/{id}");
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // VERIFY - Tentar buscar após deletar (soft delete)
            var getAfterDeleteResponse = await _client.GetAsync($"/api/v1/avisos/{id}");
            // Após soft delete, o aviso não deve aparecer (filtro global)
            getAfterDeleteResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateMultipleAvisos_ShouldAllBeCreated()
        {
            // Arrange & Act
            var createdAvisos = new List<Application.Responses.Commands.v1.CreateAvisoResponse>();

            var aviso1 = await TestHelpers.CreateAvisoAndGetResponseAsync(_client, "Título 1", "Mensagem 1");
            var aviso2 = await TestHelpers.CreateAvisoAndGetResponseAsync(_client, "Título 2", "Mensagem 2");
            var aviso3 = await TestHelpers.CreateAvisoAndGetResponseAsync(_client, "Título 3", "Mensagem 3");

            // Assert - Verificar que todos foram criados com dados corretos
            aviso1.Should().NotBeNull();
            aviso1!.Titulo.Should().Be("Título 1");
            aviso1.Mensagem.Should().Be("Mensagem 1");
            aviso1.Id.Should().BeGreaterThan(0);

            aviso2.Should().NotBeNull();
            aviso2!.Titulo.Should().Be("Título 2");
            aviso2.Mensagem.Should().Be("Mensagem 2");
            aviso2.Id.Should().BeGreaterThan(0);
            aviso2.Id.Should().NotBe(aviso1.Id); // IDs devem ser diferentes

            aviso3.Should().NotBeNull();
            aviso3!.Titulo.Should().Be("Título 3");
            aviso3.Mensagem.Should().Be("Mensagem 3");
            aviso3.Id.Should().BeGreaterThan(0);
            aviso3.Id.Should().NotBe(aviso1.Id);
            aviso3.Id.Should().NotBe(aviso2.Id);

            // Verificar que todos aparecem na listagem
            var getAllResponse = await _client.GetAsync("/api/v1/avisos");
            getAllResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task SoftDelete_ShouldNotReturnAvisoInList()
        {
            // Arrange - Criar um aviso
            var id = await TestHelpers.CreateAvisoAndGetIdAsync(_client);

            // Verificar que o aviso existe antes do delete
            var avisoBeforeDelete = await TestHelpers.GetAvisoByIdAsync(_client, id);
            avisoBeforeDelete.Should().NotBeNull();
            avisoBeforeDelete!.Ativo.Should().BeTrue();

            // Act - Deletar (soft delete)
            var deleteResponse = await _client.DeleteAsync($"/api/v1/avisos/{id}");
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Assert - Verificar que não aparece na busca por ID
            var getByIdResponse = await _client.GetAsync($"/api/v1/avisos/{id}");
            getByIdResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);

            // Assert - Verificar que não aparece na lista
            var getAllResponse = await _client.GetAsync("/api/v1/avisos");
            if (getAllResponse.IsSuccessStatusCode)
            {
                var content = await getAllResponse.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                {
                    var result = await TestHelpers.DeserializeResponse<RestResultWrapper<IEnumerable<Application.Responses.Queries.v1.GetAvisosResponse>>>(getAllResponse);
                    if (result?.Dados != null)
                    {
                        result.Dados.Should().NotContain(a => a.Id == id);
                    }
                }
            }
        }

        [Fact]
        public async Task UpdateAviso_ShouldUpdateMensagemAndSetAtualizadoEm()
        {
            // Arrange - Criar um aviso
            var id = await TestHelpers.CreateAvisoAndGetIdAsync(_client);

            // Act - Atualizar
            var updateRequest = new UpdateAvisoRequest
            {
                Id = id,
                Mensagem = "Mensagem Atualizada"
            };
            var updateResponse = await _client.PutAsJsonAsync("/api/v1/avisos", updateRequest);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // Assert - Verificar atualização
            var updatedAviso = await TestHelpers.GetAvisoByIdAsync(_client, id);
            updatedAviso.Should().NotBeNull();
            updatedAviso!.Mensagem.Should().Be("Mensagem Atualizada");
        }

        [Fact]
        public async Task CreateAviso_ShouldSetCriadoEm()
        {
            // Arrange
            var beforeCreation = DateTime.Now;

            // Act
            var createResponse = await TestHelpers.CreateAvisoAndGetResponseAsync(_client, "Título Data", "Mensagem Data");

            var afterCreation = DateTime.Now;

            // Assert - Verificar que foi criado com dados corretos
            createResponse.Should().NotBeNull();
            createResponse!.Titulo.Should().Be("Título Data");
            createResponse.Mensagem.Should().Be("Mensagem Data");
            createResponse.CriadoEm.Should().BeOnOrAfter(beforeCreation);
            createResponse.CriadoEm.Should().BeOnOrBefore(afterCreation);
            createResponse.Id.Should().BeGreaterThan(0);
        }
    }
}

