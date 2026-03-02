using CompraProgramada.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompraProgramada.Infra.Data.Configurations
{
    public class ParametroSistemaConfiguration : IEntityTypeConfiguration<ParametroSistema>
    {
        public void Configure(EntityTypeBuilder<ParametroSistema> builder)
        {
            builder.ToTable("T_PARAMETRO_SISTEMA");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("ID");

            builder.Property(p => p.Chave)
                .HasColumnName("CHAVE")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.Valor)
                .HasColumnName("VALOR")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.Descricao)
                .HasColumnName("DESCRICAO")
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(p => p.DataCriacao).HasColumnName("DATA_CRIACAO");
            builder.Property(p => p.DataAtualizacao).HasColumnName("DATA_ATUALIZACAO");

            builder.HasIndex(p => p.Chave).IsUnique();
        }
    }
}