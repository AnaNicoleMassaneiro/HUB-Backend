using System;
using HubUfpr.API.Requests;
using HubUfpr.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HubUfpr.API.Controllers
{
    [Produces("application/json")]
    [Route("api/produto")]
    public class ProdutoController : Controller
    {
        public readonly IProdutoService _produtoService;

        public ProdutoController(IProdutoService userService)
        {
            _produtoService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("cadastro")]
        public JsonResult insertProduct([FromBody] Produto request)
        {
            try
            {
                if (
                    request.nome != null &&
                    request.nome != "" &&
                    request.idVendedor != 0 &&
                    request.preco != 0 &&
                    request.descricao != null &&
                    request.quantidadeDisponivel != 0
                )
                {
                    _produtoService
                        .InsertProduto(request.nome,
                        request.status,
                        request.preco,
                        request.descricao,
                        request.quantidadeDisponivel,
                        request.idVendedor);

                    return Json("Produto inserido com sucesso :)");
                }
                else
                {
                    Response.StatusCode = 400;
                    return Json(new {
                        msg =
                            "Por favor, informe um nome de produto, vendedor ou id."
                    });
                }
            }
            catch (System.Exception ex)
            {
                throw new InvalidOperationException("Erro ao salvar produto",
                    ex);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("buscar")]
        public JsonResult searchProduto([FromBody] Produto request)
        {
            try
            {
                if (
                    request.nome != null ||
                    request.nome != "" ||
                    request.idProduto != 0 ||
                    request.idVendedor != 0
                )
                {
                    var retorno =
                        _produtoService
                            .SearchProduto(request.nome,
                            request.idProduto,
                            request.idVendedor);

                    return Json(retorno);
                }
                else
                {
                    Response.StatusCode = 400;
                    return Json(new {
                        msg =
                            "Por favor, informe um nome de produto, vendedor ou id."
                    });
                }
            }
            catch (System.Exception ex)
            {
                throw new InvalidOperationException("Erro ao buscar produto",
                    ex);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("deletar/{idProduto}")]
        public JsonResult deleteProduto(int idProduto)
        {
            try
            {
                if (idProduto != 0)
                {
                    _produtoService.DeleteProduto (idProduto);

                    return Json("sucesso ao deletar esse produto");
                }
                else
                {
                    Response.StatusCode = 400;
                    return Json(new {
                        msg =
                            "Por favor, informe um nome de produto, vendedor ou id."
                    });
                }
            }
            catch (System.Exception ex)
            {
                throw new InvalidOperationException("Erro ao buscar produto",
                    ex);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("update")]
        public JsonResult updateProduto([FromBody] Produto request)
        {
            try
            {
                if (request.idProduto != 0)
                {
                    _produtoService
                        .UpdateProduto(request.idProduto,
                        request.nome,
                        request.status,
                        request.preco,
                        request.descricao,
                        request.quantidadeDisponivel,
                        request.idVendedor);

                    Response.StatusCode = 200;
                    return Json("sucesso ao alterar esse produto");
                }
                else
                {
                    Response.StatusCode = 400;
                    return Json(new {
                        msg = "Por favor, informe o id do produto"
                    });
                }
            }
            catch (System.Exception ex)
            {
                throw new InvalidOperationException("Erro ao buscar produto",
                    ex);
            }
        }
    }
}
