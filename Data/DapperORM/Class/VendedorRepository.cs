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
    public class VendedorRepository : BaseRepository, IVendedorRepository
    {
        public VendedorRepository() { }

        public Vendedor getVendedorById(int id)
        {
            using var db = GetMySqlConnection();

            const string sql = @"select * from Vendedor v where v.idVendedor = @id";

            Vendedor vendedor = db.Query<Vendedor>(sql, new { id }, commandType: CommandType.Text).FirstOrDefault();

            if (vendedor != null)
            {
                const string sqlUser = @"select id, name, latitude, longitude, noteApp, email, grr, telefone from User where Id = @idUser";
                vendedor.User = db.Query<User>(sqlUser, new { vendedor.IdUser }, commandType: CommandType.Text).FirstOrDefault();
                return vendedor;
            }

            return null;
        }

        public List<Vendedor> getVendedoresByName(string name)
        {
            List<Vendedor> ret = new List<Vendedor>();
            using var db = GetMySqlConnection();
            const string sql = @"select v.idVendedor, v.idUser, v.isOpen, v.isAtivo, u.id, u.name, u.latitude, u.longitude, u.noteApp, u.email, u.grr, u.telefone from Vendedor v " +
                "join User u on v.idUser = u.Id where u.Name like @name";
            MySqlCommand cmd = db.CreateCommand();

            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("name", "%" + name + "%");

            MySqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                ret.Add(GetCurrentVendedorFromDataReader(dr));
            }

            dr.Close();

            return ret;
        }

        public List<Vendedor> getAllSellers()
        {
            List<Vendedor> ret = new List<Vendedor>();
            using var db = GetMySqlConnection();
            const string sql = @"select v.idVendedor, v.idUser, v.isOpen, v.isAtivo, u.id, u.name, u.latitude, u.longitude, u.noteApp, u.email, u.grr, u.telefone from Vendedor v " +
                "join User u on v.idUser = u.Id where v.isAtivo = true";
            MySqlCommand cmd = db.CreateCommand();

            cmd.CommandText = sql;

            MySqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                ret.Add(GetCurrentVendedorFromDataReader(dr));
            }

            dr.Close();

            return ret;
        }

        public List<Vendedor> getVendedoresByLocation(float lat, float lon)
        {
            List<Vendedor> ret = new List<Vendedor>();
            using var db = GetMySqlConnection();
            string sql = @"SELECT * FROM (" +
                "SELECT v.idVendedor, v.idUser, v.isOpen, v.isAtivo, u.id, u.name, u.latitude, u.longitude, u.noteApp, u.email, u.grr, u.telefone, " +
                "3956 * ACOS(COS(RADIANS(@lat)) * COS(RADIANS(u.latitude)) * COS(RADIANS(@lon) - RADIANS(u.longitude)) + SIN(RADIANS(@lat)) * SIN(RADIANS(u.latitude))) AS distance " +
                "from Vendedor v " +
                "JOIN User u ON u.id = v.idUser " +
                "WHERE u.latitude BETWEEN @lat -(10 / 69) AND @lat +(10 / 69) " +
                "AND u.longitude BETWEEN @lon -(10 / (69 * COS(RADIANS(@lat)))) AND @lon +(10 / (69 * COS(RADIANS(@lat))))" +
                "AND v.isOpen = TRUE AND v.isAtivo = TRUE" +
                ") AS d WHERE d.distance < 0.8 ORDER BY d.distance ASC";

            MySqlCommand cmd = db.CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("lat", lat);
            cmd.Parameters.AddWithValue("lon", lon);

            MySqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                ret.Add(GetCurrentVendedorFromDataReader(dr));
            }

            dr.Close();

            return ret;
        }

        private Vendedor GetCurrentVendedorFromDataReader(MySqlDataReader dr)
        {
            Vendedor v = new Vendedor();
            v.User = new User();

            v.IdVendedor = (int)dr["idVendedor"];
            v.IdUser = (int)dr["idUser"];
            v.IsAtivo = (bool)dr["isAtivo"];
            v.IsOpen = (bool)dr["isOpen"];
            v.User.Id = (int)dr["id"];
            v.User.Name = (string)dr["name"];
            v.User.Telefone = (string)dr["telefone"];

            if (dr["latitude"] != DBNull.Value) v.User.Latitude = (float)dr["latitude"];
            if (dr["longitude"] != DBNull.Value) v.User.Longitude = (float)dr["longitude"];
            if (dr["noteApp"] != DBNull.Value) v.User.NoteApp = (float)dr["noteApp"];

            v.User.Email = (string)dr["email"];
            v.User.GRR = (string)dr["grr"];

            return v;
        }

        public int AddFavoriteSeller(int idVendedor, int idCliente)
        {
            try
            {
                using var db = GetMySqlConnection();
                const string sql1 = @"SELECT idCliente, idVendedor FROM VendedorFavorito WHERE idCliente = @idCliente AND idVendedor = @idVendedor";
                const string sql2 = @"INSERT INTO VendedorFavorito VALUES (@idVendedor, @idCliente)";

                if (db.Query(sql1, new { idCliente, idVendedor }).Any())
                {
                    throw new Exception("O Vendedor já está na lista de favoritos do Cliente informado!");
                }

                int ret = db.Execute(sql2, new { idVendedor, idCliente });

                db.Close();

                return ret;
            }
            catch(Exception ex)
            {
                throw new Exception("Houve um erro ao salvar o vendedor favorito: " + ex.Message);
            }
        }

        public int RemoveFavoriteSeller(int idVendedor, int idCliente)
        {
            try
            {
                using var db = GetMySqlConnection();
                const string sql = @"DELETE FROM VendedorFavorito WHERE idVendedor = @idVendedor AND idCliente = @idCliente";

                int ret = db.Execute(sql, new { idVendedor, idCliente });

                db.Close();

                return ret;
            }
            catch (Exception ex)
            {
                throw new Exception("Houve um erro ao salvar o vendedor favorito: " + ex.Message);
            }
        }

        public List<Vendedor> GetFavorteSellersByCustomer(int idCliente)
        {
            try
            {
                using var db = GetMySqlConnection();
                List<Vendedor> favorites = new List<Vendedor>();
                const string sql = @"SELECT idVendedor FROM VendedorFavorito WHERE idCliente = @idCliente";
                MySqlCommand cmd = db.CreateCommand();
                MySqlDataReader dr;

                cmd.Parameters.AddWithValue("idCliente", idCliente);
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
                    favorites.Add(getVendedorById((int)dr["idVendedor"]));
                }

                dr.Close();
                db.Close();

                return favorites;
            }
            catch (Exception ex)
            {
                throw new Exception("Houve um erro ao salvar o vendedor favorito: " + ex.Message);
            }
        }

        public int AddFormaPagamento(int idFormaPagamento, int idVendedor)
        {
            try
            {
                using var db = GetMySqlConnection();
                const string sql = @"INSERT INTO VendedorFormaPagamento VALUES (@idFormaPagamento, @idVendedor);";

                return db.Execute(sql, new { idFormaPagamento, idVendedor });
            }
            catch (Exception ex)
            {
                throw new Exception("Houve um erro ao vincular a forma de pagamento ao vendedor: " + ex.Message);
            }
        }

        public int RemoveFormaPagamento(int idFormaPagamento, int idVendedor)
        {
            try
            {
                using var db = GetMySqlConnection();
                const string sql = @"DELETE FROM VendedorFormaPagamento WHERE idFormaPagamento = @idFormaPagamento AND idVendedor = @idVendedor;";

                return db.Execute(sql, new { idFormaPagamento, idVendedor });
            }
            catch (Exception ex)
            {
                throw new Exception("Houve um erro ao remover a forma de pagamento do vendedor: " + ex.Message);
            }
        }

        public List<FormaDePagamento> GetFormaDePagamentoByVendedor(int idVendedor)
        {
            try
            {
                List<FormaDePagamento> ret = new List<FormaDePagamento>();
                using var db = GetMySqlConnection();
                MySqlCommand cmd = db.CreateCommand();
                const string sql = @"SELECT f.idFormaPagamento, descricao, icone FROM FormaPagamento f " + 
                    "JOIN VendedorFormaPagamento v on f.idFormaPagamento = v.idFormaPagamento " + 
                    "WHERE v.idVendedor = @idVendedor;";

                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("idVendedor", idVendedor);

                MySqlDataReader dr = cmd.ExecuteReader();

                if (!dr.HasRows)
                {
                    return ret;
                }

                while (dr.Read())
                {
                    ret.Add(new FormaDePagamento
                    {
                        IdFormaPagamento = (int)dr["idFormaPagamento"],
                        Descricao = (string)dr["descricao"],
                        Icone = (string)dr["icone"]
                    });
                }

                return ret;
            }
            catch (Exception ex)
            {
                throw new Exception("Houve um erro ao bucar as formas de pagamento do vendedor: " + ex.Message);
            }
        }
    }
}


