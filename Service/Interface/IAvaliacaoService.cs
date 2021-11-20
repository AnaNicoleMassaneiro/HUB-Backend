using System.Collections.Generic;
using HubUfpr.Model;

namespace HubUfpr.Service.Interface
{
    public interface IAvaliacaoService
    {
        void InsertAvaliacao(int tipoAvaliacao, int idCliente, int idVendedor, int idProduto, int nota, string titulo, string descricao);

        List<Avaliacao> GetAvaliacao(int idProduto, int idVendedor, int idCliente);
    }
}