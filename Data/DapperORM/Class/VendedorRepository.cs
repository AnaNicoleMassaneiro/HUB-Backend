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

            db.Close();

            return null;
        }

        public List<Vendedor> getVendedoresByName(string name, int ignoreSellerId)
        {
            List<Vendedor> ret = new List<Vendedor>();
            using var db = GetMySqlConnection();
            string sql = @"SELECT v.idVendedor, v.idUser, v.isOpen, v.isAtivo, " + 
                "u.id, u.name, u.latitude, u.longitude, u.noteApp, u.email, u.grr, u.telefone FROM Vendedor v " +
                "JOIN User u ON v.idUser = u.Id WHERE u.Name like @name AND v.isAtivo = TRUE and v.isOpen = TRUE ";
            MySqlCommand cmd = db.CreateCommand();

            if (ignoreSellerId > 0)
            {
                sql += " AND v.idVendedor != @ignoreSellerId";
                cmd.Parameters.AddWithValue("ignoreSellerId", ignoreSellerId);
            }

            sql += " ORDER BY u.name ASC";

            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("name", "%" + name + "%");

            MySqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                ret.Add(GetCurrentVendedorFromDataReader(dr));
            }

            db.Close();
            dr.Close();

            return ret;
        }

        public List<Vendedor> getAllSellers(int ignoreSellerId)
        {
            List<Vendedor> ret = new List<Vendedor>();
            using var db = GetMySqlConnection();
            string sql = @"SELECT v.idVendedor, v.idUser, v.isOpen, v.isAtivo, u.id, " + 
                "u.name, u.latitude, u.longitude, u.noteApp, u.email, u.grr, u.telefone FROM Vendedor v " +
                "JOIN User u ON v.idUser = u.Id AND v.isAtivo = TRUE and v.isOpen = TRUE ";
            MySqlCommand cmd = db.CreateCommand();

            if (ignoreSellerId > 0)
            {
                sql += " AND v.idVendedor != @ignoreSellerId";
                cmd.Parameters.AddWithValue("ignoreSellerId", ignoreSellerId);
            }

            sql += " ORDER BY u.name ASC";

            cmd.CommandText = sql;

            MySqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                ret.Add(GetCurrentVendedorFromDataReader(dr));
            }

            db.Close();
            dr.Close();

            return ret;
        }

        public List<Vendedor> getVendedoresByLocation(float lat, float lon, int ignoreSellerId)
        {
            List<Vendedor> ret = new List<Vendedor>();
            using var db = GetMySqlConnection();
            MySqlCommand cmd = db.CreateCommand();
            string sql = @"SELECT * FROM (" +
                "SELECT v.idVendedor, v.idUser, v.isOpen, v.isAtivo, u.id, u.name, u.latitude, u.longitude, u.noteApp, u.email, u.grr, u.telefone, " +
                "3956 * ACOS(COS(RADIANS(@lat)) * COS(RADIANS(u.latitude)) * COS(RADIANS(@lon) - RADIANS(u.longitude)) + SIN(RADIANS(@lat)) * SIN(RADIANS(u.latitude))) AS distance " +
                "from Vendedor v " +
                "JOIN User u ON u.id = v.idUser " +
                "WHERE u.latitude BETWEEN @lat -(10 / 69) AND @lat +(10 / 69) " +
                "AND u.longitude BETWEEN @lon -(10 / (69 * COS(RADIANS(@lat)))) AND @lon +(10 / (69 * COS(RADIANS(@lat))))" +
                "AND v.isOpen = TRUE AND v.isAtivo = TRUE";
            
            if (ignoreSellerId > 0)
            {
                sql += " AND v.idVendedor != @ignoreSellerId";
                cmd.Parameters.AddWithValue("ignoreSellerId", ignoreSellerId);
            }

            sql += ") AS d WHERE d.distance < 0.8 ORDER BY d.distance ASC";

            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("lat", lat);
            cmd.Parameters.AddWithValue("lon", lon);

            MySqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                ret.Add(GetCurrentVendedorFromDataReader(dr));
            }

            db.Close();
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
            if (dr["telefone"] != DBNull.Value) v.User.Telefone = (string)dr["telefone"];

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

                favorites = favorites.OrderBy(f => f.User.Name).ToList();
                
                return favorites;
            }
            catch (Exception ex)
            {
                throw new Exception("Houve um erro ao salvar o vendedor favorito: " + ex.Message);
            }
        }

        public bool IsVendedorInCustomerFavorites(int idCliente, int idVendedor)
        {
            using var db = GetMySqlConnection();
            const string sql = @"SELECT idVendedor FROM VendedorFavorito WHERE idVendedor = @idVendedor AND idCliente = @idCliente;";

            var ret = db.Query(sql, new { idVendedor, idCliente }).Any();

            db.Close();

            return ret;
        }

        public int UpdateSellerStatus(int idVendedor, bool isAtivo, bool isOpen)
        {
            using var db = GetMySqlConnection();
            const string sql = @"UPDATE Vendedor SET isAtivo = @isAtivo, isOpen = @isOpen WHERE idVendedor = @idVendedor";
            int ret = db.Execute(sql, new { isAtivo, isOpen, idVendedor });

            db.Close();

            return ret;
        }
    }
}


