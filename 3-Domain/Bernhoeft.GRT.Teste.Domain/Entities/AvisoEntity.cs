namespace Bernhoeft.GRT.ContractWeb.Domain.SqlServer.ContractStore.Entities
{
    public partial class AvisoEntity
    {
        public int Id { get; private set; }
        public bool Ativo { get; set; } = true;
        public string Titulo { get; set; }
        public string Mensagem { get; set; }

        public DateTime CriadoEm { get; private set; } = DateTime.Now;
        public DateTime? AtualizadoEm { get; private set; }

        public static AvisoEntity CreateForTests(int id, string titulo, string mensagem)
        {
            return new AvisoEntity
            {
                Id = id,
                Titulo = titulo,
                Mensagem = mensagem,
                Ativo = true
            };
        }

        public void SetUpdatedNow()
        {
            AtualizadoEm = DateTime.Now;
        }

    }
}