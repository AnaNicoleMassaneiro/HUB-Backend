using System.Collections.Generic;
using HubUfpr.Model;

namespace HubUfpr.Service.Interface
{
    public interface IProdutoService
    {       
        void InsertProduto(string nome, string status, float preco, string descricao, int qtdProdutosDisponiveis);

        Produto SearchProduto(string nome);
    }
}