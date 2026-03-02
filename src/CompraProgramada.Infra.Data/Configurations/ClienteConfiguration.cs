using CompraProgramada.Domain.Entities.ClienteAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompraProgramada.Infra.Data.Configurations
{
    public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("T_CLIENTE");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("ID");

            builder.Property(c => c.Nome)
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnName("NOME");

            builder.Property(c => c.CPF)
                .IsRequired()
                .HasMaxLength(11)
                .HasColumnName("CPF");

            builder.HasIndex(c => c.CPF)
                .IsUnique();

            builder.Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("EMAIL");

            builder.Property(c => c.ValorAporteMensal)
                .IsRequired()
                .HasPrecision(18, 2)
                .HasColumnName("VALOR_APORTE_MENSAL");

            builder.Property(c => c.Ativo)
                .IsRequired()
                .HasColumnName("ATIVO");

            builder.Property(c => c.DataAdesao)
                .IsRequired()
                .HasColumnName("DATA_ADESAO");

            builder.Property(c => c.DataSaida)
                .HasColumnName("DATA_SAIDA");

            builder.Property(c => c.DataCriacao)
                .IsRequired()
                .HasColumnName("DATA_CRIACAO");

            builder.Property(c => c.DataAtualizacao)
                .HasColumnName("DATA_ATUALIZACAO");

            builder.HasOne(c => c.ContaGrafica)
                .WithOne(cg => cg.Cliente)
                .HasForeignKey<ContaGrafica>(cg => cg.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
