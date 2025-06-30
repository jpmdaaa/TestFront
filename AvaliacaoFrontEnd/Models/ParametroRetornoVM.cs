using System.Net;

namespace AvaliacaoFrontEnd.Models
{
    public class ParametroRetornoVM
    {
        public string? Mensagem { get; set; }
        public bool Sucesso { get; set; }

        public HttpStatusCode? StatusCode { get; set; }
    }
}
