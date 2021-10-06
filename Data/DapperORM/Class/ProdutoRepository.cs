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

        public void InsertProduct(string nome, bool status, float preco, string descricao, int quantidadeDisponivel, int idVendedor)
        {
            using var db = GetMySqlConnection();
            const string sql = @"insert into Produto (nome, status, preco, descricao, quantidadeDisponivel, idVendedor) values (@nome, @status, @preco, @descricao, @quantidadeDisponivel, @idVendedor)";

            db.Execute(sql, new
            {
                nome = nome,
                status = status,
                preco = preco,
                descricao = descricao,
                quantidadeDisponivel = quantidadeDisponivel,
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

        public void DeleteProduto(int idProduto)
        {
            using var db = GetMySqlConnection();
            const string sql = @"delete from Produto where idProduto = @idProduto";

            db.Execute(sql, new
            {
                idProduto = idProduto
            }, commandType: CommandType.Text);
        }

        public void UpdateProduto(int idProduto, string nome, bool status, float preco, string descricao, int quantidadeDisponivel, int idVendedor)
        {
            using var db = GetMySqlConnection();
            const string sql = @"update Produto set nome = @nome, status = @status, preco = @preco, descricao = @descricao, quantidadeDisponivel = @quantidadeDisponivel, idVendedor = @idVendedor where idProduto = @idProduto";

            db.Execute(sql, new
            {
                idProduto = idProduto,
                nome = nome,
                status = status,
                preco = preco,
                descricao = descricao,
                quantidadeDisponivel = quantidadeDisponivel,
                idVendedor = idVendedor
            }, commandType: CommandType.Text);
        }
    }
}
