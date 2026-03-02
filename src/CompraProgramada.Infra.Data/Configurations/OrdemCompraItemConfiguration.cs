using CompraProgramada.Domain.Entities.OrdemCompraAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompraProgramada.Infra.Data.Configurations
{
    public class OrdemCompraItemConfiguration : IEntityTypeConfiguration<OrdemCompraItem>
    {
        public void Configure(EntityTypeBuilder<OrdemCompraItem> builder)
        {
            builder.ToTable("T_ORDEM_COMPRA_ITEM");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Id)
                .HasColumnName("ID");

            builder.Property(i => i.OrdemCompraId)
                .IsRequired()
                .HasColumnName("ORDEM_COMPRA_ID");

            builder.Property(i => i.AcaoId)
                .IsRequired()
                .HasColumnName("ACAO_ID");

            builder.Property(i => i.Quantidade)
                .IsRequired()
                .HasColumnName("QUANTIDADE");

            builder.Property(i => i.PrecoUnitario)
                .IsRequired()
                .HasPrecision(18, 6)
                .HasColumnName("PRECO_UNITARIO");

            builder.Property(i => i.ValorTotal)
                .IsRequired()
                .HasPrecision(18, 2)
                .HasColumnName("VALOR_TOTAL");

            builder.Property(i => i.TipoMercado)
                .IsRequired()
                .HasConversion<int>()
                .HasColumnName("TIPO_MERCADO");

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
