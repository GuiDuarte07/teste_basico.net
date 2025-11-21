using System.Net.Http.Json;
using System.Text.Json;
using Bernhoeft.GRT.Teste.Application.Requests.Commands.v1;
using Bernhoeft.GRT.Teste.Application.Responses.Commands.v1;
using Bernhoeft.GRT.Teste.Application.Responses.Queries.v1;

namespace Bernhoeft.GRT.Teste.IntegrationTests.Helpers
{
    public static class TestHelpers
    {
        // Cached JsonSerializerOptions
        private static readonly JsonSerializerOptions CachedJsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public static CreateAvisoRequest CreateValidCreateAvisoRequest(string? titulo = null, string? mensagem = null)
        {
            return new CreateAvisoRequest
            {
                Titulo = titulo ?? "Título de Teste",
                Mensagem = mensagem ?? "Mensagem de Teste"
            };
        }

        public static UpdateAvisoRequest CreateValidUpdateAvisoRequest(int id, string? mensagem = null)
        {
            return new UpdateAvisoRequest
            {
                Id = id,
                Mensagem = mensagem ?? "Mensagem Atualizada"
            };
        }

        public static async Task<T?> DeserializeResponse<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(content))
                return default;

            return JsonSerializer.Deserialize<T>(content, CachedJsonOptions);
        }

        public static async Task<int> CreateAvisoAndGetIdAsync(HttpClient client)
        {
            var createRequest = CreateValidCreateAvisoRequest();
            var createResponse = await client.PostAsJsonAsync("/api/v1/avisos", createRequest);
            createResponse.EnsureSuccessStatusCode();

            // Obter o ID diretamente da resposta de criação
            var result = await DeserializeResponse<RestResultWrapper<CreateAvisoResponse>>(createResponse);
            if (result?.Dados != null)
            {
                return result.Dados.Id;
            }

            throw new InvalidOperationException("Não foi possível obter o ID do aviso criado");
        }

        public static async Task<CreateAvisoResponse?> CreateAvisoAndGetResponseAsync(HttpClient client, string? titulo = null, string? mensagem = null)
        {
            var createRequest = CreateValidCreateAvisoRequest(titulo, mensagem);
            var createResponse = await client.PostAsJsonAsync("/api/v1/avisos", createRequest);
            createResponse.EnsureSuccessStatusCode();

            var result = await DeserializeResponse<RestResultWrapper<CreateAvisoResponse>>(createResponse);
            return result?.Dados;
        }

        public static async Task<GetAvisoByIdResponse?> GetAvisoByIdAsync(HttpClient client, int id)
        {
            var response = await client.GetAsync($"/api/v1/avisos/{id}");
            if (!response.IsSuccessStatusCode)
                return null;

            var result = await DeserializeResponse<RestResultWrapper<GetAvisoByIdResponse>>(response);
            return result?.Dados;
        }
    }

    // Wrapper para deserializar a resposta do tipo IDocumentationRestResult
    public class RestResultWrapper<T>
    {
        public T? Dados { get; set; }
        //public int StatusCode { get; set; }
        public List<string>? Messages { get; set; }
    }

}

