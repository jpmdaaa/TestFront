using AvaliacaoFrontEnd.Database.Configurations;
using AvaliacaoFrontEnd.Database.Entidades;
using Microsoft.EntityFrameworkCore;

namespace AvaliacaoFrontEnd.Database
{
    public class CacheContext : DbContext
    {
        public CacheContext(DbContextOptions<CacheContext> options) : base(options) { }

        public DbSet<Produto> Produtos { get; set; }

        #region Overrides

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ProdutoConfiguration());
        }

        #endregion
    }
}
