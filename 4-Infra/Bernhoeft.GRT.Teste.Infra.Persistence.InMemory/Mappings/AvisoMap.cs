using Bernhoeft.GRT.ContractWeb.Domain.SqlServer.ContractStore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Bernhoeft.GRT.ContractWeb.Infra.Persistence.SqlServer.ContractStore.Mappings
{
    public partial class AvisoMap : IEntityTypeConfiguration<AvisoEntity>
    {
        public void Configure(EntityTypeBuilder<AvisoEntity> builder)
        {
            builder.ToTable("Aviso", "dbo");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName(@"id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.Ativo).HasColumnName(@"ativo").HasColumnType("bit").IsRequired();
            builder.Property(x => x.Titulo).HasColumnName(@"titulo").HasColumnType("varchar(50)").IsRequired().IsUnicode(false).HasMaxLength(50);
            builder.Property(x => x.Mensagem).HasColumnName(@"mensagem").HasColumnType("text(2147483647)").IsRequired().IsUnicode(false).HasMaxLength(2147483647);

            builder.Property(x => x.CriadoEm)
                .HasColumnName(@"criado_em")
                .HasColumnType("datetime")
                .IsRequired()
                .ValueGeneratedOnAdd() // Gera valor na criação
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore); // Impede updates


            builder.Property(x => x.AtualizadoEm).HasColumnName(@"atualizado_em").HasColumnType("datetime").IsRequired(false);

            // Adicionando filtro global na coluna Ativo.
            builder.HasQueryFilter(x => x.Ativo);

            InitializePartial(builder);
        }

        partial void InitializePartial(EntityTypeBuilder<AvisoEntity> builder);
    }

}