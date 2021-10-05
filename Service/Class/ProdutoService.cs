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

        public void InsertProduto(string nome, bool status, float preco, string descricao, int quantidadeDisponivel, int idVendedor)
        {
            _produtoRepository.InsertProduct(nome, status, preco, descricao, quantidadeDisponivel, idVendedor);
        }

        public Produto SearchProduto(string nome, int idProduto, int idVendedor) => _produtoRepository.SearchProduct(nome, idProduto, idVendedor);

        public void DeleteProduto(int idProduto){
             _produtoRepository.DeleteProduto(idProduto);
        }
    }
}