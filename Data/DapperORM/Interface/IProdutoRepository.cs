using System;
using HubUfpr.Model;

namespace HubUfpr.Data.DapperORM.Interface
{
    public interface IProdutoRepository
    {
        void InsertProduct(string nome, bool status, float preco, string descricao, int quantidadeDisponivel, int idVendedor);

        Produto SearchProduct(string nome, int idProduto, int idVendedor);

        void DeleteProduto(int idProduto);

        void UpdateProduto(int idProduto, string nome, bool status, float preco, string descricao, int quantidadeDisponivel, int idVendedor);
    }
}
