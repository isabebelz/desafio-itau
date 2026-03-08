using CompraProgramada.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompraProgramada.Infra.Data.Configurations
{
    public class CotacaoConfiguration : IEntityTypeConfiguration<Cotacao>
    {
        public void Configure(EntityTypeBuilder<Cotacao> builder)
        {
            builder.ToTable("T_COTACAO");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("ID");

            builder.Property(c => c.Codigo)
                .IsRequired()
                .HasMaxLength(12) 
                .HasColumnName("TICKER");

            builder.Property(c => c.PrecoFechamento)
                .IsRequired()
                .HasPrecision(18, 2)
                .HasColumnName("PRECO_FECHAMENTO");

            builder.Property(c => c.PrecoAbertura)
                .HasPrecision(18, 2)
                .HasColumnName("PRECO_ABERTURA");

            builder.Property(c => c.PrecoMaximo)
                .HasPrecision(18, 2)
                .HasColumnName("PRECO_MAXIMO");

            builder.Property(c => c.PrecoMinimo)
                .HasPrecision(18, 2)
                .HasColumnName("PRECO_MINIMO");

            builder.Property(c => c.DataPregao)
                .IsRequired()
                .HasColumnName("DATA_PREGAO");

            builder.Property(c => c.CodigoBDI)
                .HasMaxLength(2)
                .HasColumnName("CODIGO_BDI");

            builder.Property(c => c.TipoMercado)
                .IsRequired()
                .HasColumnName("TIPO_MERCADO");

            builder.Property(c => c.DataCriacao)
                .IsRequired()
                .HasColumnName("DATA_CRIACAO");

            builder.Property(c => c.DataAtualizacao)
                .HasColumnName("DATA_ATUALIZACAO");

            builder.HasIndex(c => new { c.Codigo, c.DataPregao })
                .IsUnique()
                .HasDatabaseName("IX_COTACAO_TICKER_DATA");
        }
    }
}