using CompraProgramada.Domain.Entities.OrdemCompraAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompraProgramada.Infra.Data.Configurations
{
    public class OrdemCompraConfiguration : IEntityTypeConfiguration<OrdemCompra>
    {
        public void Configure(EntityTypeBuilder<OrdemCompra> builder)
        {
            builder.ToTable("T_ORDEM_COMPRA");

            builder.HasKey(oc => oc.Id);

            builder.Property(oc => oc.Id)
                .HasColumnName("ID");

            builder.Property(oc => oc.DataExecucao)
                .IsRequired()
                .HasColumnName("DATA_EXECUCAO");

            builder.Property(oc => oc.ValorTotal)
                .IsRequired()
                .HasPrecision(18, 2)
                .HasColumnName("VALOR_TOTAL");

            builder.Property(oc => oc.Status)
                .IsRequired()
                .HasConversion<int>()
                .HasColumnName("STATUS");

            builder.Property(oc => oc.DataCriacao)
                .IsRequired()
                .HasColumnName("DATA_CRIACAO");

            builder.Property(oc => oc.DataAtualizacao)
                .HasColumnName("DATA_ATUALIZACAO");

            builder.HasMany(oc => oc.Itens)
                .WithOne(i => i.OrdemCompra)
                .HasForeignKey(i => i.OrdemCompraId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
