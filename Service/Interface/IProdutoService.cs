using System.Collections.Generic;
using HubUfpr.Model;

namespace HubUfpr.Service.Interface
{
    public interface IProdutoService
    {       
        void InsertProduto(string nome, bool status, float preco, string descricao, int quantidadeDisponivel, int idVendedor, string imagem);

        List<Produto> SearchProduto(string nome, int idProduto, int idVendedor);

        int DeleteProduto(int idProdutor);

        int UpdateProduto(int idProduto, string nome, bool status, float preco, string descricao, int quantidadeDisponivel, string image);

        int UpdateScore(int idProduto, float score);

        List<Produto> GetAllProducts();
    }
}