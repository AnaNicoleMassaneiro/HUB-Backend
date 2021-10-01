using System;
using HubUfpr.Model;

namespace HubUfpr.Data.DapperORM.Interface
{
    public interface IProdutoRepository
    {
        void InsertProduct(string nome, string status, float preco, string descricao, int qtdProdutosDisponiveis, int idVendedor);

        Produto SearchProduct(string nome, int idProduto, int idVendedor);
    }
}
