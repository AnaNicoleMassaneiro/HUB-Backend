using System;
using HubUfpr.Model;
using HubUfpr.Service.Interface;
using HubUfpr.Data.DapperORM.Interface;
using System.Collections.Generic;

namespace HubUfpr.Service.Class
{
    public class FormaDePagamentoService : IFormaDePagamentoService
    {
        private readonly IFormaDePagamentoRepository _formaDePagamentoRepository;

        public FormaDePagamentoService(IFormaDePagamentoRepository formaDePagamentoRepository)
        {
            _formaDePagamentoRepository = formaDePagamentoRepository;
        }

        public int AddFormaPagamento(int idFormaPagamento, int idVendedor)
        {
            return _formaDePagamentoRepository.AddFormaPagamento(idFormaPagamento, idVendedor);
        }

        public int RemoveFormaPagamento(int idFormaPagamento, int idVendedor)
        {
            return _formaDePagamentoRepository.RemoveFormaPagamento(idFormaPagamento, idVendedor);
        }

        public List<FormaDePagamento> GetFormaDePagamentoByVendedor(int idVendedor)
        {
            return _formaDePagamentoRepository.GetFormaDePagamentoByVendedor(idVendedor);
        }

        public List<FormaDePagamento> ListFormaDePagamento()
        {
            return _formaDePagamentoRepository.ListFormaDePagamento();
        }
    }
}
