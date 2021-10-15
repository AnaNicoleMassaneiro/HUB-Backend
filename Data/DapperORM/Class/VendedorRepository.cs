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
                const string sqlUser = @"select id, name, latitude, longitude, noteApp, email, grr from User where Id = @idUser";
                vendedor.User = db.Query<User>(sqlUser, new { vendedor.IdUser }, commandType: CommandType.Text).FirstOrDefault();
                return vendedor;
            }

            return null;
        }

        public List<Vendedor> getVendedoresByLocation(float lat, float lon)
        {
            return new List<Vendedor>();
        }

        public List<Vendedor> getVendedoresByName(string name)
        {
            List<Vendedor> ret = new List<Vendedor>();
            using var db = GetMySqlConnection();
            const string sql = @"select v.idVendedor, v.idUser, v.isOpen, v.isAtivo, u.id, u.name, u.latitude, u.longitude, u.noteApp, u.email, u.grr from Vendedor v " +
                "join User u on v.idUser = u.Id where u.Name like @name";
            MySqlCommand cmd = db.CreateCommand();

            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("name", "%" + name + "%");

            MySqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                Vendedor v = new Vendedor();
                v.User = new User();

                v.IdVendedor = (int)dr["idVendedor"];
                v.IdUser = (int)dr["idUser"];
                v.IsAtivo = (bool)dr["isAtivo"];
                v.IsOpen = (bool)dr["isOpen"];
                v.User.Id = (int)dr["id"];
                v.User.Name = (string)dr["name"];

                if (dr["latitude"] != DBNull.Value) v.User.Latitude = (float)dr["latitude"];
                if (dr["longitude"] != DBNull.Value) v.User.Longitude = (float)dr["longitude"];
                if (dr["noteApp"] != DBNull.Value) v.User.NoteApp = (float)dr["noteApp"];

                v.User.Email = (string)dr["email"];
                v.User.GRR = (string)dr["grr"];

                ret.Add(v);
            }

            dr.Close();

            return ret;
        }
    }
}
