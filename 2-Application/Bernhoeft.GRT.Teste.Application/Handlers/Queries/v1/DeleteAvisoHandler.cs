using Bernhoeft.GRT.ContractWeb.Domain.SqlServer.ContractStore.Entities;
using Bernhoeft.GRT.ContractWeb.Domain.SqlServer.ContractStore.Interfaces.Repositories;
using Bernhoeft.GRT.Core.EntityFramework.Domain.Interfaces;
using Bernhoeft.GRT.Core.EntityFramework.Infra;
using Bernhoeft.GRT.Core.Enums;
using Bernhoeft.GRT.Core.Extensions;
using Bernhoeft.GRT.Core.Interfaces.Results;
using Bernhoeft.GRT.Core.Models;
using Bernhoeft.GRT.Teste.Application.Requests.Commands.v1;
using Bernhoeft.GRT.Teste.Application.Requests.Queries.v1;
using Bernhoeft.GRT.Teste.Application.Responses.Commands.v1;
using MediatR;

namespace Bernhoeft.GRT.Teste.Application.Handlers.Queries.v1
{
    public class DeleteAvisoHandler : IRequestHandler<DeleteAvisoRequest, IOperationResult<DeleteAvisoResponse>>
    {
        private readonly IAvisoRepository _avisoRepository;
        public DeleteAvisoHandler(IAvisoRepository avisoRepository)
        {
            _avisoRepository = avisoRepository;
        }

        public async Task<IOperationResult<DeleteAvisoResponse>> Handle(DeleteAvisoRequest request, CancellationToken cancellationToken)
        {
            var aviso = await _avisoRepository.GetByIdAsync(request.Id, cancellationToken);

            if (aviso is null)
                return OperationResult<DeleteAvisoResponse>.ReturnNotFound().AddMessage("Entidade a ser deletada não encontrada");


            await _avisoRepository.SoftDeleteAvisoAsync(aviso, cancellationToken);

            return OperationResult<DeleteAvisoResponse>.ReturnNoContent();
        }
    }
}
