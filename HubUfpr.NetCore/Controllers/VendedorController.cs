using System;
using HubUfpr.API.Requests;
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

                return Json(new { vendedor = _vendedorService.getVendedorById(id) });

            }
            catch (System.Exception ex)
            {
                throw new InvalidOperationException("Erro ao buscar Vendedor: ", ex);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("buscarPorNome")]
        public JsonResult SearchVendedorById([FromBody] SearchVendedorByName req)
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
            catch (System.Exception ex)
            {
                throw new InvalidOperationException("Erro ao buscar Vendedor: ", ex);
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
            catch (System.Exception ex)
            {
                throw new InvalidOperationException("Erro ao buscar Vendedor: ", ex);
            }
        }
    }
}
