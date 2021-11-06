using System;
using System.Collections.Generic;
using HubUfpr.Model;

namespace HubUfpr.Data.DapperORM.Interface
{
    public interface IProdutoRepository
    {
        void InsertProduct(string nome, bool status, float preco, string descricao, int quantidadeDisponivel, int idVendedor, string imagem);

        Produto SearchProductById(int idProduto);

        List<Produto> SearchProductByName(string name, bool isReturnAtivoOnly);

        List<Produto> SearchProductBySeller(int idSeller, bool isReturnAtivoOnly);

        int DeleteProduto(int idProduto);

        int UpdateProduto(int idProduto, string nome, bool status, float preco, string descricao, int quantidadeDisponivel, string imagem, bool isKeepImage);

        int UpdateScore(int idProduto, float score);

        List<Produto> GetAllProducts();

        bool IsStockAvailable(int idProduto, int quantity);

        int UpdateCurrentAvailableAmount(int idProduto, int amount);

        int GetCurrentAvailableAmount(int idProduto);
    }
}
