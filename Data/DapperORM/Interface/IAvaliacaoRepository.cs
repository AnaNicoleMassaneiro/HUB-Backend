using System.Collections.Generic;
using HubUfpr.Model;

namespace HubUfpr.Data.DapperORM.Interface
{
    public interface IAvaliacaoRepository
    {
        void InsertAvaliacao(int tipoAvaliacao, int idCliente, int idVendedor, int idProduto, int nota, string titulo, string descricao);

        List<Avaliacao> GetAvaliacao(int idProduto, int idVendedor, int idCliente);
    }
}
