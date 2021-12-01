using System;
using HubUfpr.Model;
using System.Collections.Generic;

namespace HubUfpr.Data.DapperORM.Interface
{
    public interface IFormaDePagamentoRepository
    {
        int AddFormaPagamento(int idFormaPagamento, int idVendedor);

        int RemoveFormaPagamento(int idFormaPagamento, int idVendedor);

        List<FormaDePagamento> GetFormaDePagamentoByVendedor(int idVendedor);

        List<FormaDePagamento> ListFormaDePagamento();
    }
}
