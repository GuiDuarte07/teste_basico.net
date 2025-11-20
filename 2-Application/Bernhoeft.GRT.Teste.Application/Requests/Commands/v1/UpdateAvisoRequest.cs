#nullable enable

using Bernhoeft.GRT.Core.Interfaces.Results;
using Bernhoeft.GRT.Teste.Application.Responses.Commands.v1;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bernhoeft.GRT.Teste.Application.Requests.Commands.v1
{
    public class UpdateAvisoRequest : IRequest<IOperationResult<UpdateAvisoResponse>>
    {
        public int Id { get; set; }
        public string? Mensagem { get; set; }
        public bool? Ativo { get; set; }
    }
}
#nullable restore
