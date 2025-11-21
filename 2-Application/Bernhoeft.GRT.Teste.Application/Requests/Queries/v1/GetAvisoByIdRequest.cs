using Bernhoeft.GRT.Core.Interfaces.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Bernhoeft.GRT.Teste.Application.Responses.Queries.v1;

namespace Bernhoeft.GRT.Teste.Application.Requests.Queries.v1
{
    public class GetAvisoByIdRequest : IRequest<IOperationResult<GetAvisoByIdResponse>>
    {
        [FromRoute(Name = "id")]
        public int Id { get; set; }
    }

}
