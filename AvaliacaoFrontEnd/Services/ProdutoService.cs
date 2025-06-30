using AvaliacaoFrontEnd.Database;
using AvaliacaoFrontEnd.Database.Entidades;

namespace AvaliacaoFrontEnd.Services
{
    public class ProdutoService
    {
        #region Campos privados

        private readonly CacheContext _context;

        #endregion

        #region Construtor

        public ProdutoService(CacheContext context)
        {
            _context = context;
            InicializarProdutos();
        }

        #endregion

        public List<Produto> Produtos()
        {
            return _context.Produtos.ToList();
        }

        public Produto? ProdutoItem(int id)
        {
            return _context.Produtos.FirstOrDefault(p => p.Id == id);
        }

        public Produto? AdicionarItem(Produto dados)
        {
            dados.DataInclusao = DateTime.Now;
            dados.DataAlteracao = DateTime.Now;
            _context.Produtos.Add(dados);
            _context.SaveChanges();
            return dados;
        }

        public Produto? ModificarItem(Produto dados)
        {
            var item = _context.Produtos.First(p => p.Id == dados.Id);
            item.Nome = dados.Nome;
            item.Valor = dados.Valor;
            item.DataAlteracao = DateTime.Now;
            _context.Produtos.Add(item);
            _context.SaveChanges();
            return dados;
        }

        public Produto? ExcluirItem(int id)
        {
            var item = _context.Produtos.First(p => p.Id == id);
            _context.Produtos.Remove(item);
            _context.SaveChanges();
            return item;
        }

        #region Métodos privados

        private void InicializarProdutos()
        {
            var item = _context.Produtos.FirstOrDefault();
            if (item == null)
            {
                var valor = 1M;
                for (int i = 1; i < 11; i++)
                {
                    _context.Produtos.Add(new Database.Entidades.Produto
                    {
                        Nome = $"Nome {i}",
                        DataAlteracao = DateTime.Now,
                        DataInclusao = DateTime.Now,
                        Valor = valor * 2
                    });
                }
                _context.SaveChanges();
            }
        }

        #endregion
    }
}
