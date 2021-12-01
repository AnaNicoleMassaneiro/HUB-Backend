using System;
using System.Collections.Generic;
using Dapper;
using HubUfpr.Data.DapperORM.Interface;
using HubUfpr.Model;
using MySql.Data.MySqlClient;

namespace HubUfpr.Data.DapperORM.Class
{
    public class FormaDePagamentoRepository : BaseRepository, IFormaDePagamentoRepository
    {
        public FormaDePagamentoRepository() { }

        public int AddFormaPagamento(int idFormaPagamento, int idVendedor)
        {
            try
            {
                using var db = GetMySqlConnection();
                const string sql = @"INSERT INTO VendedorFormaPagamento VALUES (@idFormaPagamento, @idVendedor);";

                var ret = db.Execute(sql, new { idFormaPagamento, idVendedor });

                db.Close();

                return ret;
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

                var ret = db.Execute(sql, new { idFormaPagamento, idVendedor });

                db.Close();

                return ret;
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
                    dr.Close();
                    db.Close();
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

                dr.Close();
                db.Close();
                return ret;
            }
            catch (Exception ex)
            {
                throw new Exception("Houve um erro ao bucar as formas de pagamento do vendedor: " + ex.Message);
            }
        }

        public List<FormaDePagamento> ListFormaDePagamento()
        {
            List<FormaDePagamento> ret = new List<FormaDePagamento>();
            using var db = GetMySqlConnection();
            MySqlCommand cmd = db.CreateCommand();
            const string sql = @"SELECT idFormaPagamento, descricao, icone FROM FormaPagamento";

            cmd.CommandText = sql;

            MySqlDataReader dr = cmd.ExecuteReader();

            if (!dr.HasRows)
            {
                dr.Close();
                db.Close();
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

            dr.Close();
            db.Close();
            return ret;
        }
    }
}


