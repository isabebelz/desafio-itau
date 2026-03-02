using CompraProgramada.Domain.Entities.OrdemCompraAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompraProgramada.Infra.Data.Configurations
{
    public class DistribuicaoConfiguration : IEntityTypeConfiguration<Distribuicao>
    {
        public void Configure(EntityTypeBuilder<Distribuicao> builder)
        {
            builder.ToTable("T_DISTRIBUICAO");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Id)
                .HasColumnName("ID");

            builder.Property(d => d.OrdemCompraId)
                .IsRequired()
                .HasColumnName("ORDEM_COMPRA_ID");

            builder.Property(d => d.ContaGraficaId)
                .IsRequired()
                .HasColumnName("CONTA_GRAFICA_ID");

            builder.Property(d => d.AcaoId)
                .IsRequired()
                .HasColumnName("ACAO_ID");

            builder.Property(d => d.Quantidade)
                .IsRequired()
                .HasColumnName("QUANTIDADE");

            builder.Property(d => d.PrecoUnitario)
                .IsRequired()
                .HasPrecision(18, 6)
                .HasColumnName("PRECO_UNITARIO");

            builder.Property(d => d.ValorIRDedoDuro)
                .IsRequired()
                .HasPrecision(18, 6)
                .HasColumnName("VALOR_IR_DEDO_DURO");

            builder.Property(d => d.DataDistribuicao)
                .IsRequired()
                .HasColumnName("DATA_DISTRIBUICAO");

            builder.Property(d => d.DataCriacao)
                .IsRequired()
                .HasColumnName("DATA_CRIACAO");

            builder.Property(d => d.DataAtualizacao)
                .HasColumnName("DATA_ATUALIZACAO");

            builder.HasOne(d => d.OrdemCompra)
                .WithMany()
                .HasForeignKey(d => d.OrdemCompraId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.ContaGrafica)
                .WithMany()
                .HasForeignKey(d => d.ContaGraficaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Acao)
                .WithMany()
                .HasForeignKey(d => d.AcaoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
