using System.Net;
using System.Net.Mime;
using Asp.Versioning;
using AvaliacaoFrontEnd.Controllers.Base;
using AvaliacaoFrontEnd.Models;
using Microsoft.AspNetCore.Mvc;

namespace AvaliacaoFrontEnd.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/autenticacao")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ApiController]
    public class AutenticacaoController : BaseController
    {
        #region Construtor

        public AutenticacaoController(IConfiguration configuration) : base(configuration) { }

        #endregion

        [HttpPost]
        [Route("login")]
        [ProducesResponseType(typeof(JWTRetornoVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ParametroRetornoVM), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ParametroRetornoVM), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public IActionResult Login([FromBody] LoginDadosVM dados)
        {
            try
            {
                if (dados.Usuario == null || string.IsNullOrEmpty(dados.Usuario))
                {
                    return Result(new ParametroRetornoVM
                    {
                        Mensagem = "Usuário não informado",
                        StatusCode = HttpStatusCode.BadRequest,
                    });
                }

                if (dados.Senha == null || string.IsNullOrEmpty(dados.Senha))
                {
                    return Result(new ParametroRetornoVM
                    {
                        Mensagem = "Senha não informado",
                        StatusCode = HttpStatusCode.BadRequest,
                    });
                }

                if (dados.Usuario.Equals(cUserAdm, StringComparison.OrdinalIgnoreCase))
                {
                    if (dados.Senha == cUserAdmSenha)
                    {
                        return Ok(base.GerarToken(dados.Usuario, 1, "", new List<string> { cRoleAdmin }));
                    }

                    return Result(new ParametroRetornoVM
                    {
                        Mensagem = "Senha incorreta",
                        StatusCode = HttpStatusCode.BadRequest,
                    });
                }
                else if (dados.Usuario.Equals(cUserNormal, StringComparison.OrdinalIgnoreCase))
                {
                    if (dados.Senha == cUserNormalSenha)
                    {
                        return Ok(base.GerarToken(dados.Usuario, 1, "", new List<string> { cRoleUser }));
                    }

                    return Result(new ParametroRetornoVM
                    {
                        Mensagem = "Senha incorreta",
                        StatusCode = HttpStatusCode.BadRequest,
                    });
                }

                return Result(new ParametroRetornoVM
                {
                    Mensagem = "Dados de autenticação incorretos",
                    StatusCode = HttpStatusCode.BadRequest,
                });
            }
            catch (Exception ex)
            {
                return Problem();
            }
        }
    }
}
