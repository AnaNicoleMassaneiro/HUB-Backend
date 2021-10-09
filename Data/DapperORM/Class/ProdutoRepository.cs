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

        public void InsertProduct(string nome, bool isAtivo, float preco, string descricao, int quantidadeDisponivel, int idVendedor)
        {
            using var db = GetMySqlConnection();
            const string sql = @"insert into Produto (nome, isAtivo, preco, descricao, quantidadeDisponivel, idVendedor) values (@nome, @isAtivo, @preco, @descricao, @quantidadeDisponivel, @idVendedor)";

            db.Execute(sql, new
            {
                nome,
                isAtivo,
                preco,
                descricao,
                quantidadeDisponivel,
                idVendedor
            }, commandType: CommandType.Text);
        }

        public List<Produto> SearchProduct(string nome, int idProduto, int idVendedor)
        {
            List<Produto> ret = new List<Produto>();
            using var db = GetMySqlConnection();
            const string sql = @"select * from Produto U where nome like @nome OR idProduto = @idProduto OR idVendedor = @idVendedor";
            MySqlCommand cmd = db.CreateCommand();

            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("nome", "%"+nome+"%");
            cmd.Parameters.AddWithValue("idProduto", idProduto);
            cmd.Parameters.AddWithValue("idVendedor", idVendedor);

            MySqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                Produto p = new Produto();
                p.Id = (int)dr["idProduto"];
                p.Nome = (string)dr["nome"];
                p.IsAtivo = (bool)dr["isAtivo"];
                p.Preco = (float)dr["preco"];
                p.Descricao = (string)dr["descricao"];
                p.QuantidadeDisponivel = (int)dr["quantidadeDisponivel"];
                    
                ret.Add(p);
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

        public int UpdateProduto(int idProduto, string nome, bool isAtivo, float preco, string descricao, int quantidadeDisponivel)
        {
            using var db = GetMySqlConnection();
            const string sql = @"update Produto set nome = @nome, isAtivo = @isAtivo, preco = @preco, descricao = @descricao, quantidadeDisponivel = @quantidadeDisponivel where idProduto = @idProduto";

            return db.Execute(sql, new
            {
                idProduto,
                nome,
                isAtivo,
                preco,
                descricao,
                quantidadeDisponivel
            }, commandType: CommandType.Text);
        }
    }
}
