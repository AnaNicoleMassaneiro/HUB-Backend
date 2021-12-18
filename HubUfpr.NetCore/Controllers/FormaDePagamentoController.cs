using System;
using System.Collections.Generic;
using HubUfpr.API.Requests;
using HubUfpr.Model;
using HubUfpr.Service.Class;
using HubUfpr.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HubUfpr.API.Controllers
{
    [Produces("application/json")]
    [Route("api/formaPagamento")]
    public class FormaDePagamento : Controller
    {
        protected readonly IFormaDePagamentoService _formaDePagamentoService;

        public FormaDePagamento(IFormaDePagamentoService formaDePagamentoService)
        {
            _formaDePagamentoService = formaDePagamentoService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("adicionar")]
        public JsonResult AddFormaDePagamento([FromBody] FormaDePagamentoRequest req)
        {
            try
            {
                if (req.IdFormaDePagamento <= 0 || req.IdVendedor <= 0)
                {
                    Response.StatusCode = 400;
                    return Json(new { msg = "Você deve informar o ID da Forma de Pagamento de do Vendedor!" });
                }

                if (Request.Headers["Authorization"].Count > 0 && Request.Headers["Authorization"].ToString().Trim().Length > 0)
                {
                    if (!TokenService.IsTokenValidMatchSellerId(Request.Headers["Authorization"], req.IdVendedor))
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

                int ret = _formaDePagamentoService.AddFormaPagamento(req.IdFormaDePagamento, req.IdVendedor);

                if (ret == 1)
                {
                    return Json(new { msg = "Forma de Pagamento vinculada com sucesso!" });
                }

                Response.StatusCode = 400;
                return Json(new { msg = "Houve um erro ao vincular a Forma de Pagamento ao Vendedor!" });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { msg = "Houve um erro ao vincular a Forma de Pagamento ao Vendedor: " + ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpDelete]
        [Route("remover")]
        public JsonResult RemoveFormaDePagamento([FromBody] FormaDePagamentoRequest req)
        {
            try
            {
                if (req.IdFormaDePagamento <= 0 || req.IdVendedor <= 0)
                {
                    Response.StatusCode = 400;
                    return Json(new { msg = "Você deve informar o ID da Forma de Pagamento de do Vendedor!" });
                }

                if (Request.Headers["Authorization"].Count > 0 && Request.Headers["Authorization"].ToString().Trim().Length > 0)
                {
                    if (!TokenService.IsTokenValidMatchSellerId(Request.Headers["Authorization"], req.IdVendedor))
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

                int ret = _formaDePagamentoService.RemoveFormaPagamento(req.IdFormaDePagamento, req.IdVendedor);

                if (ret == 1)
                {
                    return Json(new { msg = "Forma de Pagamento removida com sucesso!" });
                }

                Response.StatusCode = 404;
                return Json(new { msg = "Forma de Pagamento não encontrada para o Vendedor informado." });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { msg = "Houve um erro ao remover a Forma de Pagamento do Vendedor: " + ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("buscar/{idVendedor}")]
        public JsonResult GetFormaDePagamentoBySeller(int idVendedor)
        {
            try
            {
                if (idVendedor <= 0)
                {
                    Response.StatusCode = 400;
                    return Json(new { msg = "Você deve informar o ID do Vendedor!" });
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

                var formasDePagamento = _formaDePagamentoService.GetFormaDePagamentoByVendedor(idVendedor);

                if (formasDePagamento.Count == 0)
                {
                    Response.StatusCode = 404;
                    return Json(new { msg = "Nenhuma Forma de Pagamento encontrada." });
                }

                return Json(new { formasDePagamento });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { msg = "Houve um erro ao buscar a Forma de Pagamento do Vendedor: " + ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("buscar")]
        public JsonResult ListFormaDePagamento()
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

                var formasDePagamento = _formaDePagamentoService.ListFormaDePagamento();

                if (formasDePagamento.Count == 0)
                {
                    Response.StatusCode = 404;
                    return Json(new { msg = "Nenhuma Forma de Pagamento encontrada." });
                }

                return Json(new { formasDePagamento });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { msg = "Houve um erro ao buscar as Formas de Pagamento: " + ex.Message });
            }
        }
    }
}
