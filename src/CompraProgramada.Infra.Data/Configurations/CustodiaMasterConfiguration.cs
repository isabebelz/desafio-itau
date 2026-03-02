using CompraProgramada.Domain.Entities.ContaMasterAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompraProgramada.Infra.Data.Configurations
{
    public class CustodiaMasterConfiguration : IEntityTypeConfiguration<CustodiaMaster>
    {
        public void Configure(EntityTypeBuilder<CustodiaMaster> builder)
        {
            builder.ToTable("T_CUSTODIA_MASTER");

            builder.HasKey(cm => cm.Id);

            builder.Property(cm => cm.Id)
                .HasColumnName("ID");

            builder.Property(cm => cm.ContaMasterId)
                .IsRequired()
                .HasColumnName("CONTA_MASTER_ID");

            builder.Property(cm => cm.AcaoId)
                .IsRequired()
                .HasColumnName("ACAO_ID");

            builder.Property(cm => cm.Quantidade)
                .IsRequired()
                .HasColumnName("QUANTIDADE");

            builder.Property(cm => cm.PrecoMedio)
                .IsRequired()
                .HasPrecision(18, 6)
                .HasColumnName("PRECO_MEDIO");

            builder.Property(cm => cm.DataCriacao)
                .IsRequired()
                .HasColumnName("DATA_CRIACAO");

            builder.Property(cm => cm.DataAtualizacao)
                .HasColumnName("DATA_ATUALIZACAO");

            builder.HasIndex(cm => new { cm.ContaMasterId, cm.AcaoId })
                .IsUnique();

            builder.HasOne(cm => cm.ContaMaster)
                .WithMany(c => c.Custodias)
                .HasForeignKey(cm => cm.ContaMasterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(cm => cm.Acao)
                .WithMany()
                .HasForeignKey(cm => cm.AcaoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
