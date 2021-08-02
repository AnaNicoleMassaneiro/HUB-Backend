using System;
using System.Data;
using Dapper;
using HubUfpr.Data.DapperORM.Interface;

namespace HubUfpr.Data.DapperORM.Class
{
    public class ProdutoRepository : BaseRepository, IProdutoRepository
    {
        public ProdutoRepository()
        {
        }

        public void InsertProduct(string nome, string status, float preco, string descricao, int qtdProdutosDisponiveisl)
        {
            using var db = GetMySqlConnection();
            const string sql = @"insert into Produto (nome, status, preco, descricao, quantidadeProdutosDisponiveis) values (@nome, @status, @preco, @descricao, @quantidadeProdutosDisponiveis)";

            db.Execute(sql, new { nome = nome, status = status, preco = preco, descricao = descricao, quantidadeProdutosDisponiveis = qtdProdutosDisponiveisl }, commandType: CommandType.Text);
        }
    }
}
