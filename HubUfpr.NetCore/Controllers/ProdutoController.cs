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
        public readonly IUserService _userService;

        public ProdutoController(IProdutoService produtoService, IUserService userService)
        {
            _produtoService = produtoService;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("cadastro")]
        public JsonResult InsertProduct([FromForm] InsertProduto request)
        {
            try
            {       
                if (
                    request.nome == null || request.nome.Trim() == "" ||
                    request.idVendedor <= 0 || 
                    request.preco <= 0 ||
                    request.descricao == null || request.descricao.Trim() == "" ||
                    request.quantidadeDisponivel < 0
                )
                {
                    Response.StatusCode = 400;
                    return Json(new
                    {
                        msg =
                            "Por favor, informe um nome de produto, código do vendedor, preço, descrição e quantidade disponível."
                    });
                }
                else if (!_userService.IsValidVendedor(request.idVendedor))
                {
                    Response.StatusCode = 400;
                    return Json(new 
                    {
                        msg = "O Vendedor informado não existe!"
                    });
                }
                else
                {
                    string image = null;
                    if (request.ProductImage != null)
                    {
                        var fs = request.ProductImage.OpenReadStream();
                        var bs = new System.IO.BinaryReader(fs);
                        Byte[] bytes = bs.ReadBytes((Int32)fs.Length);
                        image = Convert.ToBase64String(bytes, 0, bytes.Length);
                    }

                    _produtoService
                        .InsertProduto(
                            request.nome,
                            request.isAtivo,
                            request.preco,
                            request.descricao,
                            request.quantidadeDisponivel,
                            request.idVendedor,
                            image);


                    return Json(new { msg = "Produto inserido com sucesso!" });
                }
            }
            catch (System.Exception ex)
            {
                throw new InvalidOperationException("Erro ao salvar produto", ex);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("buscarTodos")]
        public JsonResult GetAllProducts()
        {
            try
            {
                return Json(new { produtos = _produtoService.GetAllProducts() });
            }
            catch (Exception ex)
            {
                return Json(new { msg = "Houve um erro ao atualizar a nota: " + ex });
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("buscarPorId/{id}")]
        public JsonResult SearchProductById(int id)
        {
            try
            {
                if (id > 0)
                {
                    var produto =
                        _produtoService
                            .SearchProductById(id);

                    if (produto == null)
                    {
                        Response.StatusCode = 404;
                        return Json(new { msg = "Nenhum produto encontrado." });
                    }
                    
                    return Json(new { produto });
                }
                else
                {
                    Response.StatusCode = 400;
                    return Json(new {
                        msg =
                            "Por favor informe um id de produto válido."
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
        [Route("buscarPorNome")]
        public JsonResult SearchProductByName([FromBody] SearchProductByName req)
        {
            try
            {
                if (req.Name == null || req.Name.Trim().Length == 0)
                {
                    Response.StatusCode = 400;
                    return Json(new { msg = "Você deve informar o nome do Produto!" });
                }

                var produtos = _produtoService
                    .SearchProductByName(req.Name.Trim(), req.ReturnActiveOnly);

                if (produtos.Count == 0)
                {
                    Response.StatusCode = 404;
                    return Json(new { msg = "Nenhum produto encontrado." });
                }

                    return Json(new { produtos });
            }
            catch (System.Exception ex)
            {
                throw new InvalidOperationException("Erro ao buscar produto",
                    ex);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("buscarPorVendedor")]
        public JsonResult SearchProductBySeller([FromBody] SearchProductBySeller req)
        {
            try
            {
                if (req.SellerId <= 0)
                {
                    Response.StatusCode = 400;
                    return Json(new { msg = "Você deve informar o id do Vendedor do Produto!" });
                }

                var produtos = _produtoService
                    .SearchProductBySeller(req.SellerId, req.ReturnActiveOnly);

                if (produtos.Count == 0)
                {
                    Response.StatusCode = 404;
                    return Json(new { msg = "Nenhum produto encontrado." });
                }

                return Json(new { produtos });
            }
            catch (System.Exception ex)
            {
                throw new InvalidOperationException("Erro ao buscar produto",
                    ex);
            }
        }

        [AllowAnonymous]
        [HttpDelete]
        [Route("deletar/{idProduto}")]
        public JsonResult DeleteProduto(int idProduto)
        {
            try
            {
                if (idProduto > 0)
                {
                    int ret = _produtoService.DeleteProduto (idProduto);

                    if (ret > 0)
                        return Json(new { msg = "Produto deletado com sucesso." });
                    else
                        return Json(new { msg = "Produto não encontrado." });
                }
                else
                {
                    Response.StatusCode = 400;
                    return Json(new { msg = "Por favor, informe um ID de Produto válido." });
                }
            }
            catch (System.Exception ex)
            {
                throw new InvalidOperationException("Erro ao buscar produto",
                    ex);
            }
        }

        [AllowAnonymous]
        [HttpPut]
        [Route("update/{idProduto}")]
        public JsonResult UpdateProduto([FromForm] UpdateProduto request, int idProduto)
        {
            try
            {
                if (idProduto > 0)
                {
                    if (request.nome.Trim() == "" || request.nome == null || request.preco <= 0 || 
                        request.descricao.Trim() == "" || request.descricao == null || request.quantidadeDisponivel < 0)
                    {
                        Response.StatusCode = 400;
                        return Json(new { msg = "Você deve informar um nome, status, preço, descrição e quantidade disponível!" });
                    }

                    string image = null;
                    if (request.ProductImage != null)
                    {
                        var fs = request.ProductImage.OpenReadStream();
                        var bs = new System.IO.BinaryReader(fs);
                        Byte[] bytes = bs.ReadBytes((Int32)fs.Length);
                        image = Convert.ToBase64String(bytes, 0, bytes.Length);
                    }

                    int ret = _produtoService
                        .UpdateProduto(idProduto,
                        request.nome,
                        request.isAtivo,
                        request.preco,
                        request.descricao,
                        request.quantidadeDisponivel,
                        image
                     );

                    Response.StatusCode = 200;
                    if (ret > 0)
                        return Json(new { msg = "Produto alterado com sucesso." });
                    else
                        return Json(new { msg = "Produto não encontrado." });
                }
                else
                {
                    Response.StatusCode = 400;
                    return Json(new { msg = "Por favor, informe o ID do Produto" });
                }
            }
            catch (System.Exception ex)
            {
                throw new InvalidOperationException("Erro ao buscar produto",
                    ex);
            }
        }

        [AllowAnonymous]
        [HttpPatch]
        [Route("atualizaNota/{productId}")]
        public JsonResult UpdateScore([FromBody] UpdateScore request, int productId)
        {
            try
            {
                if (request.Score <= 0)
                {
                    Response.StatusCode = 400;
                    return Json(new { msg = "Você deve informar uma nota válida." });
                }

                if (productId <= 0)
                {
                    Response.StatusCode = 400;
                    return Json(new { msg = "Você deve informar ID de produto válido." });
                }

                int ret = _produtoService.UpdateScore(productId, request.Score);
                
                if (ret > 0)
                {
                    Response.StatusCode = 200;
                    return Json(new { msg = "Nota atualizada com sucesso." });
                }
                else {
                    Response.StatusCode = 200;
                    return Json(new { msg = "Produto não encontrado." });
                }
                
            }
            catch(Exception ex)
            {
                return Json(new { msg = "Houve um erro ao atualizar a nota: " + ex });
            }
        }
    }
}
