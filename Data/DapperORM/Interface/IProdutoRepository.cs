using System;
using System.Collections.Generic;
using HubUfpr.Model;

namespace HubUfpr.Data.DapperORM.Interface
{
    public interface IProdutoRepository
    {
        void InsertProduct(string nome, bool status, float preco, string descricao, int quantidadeDisponivel, int idVendedor, string imagem);

        List<Produto> SearchProduct(string nome, int idProduto, int idVendedor);

        int DeleteProduto(int idProduto);

        int UpdateProduto(int idProduto, string nome, bool status, float preco, string descricao, int quantidadeDisponivel, string imagem);

        int UpdateScore(int idProduto, float score);
    }
}
