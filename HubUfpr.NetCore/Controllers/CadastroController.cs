using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HubUfpr.API.Requests;
using HubUfpr.Service.Interface;
using System;

namespace HubUfpr.API.Controllers
{
    [Produces("application/json")]
    [Route("api/produto")]
    public class CadastroController : Controller
    {
        public readonly IProdutoService _produtoService;

        public CadastroController(IProdutoService userService)
        {
            _produtoService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("cadastro")]
        public JsonResult insertProduct([FromBody] Produto request)
        {
            // TODO validar campos obrigatorios
            try
            {
                _produtoService.InsertProduto(request.nome, request.status, request.preco, request.descricao, request.qtdProdutosDisponiveis);

                return Json("Produto inserido com sucesso :)");
            }
            catch (System.Exception ex)
            {
                throw new InvalidOperationException("Erro ao salvar produto", ex);
            }

        }

        [AllowAnonymous]
        [HttpPost]
        [Route("buscar")]
        public JsonResult searchProduto([FromBody] Produto request)
        {
            try
            {
                var retorno = _produtoService.SearchProduto(request.nome, request.idProduto);

                return Json(retorno);
            }
            catch (System.Exception ex)
            {
                throw new InvalidOperationException("Erro ao buscar produto", ex);
            }

        }
    }
}