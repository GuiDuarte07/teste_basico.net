using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bernhoeft.GRT.ContractWeb.Domain.SqlServer.ContractStore.Entities;

namespace Bernhoeft.GRT.Teste.Application.Responses.Commands.v1
{
    public class UpdateAvisoResponse
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Mensagem { get; set; }
        public bool Ativo { get; set; }

        public static implicit operator UpdateAvisoResponse(AvisoEntity entity)
            => new()
            {
                Id = entity.Id,
                Titulo = entity.Titulo,
                Mensagem = entity.Mensagem,
                Ativo = entity.Ativo
            };
    }
}
