using System.Net;
using AvaliacaoFrontEnd.Models;
using AvaliacaoFrontEnd.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;

namespace AvaliacaoFrontEnd.Controllers.Base
{
    public class BaseController : ControllerBase
    {
        #region Constantes

        protected const string cRoleAdmin = "Admin";
        protected const string cRoleUser = "User";

        protected const string cUserAdm = "Admin";
        protected const string cUserAdmSenha = "125Adm";

        protected const string cUserNormal = "User";
        protected const string cUserNormalSenha = "125User";

        #endregion

        #region Campos privados

        private readonly IConfiguration _configuration;

        #endregion

        #region Construtor

        public BaseController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #endregion

        protected ObjectResult Result(ParametroRetornoVM dados)
        {
            if (dados.StatusCode == HttpStatusCode.Forbidden)
            {
                dados.Mensagem = "Você não possui permissão para acessar este recurso.";
                return StatusCode((int)dados.StatusCode, dados);
            }

            if (!dados.Mensagem.TrimDTC().EndsWith(".") && !dados.Mensagem.TrimDTC().EndsWith(">"))
            {
                dados.Mensagem = $"{dados.Mensagem}.";
            }

            if (dados.StatusCode != null)
            {
                return StatusCode((int)dados.StatusCode, dados);
            }
            return NotFound(dados);
        }

        protected JWTRetornoVM GerarToken(string usuario, int validadeToken, string refreshToken, List<string>? roles = null)
        {
            var service = new AutenticacaoService();

            var audience = _configuration["JWT:ValidAudience"]!;
            var issuer = _configuration["JWT:ValidIssuer"]!;
            var secret = _configuration["JWT:Secret"]!;
            return service.JwtEncode(usuario, issuer, audience, secret, validadeToken, refreshToken, roles, JwtBearerDefaults.AuthenticationScheme);
        }
    }
}

namespace System
{
    public static class StringExtensions
    {
        public static string TrimDTC(this string? value)
        {
            if (value == null)
            {
                return string.Empty;
            }
            return value.Trim();
        }
    }
}
