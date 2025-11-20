using Bernhoeft.GRT.ContractWeb.Domain.SqlServer.ContractStore.Entities;

public class GetAvisoByIdResponse
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public string Mensagem { get; set; }
    public bool Ativo { get; set; }

    public static implicit operator GetAvisoByIdResponse(AvisoEntity entity)
        => new()
        {
            Id = entity.Id,
            Titulo = entity.Titulo,
            Mensagem = entity.Mensagem,
            Ativo = entity.Ativo
        };
}
