using System.Collections.Generic;
using HubUfpr.Model;

namespace HubUfpr.Service.Interface
{
    public interface IProdutoService
    {       
        void InsertProduto(string nome, string status, float preco, string descricao, int qtdProdutosDisponiveis, int idVendedor);

        Produto SearchProduto(string nome, int idProduto, int idVendedor);

        void DeleteProduto(int idProdutor);
    }
}