using AvaliacaoFrontEnd.Database.Base;

namespace AvaliacaoFrontEnd.Database.Entidades
{
    public class Produto : ModelCacheBase
    {
        public string? Nome { get; set; }
        public decimal Valor { get; set; }
    }
}
