using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using HubUfpr.Data.DapperORM.Interface;
using HubUfpr.Model;
using MySql.Data.MySqlClient;

namespace HubUfpr.Data.DapperORM.Class
{
    public class ProdutoRepository : BaseRepository, IProdutoRepository
    {
        public ProdutoRepository()
        {
        }

        public void InsertProduct(string nome, bool isAtivo, float preco, string descricao, int quantidadeDisponivel, int idVendedor, string imagem)
        {
            using var db = GetMySqlConnection();
            const string sql = @"insert into Produto (nome, isAtivo, preco, descricao, quantidadeDisponivel, idVendedor, imagem) values " +
                "(@nome, @isAtivo, @preco, @descricao, @quantidadeDisponivel, @idVendedor, @imagem)";

            db.Execute(sql, new
            {
                nome,
                isAtivo,
                preco,
                descricao,
                quantidadeDisponivel,
                idVendedor,
                imagem
            }, commandType: CommandType.Text);
        }

        public List<Produto> SearchProduct(string nome, int idProduto, int idVendedor)
        {
            List<Produto> ret = new List<Produto>();
            using var db = GetMySqlConnection();
            string sql;

            if (nome != null)
                sql = @"select p.idProduto, p.idVendedor, p.nome, p.isAtivo, p.preco, p.notaProduto, p.descricao, p.imagem, p.quantidadeDisponivel, " +
                    "v.isAtivo, v.isOpen, v.idVendedor, v.idUser from Produto p " + 
                    "join Vendedor v on v.idVendedor = p.idVendedor " +
                    "where nome like @nome OR idProduto = @idProduto OR idVendedor = @idVendedor";
            else
                sql = @"select * from Produto U where idProduto = @idProduto OR idVendedor = @idVendedor";

            MySqlCommand cmd = db.CreateCommand();

            cmd.CommandText = sql;

            if (nome != null) cmd.Parameters.AddWithValue("nome", "%"+nome+"%");
            cmd.Parameters.AddWithValue("idProduto", idProduto);
            cmd.Parameters.AddWithValue("idVendedor", idVendedor);

            MySqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                ret.Add(GetProductFromDataReader(dr));
            }

            dr.Close();

            return ret;
        }

        public int DeleteProduto(int idProduto)
        {
            using var db = GetMySqlConnection();
            const string sql = @"delete from Produto where idProduto = @idProduto";

            return db.Execute(sql, new { idProduto }, commandType: CommandType.Text);
        }

        public int UpdateProduto(int idProduto, string nome, bool isAtivo, float preco, string descricao, int quantidadeDisponivel, string imagem)
        {
            using var db = GetMySqlConnection();
            string sql = @"update Produto set nome = @nome, isAtivo = @isAtivo, preco = @preco, descricao = @descricao, quantidadeDisponivel = @quantidadeDisponivel";

            if (imagem != null)
            {
                sql += ", imagem = @imagem";
            }

            sql += " where idProduto = @idProduto";


            return db.Execute(sql, new
            {
                idProduto,
                nome,
                isAtivo,
                preco,
                descricao,
                quantidadeDisponivel,
                imagem
            }, commandType: CommandType.Text);
        }

        public int UpdateScore(int productId, float score)
        {
            using var db = GetMySqlConnection();
            const string sql = @"update Produto set notaProduto = @score where idProduto = @productId";

            return db.Execute(sql, new { score, productId }, commandType: CommandType.Text);
        }

        public List<Produto> GetAllProducts()
        {
            List<Produto> ret = new List<Produto>();
            using var db = GetMySqlConnection();
            string sql = @"select p.idProduto, p.idVendedor, p.nome, p.isAtivo, p.preco, p.notaProduto, p.descricao, p.imagem, p.quantidadeDisponivel, " + 
                "v.isAtivo, v.isOpen, v.idVendedor, v.idUser from Produto p " +
                "join Vendedor v on v.idVendedor = p.idVendedor " + 
                "where v.isAtivo = true and p.isAtivo = true";

            MySqlCommand cmd = db.CreateCommand();

            cmd.CommandText = sql;

            MySqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                ret.Add(GetProductFromDataReader(dr));
            }

            dr.Close();

            return ret;
        }

        private Produto GetProductFromDataReader(MySqlDataReader dr)
        {
            Produto p = new Produto();
            Vendedor v = new Vendedor();
            
            p.Id = (int)dr["idProduto"];
            p.Nome = (string)dr["nome"];
            p.IsAtivo = (bool)dr["isAtivo"];
            p.Preco = (float)dr["preco"];
            p.NotaProduto = (float)dr["notaProduto"];
            p.Descricao = (string)dr["descricao"];
            if (dr["imagem"] != DBNull.Value) p.Imagem = (string)dr["imagem"];
            p.QuantidadeDisponivel = (int)dr["quantidadeDisponivel"];

            v.IdUser = (int)dr["idUser"];
            v.IdVendedor = (int)dr["idVendedor"];
            v.IsAtivo = (bool)dr["isAtivo"];
            v.IsOpen = (bool)dr["isOpen"];

            p.Vendedor = v;

            return p;
        }
    }
}
