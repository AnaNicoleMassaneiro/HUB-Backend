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

        public void InsertProduct(string nome, string status, float preco, string descricao, int qtdProdutosDisponiveis)
        {
            using var db = GetMySqlConnection();
            const string sql = @"insert into Produto (nome, status, preco, descricao, quantidadeProdutosDisponiveis) values (@nome, @status, @preco, @descricao, @quantidadeProdutosDisponiveis)";

            db.Execute(sql, new { nome = nome, status = status, preco = preco, descricao = descricao, quantidadeProdutosDisponiveis = qtdProdutosDisponiveis }, commandType: CommandType.Text);
        }

        public Produto SearchProduct(string nome)
        {
            using var db = GetMySqlConnection();
            const string sql = @"select * from Produto U where nome = @nome";

            return db.Query<Produto>(sql, new { nome = nome }, commandType: CommandType.Text).FirstOrDefault();
        }
    }
}
