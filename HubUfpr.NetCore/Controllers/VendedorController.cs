using System;
using System.Collections.Generic;
using HubUfpr.API.Requests;
using HubUfpr.Model;
using HubUfpr.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HubUfpr.API.Controllers
{
    [Produces("application/json")]
    [Route("api/vendedor")]
    public class VendedorController : Controller
    {
        protected readonly IUserService _userService;
        protected readonly IVendedorService _vendedorService;

        public VendedorController(IVendedorService vendedorService, IUserService userService)
        {
            _userService = userService;
            _vendedorService = vendedorService;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("buscarPorId/{id}")]
        public JsonResult SearchVendedorById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    Response.StatusCode = 400;
                    return Json(new { msg = "Você deve especificar o ID do vendedor!" });
                }

                var vendedor = _vendedorService.getVendedorById(id);

                if (vendedor == null)
                {
                    Response.StatusCode = 404;
                    return Json(new { msg = "Vendedor não encontrado." });
                }

                return Json(new { vendedor });

            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Erro ao buscar Vendedor: ", ex);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("buscarPorNome")]
        public JsonResult SearchVendedorByNome([FromBody] SearchVendedorByName req)
        {
            try
            {
                if (req.Name == null || req.Name.Trim().Length == 0)
                {
                    Response.StatusCode = 400;
                    return Json(new { msg = "Você deve informar o nome do Vendedor!" });
                }

                return Json(new { vendedores = _vendedorService.getVendedoresByName(req.Name) });

            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Erro ao buscar Vendedores: ", ex);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("buscarTodos")]
        public JsonResult SearchAllSellers()
        {
            try
            {
                return Json(new { vendedores = _vendedorService.getAllSellers() });

            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Erro ao buscar Vendedores: ", ex);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("buscarPorLocalizacao")]
        public JsonResult SearchSellerByLocation([FromBody] SearchVendedorByLocation req)
        {
            try
            {
                if (req.Latitude == 0 && req.Longitude == 0) {
                    Response.StatusCode = 400;
                    return Json(new { msg = "Você deve informar a Latitude e Longitude." });
                }

                var vendedores = _vendedorService.getVendedoresByLocation(req.Latitude, req.Longitude);

                if (vendedores.Count == 0)
                {
                    Response.StatusCode = 404;
                    return Json(new { msg = "Nenhum Vendedor encontrado. " });
                }

                return Json(new { vendedores });
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Erro ao buscar Vendedores: ", ex);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("favoritos/adicionar")]
        public JsonResult AddFavoriteSeller([FromBody] FavoriteSeller req)
        {
            try
            {
                if (req.IdCliente <= 0 || req.IdVendedor <= 0)
                {
                    Response.StatusCode = 400;
                    return Json(new { msg = "Você deve informar os IDs do Vendedor e do Cliente!" });
                }

                int ret = _vendedorService.AddFavoriteSeller(req.IdVendedor, req.IdCliente);

                if (ret != 1)
                {
                    Response.StatusCode = 400;
                    return Json(new { msg = "Houve um erro ao adicionar o vendedor favorito!" });
                }

                return Json(new { msg = "Vendedor favorito adicionado com sucesso." });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { msg = "Erro ao cadastrar Vendedor favorito: " + ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpDelete]
        [Route("favoritos/remover")]
        public JsonResult DeleteFavoriteSeller([FromBody] FavoriteSeller req)
        {
            try
            {
                if (req.IdCliente <= 0 || req.IdVendedor <= 0)
                {
                    Response.StatusCode = 400;
                    return Json(new { msg = "Você deve informar os IDs do Vendedor e do Cliente!" });
                }

                int ret = _vendedorService.RemoveFavoriteSeller(req.IdVendedor, req.IdCliente);

                if (ret == 0)
                {
                    Response.StatusCode = 404;
                    return Json(new { msg = "Vendedor favorito não foi encontrado." });
                }

                else if (ret > 1)
                {
                    Response.StatusCode = 400;
                    return Json(new { msg = "Algo errado aconteceu, múltiplos registros foram removidos!!" });
                }

                return Json(new { msg = "Vendedor favorito removido com sucesso." });
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Erro ao remover Vendedor favorito: ", ex);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("favoritos/buscar/{idCliente}")]
        public JsonResult GetFavoriteSellerByCustomer(int idCliente)
        {
            try
            {
                if (idCliente <= 0)
                {
                    Response.StatusCode = 400;
                    return Json(new { msg = "Você deve informar o ID do Cliente!" });
                }

                List<Vendedor> favoritos = _vendedorService.GetFavorteSellersByCustomer(idCliente);

                if (favoritos == null || favoritos.Count == 0)
                {
                    Response.StatusCode = 404;
                    return Json(new { msg = "Nenhum Vendedor favorito foi encontrado." });
                }

                return Json(new { favoritos });
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Erro ao buscar Vendedores favoritos: ", ex);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("favoritos/isFavorite")]
        public JsonResult IsSellerInCustomersFavorites([FromBody] FavoriteSeller req)
        {
            try
            {
                if (req.IdCliente <= 0 || req.IdVendedor <= 0)
                {
                    Response.StatusCode = 400;
                    return Json(new { msg = "Você deve informar o ID do Cliente e do Vendedor!" });
                }

                bool isFavorite = _vendedorService.IsVendedorInCustomerFavorites(req.IdCliente, req.IdVendedor);

                return Json(new { isFavorite });
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Erro ao buscar Vendedores favoritos: ", ex);
            }
        }

        [AllowAnonymous]
        [HttpPatch]
        [Route("atualizarStatus")]
        public JsonResult UpdateSellerStatus([FromBody] SellerStatus req)
        {
            try
            { 
                if (req.IdVendedor == null || req.IdVendedor < 1 || req.IsAtivo == null || req.IsOpen == null)
                {
                    Response.StatusCode = 400;
                    return Json(new { msg = "Você deve informar o ID do Vendedor, o Status Ativo e o Status da Loja!" });
                }

                int ret = _vendedorService.UpdateSellerStatus((int)req.IdVendedor, (bool)req.IsAtivo, (bool)req.IsOpen);

                if (ret == 1)
                {
                    return Json(new { msg = "Vendedor atualizado com sucesso. "});
                }

                Response.StatusCode = 404;
                return Json(new { msg = "Vendedor não encontrado." });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { msg = "Erro ao atualizar Vendedor: ", ex });
            }
        }
    }
}
