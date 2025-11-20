using Bernhoeft.GRT.ContractWeb.Domain.SqlServer.ContractStore.Interfaces.Repositories;
using Bernhoeft.GRT.Core.Interfaces.Results;
using Bernhoeft.GRT.Core.Models;
using Bernhoeft.GRT.Teste.Application.Requests.Commands.v1;
using Bernhoeft.GRT.Teste.Application.Responses.Commands.v1;
using MediatR;

namespace Bernhoeft.GRT.Teste.Application.Handlers.Queries.v1
{
    public class UpdateAvisoHandler : IRequestHandler<UpdateAvisoRequest, IOperationResult<UpdateAvisoResponse>>
    {
        private readonly IAvisoRepository _repository;

        public UpdateAvisoHandler(IAvisoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IOperationResult<UpdateAvisoResponse>> Handle(UpdateAvisoRequest request, CancellationToken cancellationToken)
        {
            var aviso = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (aviso is null)
                return OperationResult<UpdateAvisoResponse>.ReturnNotFound().AddMessage("Entidade a ser atualizada não encontrada");

            if (!string.IsNullOrEmpty(request.Mensagem))
                aviso.Mensagem = request.Mensagem;

            if (request.Ativo is not null)
                aviso.Ativo = (bool)request.Ativo;

            var entity = await _repository.UpdateAvisoAsync(aviso, cancellationToken);

            return OperationResult<UpdateAvisoResponse>.ReturnOk((UpdateAvisoResponse)entity);
        }
    }

}
