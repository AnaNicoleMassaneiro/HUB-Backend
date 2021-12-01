using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

            db.Close();
        }

        public Produto SearchProductById(int idProduto)
        {
            Produto p = null;
            using var db = GetMySqlConnection();
            string sql = sql = @"select p.idProduto, p.idVendedor, p.nome, p.isAtivo, p.preco, p.notaProduto, p.descricao, p.imagem, p.quantidadeDisponivel, " +
                    "v.isAtivo, v.isOpen, v.idVendedor, v.idUser from Produto p " +
                    "join Vendedor v on v.idVendedor = p.idVendedor " +
                    "where p.idProduto = @id";

            MySqlCommand cmd = db.CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("id", idProduto);

            MySqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                dr.Read();
                p = GetProductFromDataReader(dr);
            }
            
            dr.Close();
            db.Close();

            return p;
        }

        public List<Produto> SearchProductByName(string name, bool isReturnAtivoOnly)
        {
            List<Produto> ret = new List<Produto>();
            using var db = GetMySqlConnection();
            string sql;

            sql = @"select p.idProduto, p.idVendedor, p.nome, p.isAtivo, p.preco, p.notaProduto, p.descricao, p.imagem, p.quantidadeDisponivel, " +
                "v.isAtivo, v.isOpen, v.idVendedor, v.idUser from Produto p " + 
                "join Vendedor v on v.idVendedor = p.idVendedor " +
                "where p.nome like @nome";

            if (isReturnAtivoOnly)
                sql += " AND p.isAtivo = true AND v.isAtivo = true";

            MySqlCommand cmd = db.CreateCommand();

            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("nome", "%"+name+"%");
            
            MySqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                ret.Add(GetProductFromDataReader(dr));
            }

            db.Close();
            dr.Close();

            return ret;
        }

        public List<Produto> SearchProductBySeller(int idSeller, bool isReturnAtivoOnly)
        {
            List<Produto> ret = new List<Produto>();
            using var db = GetMySqlConnection();
            string sql;

            sql = @"select p.idProduto, p.idVendedor, p.nome, p.isAtivo, p.preco, p.notaProduto, p.descricao, p.imagem, p.quantidadeDisponivel, " +
                "v.isAtivo, v.isOpen, v.idVendedor, v.idUser from Produto p " +
                "join Vendedor v on v.idVendedor = p.idVendedor " +
                "where p.idVendedor = @id";

            if (isReturnAtivoOnly)
                sql += " AND p.isAtivo = true AND v.isAtivo = true";

            MySqlCommand cmd = db.CreateCommand();

            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("id", idSeller);

            MySqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                ret.Add(GetProductFromDataReader(dr));
            }

            db.Close();
            dr.Close();

            return ret;
        }

        public int DeleteProduto(int idProduto)
        {
            using var db = GetMySqlConnection();
            const string sql = @"delete from Produto where idProduto = @idProduto";

            return db.Execute(sql, new { idProduto }, commandType: CommandType.Text);
        }

        public int UpdateProduto(int idProduto, string nome, bool isAtivo, float preco, string descricao, int quantidadeDisponivel, string imagem, bool isKeepImage)
        {
            using var db = GetMySqlConnection();
            string sql = @"update Produto set nome = @nome, isAtivo = @isAtivo, preco = @preco, descricao = @descricao, " +
                "quantidadeDisponivel = @quantidadeDisponivel";

            if (!isKeepImage)
            {
                sql += ", imagem = @imagem";
            }

            sql += " where idProduto = @idProduto";

            var ret = db.Execute(sql, new
            {
                idProduto,
                nome,
                isAtivo,
                preco,
                descricao,
                quantidadeDisponivel,
                imagem
            }, commandType: CommandType.Text);

            db.Close();

            return ret;
        }

        public int UpdateScore(int productId, float score)
        {
            using var db = GetMySqlConnection();
            const string sql = @"update Produto set notaProduto = @score where idProduto = @productId";

            var ret = db.Execute(sql, new { score, productId }, commandType: CommandType.Text);

            db.Close();

            return ret;
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

            db.Close();
            dr.Close();

            return ret;
        }

        public bool IsStockAvailable(int idProduto, int quantity)
        {
            using var db = GetMySqlConnection();
            string sql = @"SELECT quantidadeDisponivel from Produto where idProduto = @idProduto and quantidadeDisponivel >= @quantity";

            var ret = db.Query<string>(sql, new { idProduto, quantity }, commandType: CommandType.Text).Any();

            db.Close();

            return ret;
        }

        public int GetCurrentAvailableAmount(int idProduto)
        {
            using var db = GetMySqlConnection();
            const string sql = @"SELECT quantidadeDisponivel FROM Produto where idProduto = @idProduto";
            int ret = db.Query<int>(sql, new { idProduto }).FirstOrDefault();

            db.Close();

            return ret;
        }

        public int UpdateCurrentAvailableAmount(int idProduto, int amount)
        {
            using var db = GetMySqlConnection();
            const string sql = @"UPDATE Produto set quantidadeDisponivel = @amount WHERE idProduto = @idProduto";
            int ret = db.Execute(sql, new { idProduto, amount });

            db.Close();

            return ret;
        }

        private Produto GetProductFromDataReader(MySqlDataReader dr)
        {
            Produto p = new Produto();
            Vendedor v = new Vendedor();
            
            p.Id = (int)dr["idProduto"];
            p.idVendedor = (int)dr["idVendedor"];
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
