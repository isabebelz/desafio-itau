using CompraProgramada.Domain.Entities.ClienteAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompraProgramada.Infra.Data.Configurations
{
    public class ContaGraficaConfiguration : IEntityTypeConfiguration<ContaGrafica>
    {
        public void Configure(EntityTypeBuilder<ContaGrafica> builder)
        {
            builder.ToTable("T_CONTA_GRAFICA");

            builder.HasKey(cg => cg.Id);

            builder.Property(cg => cg.Id)
                .HasColumnName("ID");

            builder.Property(cg => cg.ClienteId)
                .IsRequired()
                .HasColumnName("CLIENTE_ID");

            builder.HasIndex(cg => cg.ClienteId)
                .IsUnique();

            builder.Property(cg => cg.DataCriacao)
                .IsRequired()
                .HasColumnName("DATA_CRIACAO");

            builder.Property(cg => cg.DataAtualizacao)
                .HasColumnName("DATA_ATUALIZACAO");
        }
    }
}
