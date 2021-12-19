using System;
using HubUfpr.API.Requests;
using HubUfpr.Service.Class;
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
                
                if (!_userService.IsValidVendedor(request.idVendedor))
                {
                    Response.StatusCode = 400;
                    return Json(new 
                    {
                        msg = "O Vendedor informado não existe!"
                    });
                }

                if (Request.Headers["Authorization"].Count > 0 && Request.Headers["Authorization"].ToString().Trim().Length > 0)
                {
                    if (!TokenService.IsTokenValidMatchSellerId(Request.Headers["Authorization"], request.idVendedor))
                    {
                        Response.StatusCode = 401;
                        return Json(new { msg = "O token de acesso informado não é válido." });
                    }
                }
                else
                {
                    Response.StatusCode = 401;
                    return Json(new { msg = "Você deve informar seu token de acesso para acessar este conteúdo." });
                }

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
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { msg = "Erro ao criar Produto: " + ex });
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("buscarTodos")]
        public JsonResult GetAllProducts()
        {
            try
            {
                if (Request.Headers["Authorization"].Count > 0 && Request.Headers["Authorization"].ToString().Trim().Length > 0)
                {
                    if (!TokenService.IsTokenValid(Request.Headers["Authorization"]))
                    {
                        Response.StatusCode = 401;
                        return Json(new { msg = "O token de acesso informado não é válido." });
                    }
                }
                else
                {
                    Response.StatusCode = 401;
                    return Json(new { msg = "Você deve informar seu token de acesso para acessar este conteúdo." });
                }

                var produtos = _produtoService.GetAllProducts(int.Parse(TokenService.GetTokenProperty(Request.Headers["Authorization"], "SellerId")));

                if (produtos.Count > 0)
                    return Json(new { produtos });

                Response.StatusCode = 404;
                return Json(new { msg = "Nenhum produto foi encontrado." });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { msg = "Houve um erro ao buscar os Produtos: " + ex });
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
                    if (Request.Headers["Authorization"].Count > 0 && Request.Headers["Authorization"].ToString().Trim().Length > 0)
                    {
                        if (!TokenService.IsTokenValid(Request.Headers["Authorization"]))
                        {
                            Response.StatusCode = 401;
                            return Json(new { msg = "O token de acesso informado não é válido." });
                        }
                    }
                    else
                    {
                        Response.StatusCode = 401;
                        return Json(new { msg = "Você deve informar seu token de acesso para acessar este conteúdo." });
                    }

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
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { msg = "Houve um erro ao buscar o Produto: " + ex });
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

                if (Request.Headers["Authorization"].Count > 0 && Request.Headers["Authorization"].ToString().Trim().Length > 0)
                {
                    if (!TokenService.IsTokenValid(Request.Headers["Authorization"]))
                    {
                        Response.StatusCode = 401;
                        return Json(new { msg = "O token de acesso informado não é válido." });
                    }
                }
                else
                {
                    Response.StatusCode = 401;
                    return Json(new { msg = "Você deve informar seu token de acesso para acessar este conteúdo." });
                }

                var produtos = _produtoService
                    .SearchProductByName(
                        req.Name.Trim(), 
                        req.ReturnActiveOnly,
                        int.Parse(TokenService.GetTokenProperty(Request.Headers["Authorization"], "SellerId"))
                    );

                if (produtos.Count == 0)
                {
                    Response.StatusCode = 404;
                    return Json(new { msg = "Nenhum produto encontrado." });
                }

                    return Json(new { produtos });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { msg = "Houve um erro ao buscar os Produtos: " + ex });
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

                if (Request.Headers["Authorization"].Count > 0 && Request.Headers["Authorization"].ToString().Trim().Length > 0)
                {
                    if (!TokenService.IsTokenValid(Request.Headers["Authorization"]))
                    {
                        Response.StatusCode = 401;
                        return Json(new { msg = "O token de acesso informado não é válido." });
                    }
                }
                else
                {
                    Response.StatusCode = 401;
                    return Json(new { msg = "Você deve informar seu token de acesso para acessar este conteúdo." });
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
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { msg = "Houve um erro ao buscar os Produtos: " + ex });
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
                    if (Request.Headers["Authorization"].Count > 0 && Request.Headers["Authorization"].ToString().Trim().Length > 0)
                    {
                        if (!TokenService.IsTokenValidMatchSellerId(Request.Headers["Authorization"], _produtoService.GetSellerIdFromProduct(idProduto)))
                        {
                            Response.StatusCode = 401;
                            return Json(new { msg = "O token de acesso informado não é válido." });
                        }
                    }
                    else
                    {
                        Response.StatusCode = 401;
                        return Json(new { msg = "Você deve informar seu token de acesso para acessar este conteúdo." });
                    }

                    int ret = _produtoService.DeleteProduto(idProduto);

                    if (ret > 0)
                    {
                        return Json(new { msg = "Produto deletado com sucesso." });
                    }
                    else if (ret == -1)
                    {
                        Response.StatusCode = 409;
                        return Json(new { msg = "Produto não pode ser excluído." });
                    }
                    else
                    {
                        Response.StatusCode = 404;
                        return Json(new { msg = "Produto não encontrado." });
                    }
                }
                else
                {
                    Response.StatusCode = 400;
                    return Json(new { msg = "Por favor, informe um ID de Produto válido." });
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { msg = "Houve um erro ao excluir o produto: " + ex.Message });
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

                    if (Request.Headers["Authorization"].Count > 0 && Request.Headers["Authorization"].ToString().Trim().Length > 0)
                    {
                        if (!TokenService.IsTokenValidMatchSellerId(Request.Headers["Authorization"], _produtoService.GetSellerIdFromProduct(idProduto)))
                        {
                            Response.StatusCode = 401;
                            return Json(new { msg = "O token de acesso informado não é válido." });
                        }
                    }
                    else
                    {
                        Response.StatusCode = 401;
                        return Json(new { msg = "Você deve informar seu token de acesso para acessar este conteúdo." });
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
                        image,
                        request.IsKeepImage
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
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { msg = "Houve um erro ao atualizar o Produto: " + ex });
            }
        }
    }
}
