using System;
using System.Data;
using System.Linq;
using Dapper;
using HubUfpr.Data.DapperORM.Interface;
using HubUfpr.Model;

namespace HubUfpr.Data.DapperORM.Class
{
    public class ProdutoRepository : BaseRepository, IProdutoRepository
    {
        public ProdutoRepository()
        {
        }

        public void InsertProduct(string nome, string status, float preco, string descricao, int qtdProdutosDisponiveis, int idVendedor)
        {
            using var db = GetMySqlConnection();
            const string sql = @"insert into Produto (nome, status, preco, descricao, quantidadeProdutosDisponiveis, idVendedor) values (@nome, @status, @preco, @descricao, @quantidadeProdutosDisponiveis, @idVendedor)";

            db.Execute(sql, new
            {
                nome = nome,
                status = status,
                preco = preco,
                descricao = descricao,
                quantidadeProdutosDisponiveis = qtdProdutosDisponiveis,
                idVendedor = idVendedor
            }, commandType: CommandType.Text);
        }

        public Produto SearchProduct(string nome, int idProduto, int idVendedor)
        {
            using var db = GetMySqlConnection();
            const string sql = @"select * from Produto U where nome = @nome OR idProduto = @idProduto OR idVendedor = @idVendedor";

            return db.Query<Produto>(sql, new
            {
                nome = nome,
                idProduto = idProduto,
                idVendedor = idVendedor
            }, commandType: CommandType.Text).FirstOrDefault();
        }
    }
}
