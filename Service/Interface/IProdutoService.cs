using System.Collections.Generic;
using HubUfpr.Model;

namespace HubUfpr.Service.Interface
{
    public interface IProdutoService
    {       
        void InsertProduto(string nome, bool status, float preco, string descricao, int quantidadeDisponivel, int idVendedor, string imagem);

        Produto SearchProductById(int idProduto);

        List<Produto> SearchProductByName(string name, bool isReturnAtivoOnly);

        List<Produto> SearchProductBySeller(int idSeller, bool isReturnAtivoOnly);

        int DeleteProduto(int idProdutor);

        int UpdateProduto(int idProduto, string nome, bool status, float preco, string descricao, int quantidadeDisponivel, string image, bool isKeepImage);

        int UpdateScore(int idProduto, float score);

        List<Produto> GetAllProducts();

        bool IsStockAvailable(int idProduto, int quantity);

        int GetSellerIdFromProduct(int id);
    }
}