using System.Text.Json.Serialization;

namespace AvaliacaoFrontEnd.Models
{
    public class LoginDadosVM
    {
        public string? Usuario { get; set; }
        public string? Senha { get; set; }
    }

    public class JWTRetornoVM
    {
        [JsonPropertyName("access_token")]
        public string? TokenAcesso { get; set; }

        [JsonPropertyName("token_type")]
        public string? TokenTipo { get; set; }

        [JsonPropertyName("expires_in")]
        public int TokenValidade { get; set; }

        [JsonPropertyName("refresh_token")]
        public string? RefreshToken { get; set; }
    }
}
