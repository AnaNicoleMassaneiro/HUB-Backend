using System;
namespace HubUfpr.Data.DapperORM.Interface
{
    public interface IProdutoRepository
    {
        void InsertProduct(string nome, string status, float preco, string descricao, int qtdProdutosDisponiveis);
    }
}
