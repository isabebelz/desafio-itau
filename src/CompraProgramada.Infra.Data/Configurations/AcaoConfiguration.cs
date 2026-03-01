using CompraProgramada.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompraProgramada.Infra.Data.Configurations
{
    public class AcaoConfiguration : IEntityTypeConfiguration<Acao>
    {
        public void Configure(EntityTypeBuilder<Acao> builder)
        {
            builder.ToTable("T_ACAO");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                .HasColumnName("ID");

            builder.Property(a => a.Codigo)
                .IsRequired()
                .HasMaxLength(10)
                .HasColumnName("CODIGO");

            builder.Property(a => a.NomeEmpresa)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("NOME_EMPRESA");

            builder.Property(a => a.Preco)
                .IsRequired()
                .HasPrecision(18, 6)
                .HasColumnName("PRECO");

            builder.Property(a => a.Ativo)
                .IsRequired()
                .HasColumnName("ATIVO");

            builder.Property(a => a.DataCriacao)
                .IsRequired()
                .HasColumnName("DATA_CRIACAO");

            builder.Property(a => a.DataAtualizacao)
                .HasColumnName("DATA_ATUALIZACAO");
        }
    }
}
