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

        public void InsertProduto(
            string nome,
            bool status,
            float preco,
            string descricao,
            int quantidadeDisponivel,
            int idVendedor,
            string imagem
        )
        {
            _produtoRepository.InsertProduct (
                nome,
                status,
                preco,
                descricao,
                quantidadeDisponivel,
                idVendedor,
                imagem
            );
        }

        public Produto SearchProductById(int idProduto)
        {
            return _produtoRepository.SearchProductById(idProduto);
        }

        public List<Produto> SearchProductByName(string name, bool isReturnAtivoOnly)
        {
            return _produtoRepository.SearchProductByName(name, isReturnAtivoOnly);
        }

        public List<Produto> SearchProductBySeller(int idSeller, bool isReturnAtivoOnly)
        {
            return _produtoRepository.SearchProductBySeller(idSeller, isReturnAtivoOnly);
        }

        public int DeleteProduto(int idProduto)
        {
            return _produtoRepository.DeleteProduto (idProduto);
        }

        public int UpdateProduto(
            int idProduto,
            string nome,
            bool status,
            float preco,
            string descricao,
            int quantidadeDisponivel,
            string image,
            bool isKeepImage
        )
        {
            return _produtoRepository.UpdateProduto(
                idProduto,
                nome,
                status,
                preco,
                descricao,
                quantidadeDisponivel,
                image,
                isKeepImage
            );
        }

        public int UpdateScore(int productId, float score)
        {
            return _produtoRepository.UpdateScore(productId, score);
        }

        public List<Produto> GetAllProducts()
        {
            return _produtoRepository.GetAllProducts();
        }

        public bool IsStockAvailable(int idProduto, int quantity)
        {
            return _produtoRepository.IsStockAvailable(idProduto, quantity);
        }
    }
}
