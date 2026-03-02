using CompraProgramada.Domain.Entities.ClienteAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompraProgramada.Infra.Data.Configurations
{
    public class CustodiaFilhoteConfiguration : IEntityTypeConfiguration<CustodiaFilhote>
    {
        public void Configure(EntityTypeBuilder<CustodiaFilhote> builder)
        {
            builder.ToTable("T_CUSTODIA_FILHOTE");

            builder.HasKey(cf => cf.Id);

            builder.Property(cf => cf.Id)
                .HasColumnName("ID");

            builder.Property(cf => cf.ContaGraficaId)
                .IsRequired()
                .HasColumnName("CONTA_GRAFICA_ID");

            builder.Property(cf => cf.AcaoId)
                .IsRequired()
                .HasColumnName("ACAO_ID");

            builder.Property(cf => cf.Quantidade)
                .IsRequired()
                .HasColumnName("QUANTIDADE");

            builder.Property(cf => cf.PrecoMedio)
                .IsRequired()
                .HasPrecision(18, 6)
                .HasColumnName("PRECO_MEDIO");

            builder.Property(cf => cf.DataCriacao)
                .IsRequired()
                .HasColumnName("DATA_CRIACAO");

            builder.Property(cf => cf.DataAtualizacao)
                .HasColumnName("DATA_ATUALIZACAO");

            builder.HasIndex(cf => new { cf.ContaGraficaId, cf.AcaoId })
                .IsUnique();

            builder.HasOne(cf => cf.ContaGrafica)
                .WithMany(cg => cg.Custodias)
                .HasForeignKey(cf => cf.ContaGraficaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(cf => cf.Acao)
                .WithMany()
                .HasForeignKey(cf => cf.AcaoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}