using CompraProgramada.Domain.Entities.ContaMasterAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompraProgramada.Infra.Data.Configurations
{
    public class ContaMasterConfiguration : IEntityTypeConfiguration<ContaMaster>
    {
        public void Configure(EntityTypeBuilder<ContaMaster> builder)
        {
            builder.ToTable("T_CONTA_MASTER");

            builder.HasKey(cm => cm.Id);

            builder.Property(cm => cm.Id)
                .HasColumnName("ID");

            builder.Property(cm => cm.Descricao)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("DESCRICAO");

            builder.Property(cm => cm.DataCriacao)
                .IsRequired()
                .HasColumnName("DATA_CRIACAO");

            builder.Property(cm => cm.DataAtualizacao)
                .HasColumnName("DATA_ATUALIZACAO");
        }
    }
}
