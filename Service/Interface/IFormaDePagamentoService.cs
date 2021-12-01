using HubUfpr.Model;
using System.Collections.Generic;

namespace HubUfpr.Service.Interface
{
    public interface IFormaDePagamentoService
    {
        int AddFormaPagamento(int idFormaPagamento, int idVendedor);

        int RemoveFormaPagamento(int idFormaPagamento, int idVendedor);

        List<FormaDePagamento> GetFormaDePagamentoByVendedor(int idVendedor);

        List<FormaDePagamento> ListFormaDePagamento();
    }
}
