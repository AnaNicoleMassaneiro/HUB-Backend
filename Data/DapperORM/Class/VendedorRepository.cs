﻿using System;
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
                ret.Add(GetCurrentVendedorFromDataReader(dr));
            }

            dr.Close();

            return ret;
        }

        public List<Vendedor> getAllSellers()
        {
            List<Vendedor> ret = new List<Vendedor>();
            using var db = GetMySqlConnection();
            const string sql = @"select v.idVendedor, v.idUser, v.isOpen, v.isAtivo, u.id, u.name, u.latitude, u.longitude, u.noteApp, u.email, u.grr from Vendedor v " +
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
                "SELECT v.idVendedor, v.idUser, v.isOpen, v.isAtivo, u.id, u.name, u.latitude, u.longitude, u.noteApp, u.email, u.grr, " +
                "3956 * ACOS(COS(RADIANS(@lat)) * COS(RADIANS(u.latitude)) * COS(RADIANS(@lon) - RADIANS(u.longitude)) + SIN(RADIANS(@lat)) * SIN(RADIANS(u.latitude))) AS distance " +
                "from Vendedor v " +
                "JOIN User u ON u.id = v.idUser " +
                "WHERE u.latitude BETWEEN @lat -(10 / 69) AND @lat +(10 / 69) " +
                "AND u.longitude BETWEEN @lon -(10 / (69 * COS(RADIANS(@lat)))) AND @lon +(10 / (69 * COS(RADIANS(@lat))))" +
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

            if (dr["latitude"] != DBNull.Value) v.User.Latitude = (float)dr["latitude"];
            if (dr["longitude"] != DBNull.Value) v.User.Longitude = (float)dr["longitude"];
            if (dr["noteApp"] != DBNull.Value) v.User.NoteApp = (float)dr["noteApp"];

            v.User.Email = (string)dr["email"];
            v.User.GRR = (string)dr["grr"];

            return v;
        }
    }
}


