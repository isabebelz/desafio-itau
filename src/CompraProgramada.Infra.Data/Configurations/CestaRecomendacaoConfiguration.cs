using CompraProgramada.Domain.Entities.CestaAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompraProgramada.Infra.Data.Configurations
{
    public class CestaRecomendacaoConfiguration : IEntityTypeConfiguration<CestaRecomendacao>
    {
        public void Configure(EntityTypeBuilder<CestaRecomendacao> builder)
        {
            builder.ToTable("T_CESTA_RECOMENDACAO");

            builder.HasKey(cr => cr.Id);

            builder.Property(cr => cr.Id)
                .HasColumnName("ID");

            builder.Property(cr => cr.Ativa)
                .IsRequired()
                .HasColumnName("ATIVA");

            builder.Property(cr => cr.DataVigencia)
                .IsRequired()
                .HasColumnName("DATA_VIGENCIA");

            builder.Property(cr => cr.DataCriacao)
                .IsRequired()
                .HasColumnName("DATA_CRIACAO");

            builder.Property(cr => cr.DataAtualizacao)
                .HasColumnName("DATA_ATUALIZACAO");

            builder.HasMany(cr => cr.Itens)
                .WithOne(i => i.CestaRecomendacao)
                .HasForeignKey(i => i.CestaRecomendacaoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
