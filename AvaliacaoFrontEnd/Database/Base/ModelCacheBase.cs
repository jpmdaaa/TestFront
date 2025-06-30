using System.ComponentModel.DataAnnotations.Schema;

namespace AvaliacaoFrontEnd.Database.Base
{
    public class ModelCacheBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime DataInclusao { get; set; }
        public DateTime DataAlteracao { get; set; }
    }
}
