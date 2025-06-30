using System.Net;
using System.Net.Mime;
using Asp.Versioning;
using AvaliacaoFrontEnd.Controllers.Base;
using AvaliacaoFrontEnd.Database;
using AvaliacaoFrontEnd.Database.Entidades;
using AvaliacaoFrontEnd.Models;
using AvaliacaoFrontEnd.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AvaliacaoFrontEnd.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/produtos")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ApiController]
    public class ProdutosController : BaseController
    {
        #region Campos privados

        private readonly ProdutoService _acao;

        #endregion

        #region Construtor

        public ProdutosController(IConfiguration configuration, CacheContext context) : base(configuration)
        {
            _acao = new ProdutoService(context);
        }

        #endregion

        #region Produtos

        [HttpGet]
        [ProducesResponseType(typeof(List<Produto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ParametroRetornoVM), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ParametroRetornoVM), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public IActionResult RetornarTodos()
        {
            try
            {
                return Ok(_acao.Produtos());
            }
            catch (Exception ex)
            {
                return Problem();
            }
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(Produto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ParametroRetornoVM), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ParametroRetornoVM), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public IActionResult RetornarItem([FromRoute] int id)
        {
            try
            {
                if (id <= 0)
                {

                    return Result(new ParametroRetornoVM
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Mensagem = "ID não informado"
                    });
                }

                var item = _acao.ProdutoItem(id);
                if (item != null)
                {
                    return Ok(item);
                }
                return Result(new ParametroRetornoVM
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Mensagem = "Produto não encontrado"
                });
            }
            catch (Exception ex)
            {
                return Problem();
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(Produto), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ParametroRetornoVM), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ParametroRetornoVM), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public IActionResult IncluirItem([FromBody] Produto dados)
        {
            try
            {
                if (dados.Nome == null || string.IsNullOrEmpty(dados.Nome))
                {
                    return Result(new ParametroRetornoVM
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Mensagem = "Nome não informado"
                    });
                }

                var item = _acao.AdicionarItem(dados);
                if (item != null)
                {
                    return Created("", item);
                }
                return Result(new ParametroRetornoVM
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Mensagem = "Produto não encontrado"
                });
            }
            catch (Exception ex)
            {
                return Problem();
            }
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ParametroRetornoVM), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ParametroRetornoVM), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public IActionResult ModificarItem([FromRoute] int id, [FromBody] Produto dados)
        {
            try
            {
                if (id <= 0)
                {
                    return Result(new ParametroRetornoVM
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Mensagem = "ID não informado"
                    });
                }

                if (dados.Nome == null || string.IsNullOrEmpty(dados.Nome))
                {
                    return Result(new ParametroRetornoVM
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Mensagem = "Nome não informado"
                    });
                }

                dados.Id = id;
                var item = _acao.ModificarItem(dados);
                if (item != null)
                {
                    return NoContent();
                }
                return Result(new ParametroRetornoVM
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Mensagem = "Produto não encontrado"
                });
            }
            catch (Exception ex)
            {
                return Problem();
            }
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ParametroRetornoVM), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ParametroRetornoVM), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public IActionResult ExcluirItem([FromRoute] int id)
        {
            try
            {
                if (id <= 0)
                {
                    return Result(new ParametroRetornoVM
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Mensagem = "ID não informado"
                    });
                }
                var item = _acao.ExcluirItem(id);
                if (item != null)
                {
                    return NoContent();
                }
                return Result(new ParametroRetornoVM
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Mensagem = "Produto não encontrado"
                });
            }
            catch (Exception ex)
            {
                return Problem();
            }
        }

        #endregion
    }
}
