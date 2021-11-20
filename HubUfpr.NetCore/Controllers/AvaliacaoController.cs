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
    [Route("api/avaliacao")]
    public class AvaliacaoController : Controller
    {
        public readonly IAvaliacaoService _avaliacaoService;

        public AvaliacaoController(IAvaliacaoService avaliacaoService)
        {
            _avaliacaoService = avaliacaoService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("insert")]
        public JsonResult InsertAvaliacao([FromBody] InsertAvaliacao req)
        {
            try
            {
                if (
                    (req.IdCliente <= 0 && req.IdProduto <= 0) ||
                    (req.IdCliente <= 0 && req.IdVendedor <= 0) ||
                    req.Nota < 0 ||
                    req.TipoAvaliacao <= 0 || req.TipoAvaliacao > 3 ||
                    req.Titulo == null || req.Titulo.Trim() == "" ||
                    req.Descricao == null || req.Descricao.Trim() == ""
                )
                {
                    Response.StatusCode = 400;
                    return Json(new
                    {
                        msg =
                            "Por favor, informe o tipo da avaliação, os IDs das entidades envolvidas, o título, nota e descricao da avaliação."
                    });
                }
                else if (req.Titulo.Length > 50)
                {
                    Response.StatusCode = 400;
                    return Json(new
                    {
                        msg =
                            "O título da avaliação não pode ter mais do que 50 caracteres!"
                    }); ;
                }
                else if (req.Descricao.Length > 200)
                {
                    Response.StatusCode = 400;
                    return Json(new
                    {
                        msg =
                            "A descrição da avaliação não pode ter mais do que 200 caracteres!"
                    }); ;
                }
                else
                {
                    _avaliacaoService.InsertAvaliacao(
                        req.TipoAvaliacao,
                        req.IdCliente,
                        req.IdVendedor,
                        req.IdProduto,
                        req.Nota,
                        req.Titulo,
                        req.Descricao
                    );
                       
                    return Json(new { msg = "Avaliação criada com sucesso!" });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao criar avaliação: ", ex);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("cliente/{idCliente}")]
        public JsonResult GetCustomerAvaliations(int idCliente)
        {
            try
            {
                if (idCliente <= 0)
                {
                    Response.StatusCode = 400;
                    return Json(new { msg = "Você deve informar o ID do Cliente!" });
                }

                List<Avaliacao> avaliacoes = _avaliacaoService.GetAvaliacao(0, 0, idCliente);

                if (avaliacoes == null || avaliacoes.Count == 0)
                {
                    Response.StatusCode = 404;
                    return Json(new { msg = "Nenhuma avaliação foi encontrada!" });
                }

                return Json(new { avaliacoes });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { msg = "Houve um erro ao buscar as Avaliações: " + ex });
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("vendedor/{idVendedor}")]
        public JsonResult GetSellerAvaliations(int idVendedor)
        {
            try
            {
                if (idVendedor <= 0)
                {
                    Response.StatusCode = 400;
                    return Json(new { msg = "Você deve informar o ID do Vendedor!" });
                }

                List<Avaliacao> avaliacoes = _avaliacaoService.GetAvaliacao(0, idVendedor, 0);

                if (avaliacoes == null || avaliacoes.Count == 0)
                {
                    Response.StatusCode = 404;
                    return Json(new { msg = "Nenhuma avaliação foi encontrada!" });
                }

                return Json(new { avaliacoes });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { msg = "Houve um erro ao buscar as Avaliações: " + ex });
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("produto/{idProduto}")]
        public JsonResult GetProductAvaliations(int idProduto)
        {
            try
            {
                if (idProduto <= 0)
                {
                    Response.StatusCode = 400;
                    return Json(new { msg = "Você deve informar o ID do Produto!" });
                }

                List<Avaliacao> avaliacoes = _avaliacaoService.GetAvaliacao(idProduto, 0, 0);

                if (avaliacoes == null || avaliacoes.Count == 0)
                {
                    Response.StatusCode = 404;
                    return Json(new { msg = "Nenhuma avaliação foi encontrada!" });
                }

                return Json(new { avaliacoes });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { msg = "Houve um erro ao buscar as Avaliações: " + ex });
            }
        }
    }

}
