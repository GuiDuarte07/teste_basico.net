using Bernhoeft.GRT.ContractWeb.Domain.SqlServer.ContractStore.Interfaces.Repositories;
using Bernhoeft.GRT.Core.EntityFramework.Domain.Interfaces;
using Bernhoeft.GRT.Core.Enums;
using Bernhoeft.GRT.Core.Extensions;
using Bernhoeft.GRT.Core.Interfaces.Results;
using Bernhoeft.GRT.Core.Models;
using Bernhoeft.GRT.Teste.Application.Requests.Queries.v1;
using Bernhoeft.GRT.Teste.Application.Responses.Queries.v1;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Bernhoeft.GRT.Teste.Application.Handlers.Queries.v1
{
    
    public class GetAvisoByIdHandler
    : IRequestHandler<GetAvisoByIdRequest, IOperationResult<GetAvisoByIdResponse>>
    {
        private readonly IAvisoRepository _avisoRepository;

        public GetAvisoByIdHandler(IAvisoRepository avisoRepository)
        {
            _avisoRepository = avisoRepository;
        }

        public async Task<IOperationResult<GetAvisoByIdResponse>> Handle(
            GetAvisoByIdRequest request,
            CancellationToken cancellationToken)
        {
            var aviso = await _avisoRepository.ObterAvisoByIdAsync(
                request.Id,
                TrackingBehavior.NoTracking);

            if (aviso == null)
                return OperationResult<GetAvisoByIdResponse>.ReturnNotFound();

            return OperationResult<GetAvisoByIdResponse>.ReturnOk((GetAvisoByIdResponse)aviso);
        }
    }
}