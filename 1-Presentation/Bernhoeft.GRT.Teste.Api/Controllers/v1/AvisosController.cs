using Bernhoeft.GRT.Teste.Application.Requests.Commands.v1;
using Bernhoeft.GRT.Teste.Application.Requests.Queries.v1;
using Bernhoeft.GRT.Teste.Application.Responses.Commands.v1;
using Bernhoeft.GRT.Teste.Application.Responses.Queries.v1;

namespace Bernhoeft.GRT.Teste.Api.Controllers.v1
{
    /// <response code="401">Não Autenticado.</response>
    /// <response code="403">Não Autorizado.</response>
    /// <response code="500">Erro Interno no Servidor.</response>
    [AllowAnonymous]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = null)]
    [ProducesResponseType(StatusCodes.Status403Forbidden, Type = null)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = null)]
    public class AvisosController : RestApiController
    {
        /// <summary>
        /// Retorna um Aviso por ID.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Aviso com id informado.</returns>
        /// <response code="200">Sucesso.</response>
        /// <response code="400">Dados Inválidos.</response>
        /// <response code="404">Aviso Não Encontrado.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IDocumentationRestResult<GetAvisoByIdResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[JwtAuthorize(Roles = AuthorizationRoles.CONTRACTLAYOUT_SISTEMA_AVISO_PESQUISAR)]
        public async Task<object> GetAviso([FromRoute] GetAvisoByIdRequest request, CancellationToken cancellationToken)
             => await Mediator.Send(request, cancellationToken);

        /// <summary>
        /// Retorna Todos os Avisos Cadastrados para Tela de Edição.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>Lista com Todos os Avisos.</returns>
        /// <response code="200">Sucesso.</response>
        /// <response code="204">Sem Avisos.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IDocumentationRestResult<IEnumerable<GetAvisosResponse>>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<object> GetAvisos(CancellationToken cancellationToken)
            => await Mediator.Send(new GetAvisosRequest(), cancellationToken);

        /// <summary>
        /// Cria um Novo Aviso.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>entidade criada</returns>
        /// <response code="200">Criado com Sucesso.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IDocumentationRestResult<CreateAvisoResponse>))]
        public async Task<object> CreateAviso([FromBody] CreateAvisoRequest request, CancellationToken cancellationToken)
        {
            //Status 201 também seria adequado aqui, mas OperationResult.ReturnCreated não espera um objeto de resposta.
            return await Mediator.Send(request, cancellationToken);
        }

        /// <summary>
        /// Atualizar um Aviso.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>entidade atualizada</returns>
        /// <response code="200">Atualizado com Sucesso.</response>
        [HttpPut()]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IDocumentationRestResult<UpdateAvisoResponse>))]
        public async Task<object> UpdateAviso([FromBody] UpdateAvisoRequest request, CancellationToken cancellationToken)
        {
            return await Mediator.Send(request, cancellationToken);
        }

        /// <summary>
        /// Deletar um Aviso.
        /// </summary>
        /// <param name="request"></param>"
        /// <param name="cancellationToken"></param>"
        /// <returns>entidade deletada</returns>
        /// <response code="204">Deletado com Sucesso.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(IDocumentationRestResult<DeleteAvisoResponse>))]
        public async Task<object> DeleteAviso([FromRoute] DeleteAvisoRequest request, CancellationToken cancellationToken)
        {
            return await Mediator.Send(request, cancellationToken);
        }
    }
}