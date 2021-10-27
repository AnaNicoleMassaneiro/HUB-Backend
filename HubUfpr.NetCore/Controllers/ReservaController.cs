using System;
using HubUfpr.API.Requests;
using HubUfpr.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HubUfpr.API.Controllers
{
    [Produces("application/json")]
    [Route("api/reserva")]
    public class ReservaController : Controller
    {
        protected readonly IReservaService _reservaService;
        protected readonly IProdutoService _produtoService;

        public ReservaController(IReservaService reservaService, IProdutoService produtoService)
        {
            _reservaService = reservaService;
            _produtoService = produtoService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("create")]
        public JsonResult CreateReserve([FromBody] CreateReserve req)
        {
            try
            {
                if (req.IdCliente > 0 && req.IdProduto > 0 && (req.Latitude != 0 || req.Longitude != 0) &&
                    req.QuantidadeDesejada > 0)
                {
                    if (!_produtoService.IsStockAvailable(req.IdProduto, req.QuantidadeDesejada))
                    {
                        Response.StatusCode = 400;
                        return Json(new { msg = "O Produto informado não possui estoque suficiente para completar a reserva." });
                    }

                    _reservaService.CreateReserve(
                        req.IdCliente, req.IdProduto, req.QuantidadeDesejada, req.Latitude, req.Longitude);
                    return Json(new { msg = "Reserva criada com sucesso." });
                }
                
                Response.StatusCode = 400;
                return Json(new { msg = "Você deve informar o ID do cliente, id do produto, quantidade, latitude e longitude." });
            }
            catch (System.Exception ex)
            {
                throw new InvalidOperationException("Erro ao criar reserva: ", ex);
            }
        }

        [AllowAnonymous]
        [HttpPatch]
        [Route("cancel/{id}")]
        public JsonResult CancelReserve(int id)
        {
            try
            {
                if (id > 0)
                {
                    if (_reservaService.GetCurrentStatus(id) != 0)
                    {
                        Response.StatusCode = 400;
                        return Json(new { msg = "O Status da reserva informada não pode mais ser alterado." });
                    }

                    if (_reservaService.UpdateReserveStatus(id, 3) > 0)
                    {
                        return Json(new { msg = "Reserva cancelada com sucesso." });
                    }

                    return Json(new { msg = "Reserva não encontrada." });
                }

                Response.StatusCode = 400;
                return Json(new { msg = "Você deve informar o ID da reserva." });
            }
            catch (System.Exception ex)
            {
                throw new InvalidOperationException("Erro ao cancelar reserva: ", ex);
            }
        }

        [AllowAnonymous]
        [HttpPatch]
        [Route("confirm/{id}")]
        public JsonResult ConfirmReserve(int id)
        {
            try
            {
                if (id > 0)
                {
                    if (_reservaService.GetCurrentStatus(id) != 0)
                    {
                        Response.StatusCode = 400;
                        return Json(new { msg = "O Status da reserva informada não pode mais ser alterado." });
                    }


                    if (_reservaService.UpdateReserveStatus(id, 1) > 0)
                    {
                        return Json(new { msg = "Reserva confirmada com sucesso." });
                    }

                    return Json(new { msg = "Reserva não encontrada." });
                }

                Response.StatusCode = 400;
                return Json(new { msg = "Você deve informar o ID da reserva." });
            }
            catch (System.Exception ex)
            {
                throw new InvalidOperationException("Erro ao confirmar reserva: ", ex);
            }
        }

        [AllowAnonymous]
        [HttpPatch]
        [Route("expire/{id}")]
        public JsonResult ExpireReserve(int id)
        {
            try
            {
                if (id > 0)
                {
                    if (_reservaService.GetCurrentStatus(id) != 0)
                    {
                        Response.StatusCode = 400;
                        return Json(new { msg = "O Status da reserva informada não pode mais ser alterado." });
                    }

                    if (_reservaService.UpdateReserveStatus(id, 2) > 0)
                    {
                        return Json(new { msg = "Reserva expirada com sucesso." });
                    }

                    return Json(new { msg = "Reserva não encontrada." });
                }

                Response.StatusCode = 400;
                return Json(new { msg = "Você deve informar o ID da reserva." });
            }
            catch (System.Exception ex)
            {
                throw new InvalidOperationException("Erro ao expirar reserva: ", ex);
            }
        }
    }
}
