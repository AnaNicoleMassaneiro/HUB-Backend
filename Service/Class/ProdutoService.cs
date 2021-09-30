using System.Collections.Generic;
using HubUfpr.Data.DapperORM.Interface;
using HubUfpr.Model;
using HubUfpr.Service.Interface;

namespace HubUfpr.Service.Class
{
    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoService(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public List<User> GetUserList()
        {
            throw new System.NotImplementedException();
        }

        public void InsertProduto(string nome, string status, float preco, string descricao, int qtdProdutosDisponiveis)
        {
            _produtoRepository.InsertProduct(nome, status, preco, descricao, qtdProdutosDisponiveis);
        }

        public Produto SearchProduto(string nome, int idProduto, int idVendedor) => _produtoRepository.SearchProduct(nome, idProduto, idVendedor);
    }
}