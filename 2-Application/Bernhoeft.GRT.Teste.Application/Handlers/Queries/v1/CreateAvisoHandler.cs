using Bernhoeft.GRT.ContractWeb.Domain.SqlServer.ContractStore.Interfaces.Repositories;
using Bernhoeft.GRT.Core.EntityFramework.Domain.Interfaces;
using Bernhoeft.GRT.Core.Enums;
using Bernhoeft.GRT.Core.Extensions;
using Bernhoeft.GRT.Core.Interfaces.Results;
using Bernhoeft.GRT.Core.Models;
using Bernhoeft.GRT.Teste.Application.Requests.Commands.v1;
using Bernhoeft.GRT.Teste.Application.Requests.Queries.v1;
using Bernhoeft.GRT.Teste.Application.Responses.Commands.v1;
using Bernhoeft.GRT.ContractWeb.Domain.SqlServer.ContractStore.Entities;
using MediatR;

namespace Bernhoeft.GRT.Teste.Application.Handlers.Queries.v1
{

    public class CreateAvisoHandler
    : IRequestHandler<CreateAvisoRequest, IOperationResult<CreateAvisoResponse>>
    {
        private readonly IAvisoRepository _avisoRepository;

        public CreateAvisoHandler(IAvisoRepository avisoRepository)
        {
            _avisoRepository = avisoRepository;
        }

        public async Task<IOperationResult<CreateAvisoResponse>> Handle(
            CreateAvisoRequest request,
            CancellationToken cancellationToken)
        {
            var aviso = new AvisoEntity
            {
                Mensagem = request.Mensagem,
                Titulo = request.Titulo,
            };

            var entity = await _avisoRepository.AddAvisoAsync(aviso, cancellationToken);

            if (entity == null)
                return OperationResult<CreateAvisoResponse>.ReturnNotFound();

            return OperationResult<CreateAvisoResponse>.ReturnOk((CreateAvisoResponse)entity);
        }
    }
}