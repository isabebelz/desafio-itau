using CompraProgramada.Domain.Entities.CestaAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompraProgramada.Infra.Data.Configurations
{
    public class CestaRecomendacaoItemConfiguration : IEntityTypeConfiguration<CestaRecomendacaoItem>
    {
        public void Configure(EntityTypeBuilder<CestaRecomendacaoItem> builder)
        {
            builder.ToTable("T_CESTA_RECOMENDACAO_ITEM");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Id)
                .HasColumnName("ID");

            builder.Property(i => i.CestaRecomendacaoId)
                .IsRequired()
                .HasColumnName("CESTA_RECOMENDACAO_ID");

            builder.Property(i => i.AcaoId)
                .IsRequired()
                .HasColumnName("ACAO_ID");

            builder.Property(i => i.Percentual)
                .IsRequired()
                .HasPrecision(5, 2)
                .HasColumnName("PERCENTUAL");

            builder.Property(i => i.DataCriacao)
                .IsRequired()
                .HasColumnName("DATA_CRIACAO");

            builder.Property(i => i.DataAtualizacao)
                .HasColumnName("DATA_ATUALIZACAO");

            builder.HasOne(i => i.Acao)
                .WithMany()
                .HasForeignKey(i => i.AcaoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
