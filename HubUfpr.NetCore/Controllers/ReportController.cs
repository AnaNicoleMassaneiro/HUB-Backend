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
    [Route("api/relatorios")]
    public class ReportController : Controller
    {
        public readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("criar")]
        public JsonResult InsertReport([FromBody] CreateReport req)
        {
            try
            {
                if (req.IdVendedor <= 0 || req.Tipo == null || req.Tipo.Trim().Length == 0)
                {
                    Response.StatusCode = 400;
                    return Json(new { msg = "Você deve informar o ID do Vendedor e o tipo de relatório!" });
                }

                ReportHeader header = _reportService.ReportHeader(req.Tipo, req.IdVendedor, req.DateFilter);

                if (header.TotalReservas == 0)
                {
                    Response.StatusCode = 404;
                    return Json(new { msg = "Nenhuma reserva foi encontrada para o período informado. " });
                }

                List<ReportData> data = _reportService.ReportData(req.Tipo, req.IdVendedor, req.DateFilter);

                if (data.Count == 0)
                {
                    Response.StatusCode = 404;
                    return Json(new { msg = "Nenhuma reserva foi encontrada para o período informado. " });
                }

                return Json(new { header, data });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;

                return Json(new { msg = "Houve um erro ao gerar o relatório: " + ex.Message });
            }
        }
    }
}