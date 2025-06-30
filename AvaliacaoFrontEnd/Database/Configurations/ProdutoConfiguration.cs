using AvaliacaoFrontEnd.Database.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AvaliacaoFrontEnd.Database.Configurations
{
    public class ProdutoConfiguration : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("Id").HasColumnType("INTEGER").IsRequired();
            builder.Property(p => p.Nome).HasColumnName("Nome").HasColumnType("VARCHAR(80)").IsRequired();
            builder.Property(p => p.Valor).HasColumnName("Valor").HasColumnType("NUMERIC(10,2)").IsRequired();
            builder.Property(p => p.DataInclusao).HasColumnName("DataInclusao").HasColumnType("TIMESTAMP");
            builder.Property(p => p.DataAlteracao).HasColumnName("v").HasColumnType("TIMESTAMP");
            builder.ToTable("PersonCliente");
        }
    }
}
