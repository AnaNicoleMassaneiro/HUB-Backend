using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using HubUfpr.Data.DapperORM.Interface;
using HubUfpr.Model;
using MySql.Data.MySqlClient;

namespace HubUfpr.Data.DapperORM.Class
{
    public class AvaliacaoRepository : BaseRepository, IAvaliacaoRepository
    {
        //  TIPOS DE AVALIAÇÃO: 
        // 1 - "Cliente => Produto"
        // 2 - "Cliente => Vendedor"
        // 3 - "Vendedor => Cliente"

        private readonly IProdutoRepository _produtoRepository;
        private readonly IUserRepository _userRepository;
        private readonly IVendedorRepository _vendedorRepository;

        public AvaliacaoRepository(IProdutoRepository produtoRepository, IUserRepository userRepository, IVendedorRepository vendedorRepository)
        {
            _produtoRepository = produtoRepository;
            _userRepository = userRepository;
            _vendedorRepository = vendedorRepository;
        }

        public void InsertAvaliacao(int tipoAvaliacao, int idCliente, int idVendedor, int idProduto, int nota, string titulo, string descricao)
        {
            try
            {
                using var db = GetMySqlConnection();
                
                const string sql = @"INSERT INTO Avaliacao (idCliente, idVendedor, idProduto, tipoAvaliacao, titulo, nota, descricao) VALUES " +
                    "(@idCliente, @idVendedor, @idProduto, @tipoAvaliacao, @titulo, @nota, @descricao);";

                db.Execute(sql, new
                {
                    idCliente = idCliente == 0 ? null : idCliente.ToString(),
                    idVendedor = idVendedor == 0 ? null : idVendedor.ToString(),
                    idProduto = idProduto == 0 ? null : idProduto.ToString(),
                    tipoAvaliacao,
                    titulo,
                    nota, 
                    descricao
                });

                switch (tipoAvaliacao)
                {
                    case 1:
                        UpdateProductRating(idProduto);
                        break;
                    case 2:
                        UpdateSellerRating(idVendedor);
                        break;
                    case 3:
                        UpdateCustomerRating(idCliente);
                        break;
                    default:
                        throw new Exception("Tipo de avaliação informado é inválido!");
                }

                db.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Houve um erro ao inserir a avaliação: " + ex.Message);
            }
        }

        public List<Avaliacao> GetAvaliacao(int idProduto, int idVendedor, int idCliente)
        {
            try
            {
                using var db = GetMySqlConnection();
                List<Avaliacao> reservas = new List<Avaliacao>();
                string sql = @"SELECT idAvaliacao, tipoAvaliacao, idCliente, idVendedor, idProduto, titulo, nota, descricao, dataCriacao " +
                    "FROM Avaliacao WHERE ";
                MySqlCommand cmd = db.CreateCommand();
                MySqlDataReader dr;

                if (idProduto > 0 && idVendedor == 0 && idCliente == 0)
                {
                    sql += "idProduto = @idProduto AND tipoAvaliacao = 1";
                    cmd.Parameters.AddWithValue("idProduto", idProduto);

                }
                else if (idVendedor > 0 && idProduto == 0 && idCliente == 0)
                {
                    sql += "idVendedor = @idVendedor AND tipoAvaliacao = 2";
                    cmd.Parameters.AddWithValue("idVendedor", idVendedor);
                }
                else if (idCliente > 0 && idProduto == 0 && idVendedor == 0)
                {
                    sql += "idCliente = @idCliente AND tipoAvaliacao = 3";
                    cmd.Parameters.AddWithValue("idCliente", idCliente);
                }
                else
                    throw new Exception("Houve um erro ao buscar avaliações: tipo da avaliação escolhido é inválido!");

                sql += " ORDER BY dataCriacao DESC;";

                cmd.CommandText = sql;
                dr = cmd.ExecuteReader();

                if (!dr.HasRows)
                {
                    dr.Close();
                    db.Close();
                    return null;
                }

                while (dr.Read())
                {
                    reservas.Add(GetAvaliacaoFromDataReader(dr));
                }

                dr.Close();
                db.Close();
                return reservas;
            }
            catch (Exception ex)
            {
                throw new Exception("Houve um erro ao buscar avaliações: " + ex.Message);
            }
        }

        private void UpdateProductRating(int idProduto)
        {
            try {
                var db = GetMySqlConnection();
                const string sql1 = @"SELECT AVG(nota) FROM Avaliacao WHERE idProduto = @idProduto AND idVendedor IS NULL AND idCliente IS NOT NULL AND tipoAvaliacao = 1;";
                const string sql2 = @"UPDATE Produto set notaProduto = @nota WHERE idProduto = @idProduto;";

                float nota = db.Query<float>(sql1, new { idProduto }).FirstOrDefault();

                db.Execute(sql2, new { nota, idProduto });

                db.Close();
            }
            catch (Exception e)
            {
                throw new Exception("Houve um erro ao atualizar a nota do produto: " + e.Message);
            }
        }

        private void UpdateSellerRating(int idVendedor)
        {
            try
            {
                var db = GetMySqlConnection();
                const string sql1 = @"SELECT AVG(nota) FROM Avaliacao WHERE idVendedor = @idVendedor AND idProduto IS NULL AND idCliente IS NOT NULL AND tipoAvaliacao = 2;";
                const string sql2 = @"UPDATE User u JOIN Vendedor v on v.idUser = u.Id SET NoteApp = @nota WHERE v.idVendedor = @idVendedor";

                float nota = db.Query<float>(sql1, new { idVendedor }).FirstOrDefault();

                db.Execute(sql2, new { nota, idVendedor });

                db.Close();
            }
            catch (Exception e)
            {
                throw new Exception("Houve um erro ao atualizar a nota do vendedor: " + e.Message);
            }
        }

        private void UpdateCustomerRating(int idCliente)
        {
            try
            {
                var db = GetMySqlConnection();
                const string sql1 = @"SELECT AVG(nota) FROM Avaliacao WHERE idCliente = @idCliente AND idProduto IS NULL AND idVendedor IS NOT NULL AND tipoAvaliacao = 3;";
                const string sql2 = @"UPDATE User u JOIN Cliente c on c.idUser = u.Id SET NoteApp = @nota WHERE c.idCliente = @idCliente";

                float nota = db.Query<float>(sql1, new { idCliente }).FirstOrDefault();

                db.Execute(sql2, new { nota, idCliente });

                db.Close();
            }
            catch (Exception e)
            {
                throw new Exception("Houve um erro ao atualizar a nota do cliente: " + e.Message);
            }
        }

        protected Avaliacao GetAvaliacaoFromDataReader(MySqlDataReader dr)
        {
            Avaliacao a = new Avaliacao();

            a.IdAvaliacao = (int)dr["idAvaliacao"];
            a.TipoAvaliacao = (int)dr["tipoAvaliacao"];
            a.Titulo = (string)dr["titulo"];
            a.Descricao = (string)dr["titulo"];
            a.Nota = (int)dr["nota"];
            a.DataCriacao = (DateTime)dr["dataCriacao"];

            if (dr["idProduto"] != DBNull.Value)
            {
                a.Produto = _produtoRepository.SearchProductById((int)dr["idProduto"]);
            }

            if (dr["idVendedor"] != DBNull.Value)
            {
                a.Vendedor = _vendedorRepository.getVendedorById((int)dr["idVendedor"]);
            }

            a.Cliente = new Cliente();
            a.Cliente.IdCliente = (int)dr["idCliente"];
            a.Cliente.User = _userRepository.GetUserFromCustomerCode(a.Cliente.IdCliente);

            return a;
        }
    }
}
