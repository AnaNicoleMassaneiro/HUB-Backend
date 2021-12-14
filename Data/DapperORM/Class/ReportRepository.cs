using Dapper;
using System.Linq;
using HubUfpr.Data.DapperORM.Interface;
using HubUfpr.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace HubUfpr.Data.DapperORM.Class
{
    public class ReportRepository : BaseRepository, IReportRepository
    {
        public ReportHeader ReportHeader(string type, int idVendedor, string dateFilter) 
        {
            using var db = GetMySqlConnection();
            string sql = @"SELECT COUNT(r.idReserva) AS 'TotalReservas', TRUNCATE(SUM(p.preco * r.quantidadeDesejada), 2) AS 'ValorTotal', " +
                "SUM(r.quantidadeDesejada) AS 'QuantidadeItens', TRUNCATE(SUM(p.preco * r.quantidadeDesejada) / COUNT(r.idReserva), 2) AS 'TicketMedio' " +
                "FROM Reserva r JOIN Produto p on p.idProduto = r.idProduto JOIN StatusReserva sr on sr.codigo = r.statusReserva ";

            if (type.ToUpper() == "RESERVAS") sql += "WHERE sr.descricao != 'CONCLUIDA' ";
            else if (type.ToUpper() == "VENDAS") sql += "WHERE sr.descricao = 'CONCLUIDA' ";
            else throw new Exception("O tipo de relatório informado é inválido!");

            sql += "AND p.idVendedor = @idVendedor";

            if (dateFilter != null && dateFilter.Trim().Length > 0)
            {
                switch (dateFilter.ToUpper()) {
                    case "WEEK":
                        sql += " AND DATEDIFF(NOW(), r.dataCriacao) BETWEEN 0 AND 7";
                        break;
                    case "MONTH":
                        sql += " AND DATEDIFF(NOW(), r.dataCriacao) BETWEEN 0 AND 30";
                        break;
                    case "SEMESTER":
                        sql += " AND DATEDIFF(NOW(), r.dataCriacao) BETWEEN 0 AND 180";
                        break;
                    default: throw new Exception("O tipo de filtro de data informado é inválido!");
                }
            }

            var ret = db.Query<ReportHeader>(sql, new { idVendedor }).First();

            db.Close();

            return ret;
        }

        public List<ReportData> ReportData(string type, int idVendedor, string dateFilter)
        {
            using var db = GetMySqlConnection();
            List<ReportData> ret = new List<ReportData>();
            string sql = @"SELECT p.nome AS 'Produto', COUNT(r.idReserva) AS 'TotalReservas', SUM(r.quantidadeDesejada) AS 'QuantidadeVendida', " +
                "TRUNCATE(SUM(p.preco * r.quantidadeDesejada), 2) AS 'ValorTotal' FROM Reserva r " +
                "JOIN Produto p on p.idProduto = r.idProduto " +
                "JOIN StatusReserva sr on sr.codigo = r.statusReserva ";

            if (type.ToUpper() == "RESERVAS") sql += "WHERE sr.descricao != 'CONCLUIDA' ";
            else if (type.ToUpper() == "VENDAS") sql += "WHERE sr.descricao = 'CONCLUIDA' ";
            else throw new Exception("O tipo de relatório informado é inválido!");

            sql += "AND p.idVendedor = @idVendedor";

            if (dateFilter != null && dateFilter.Trim().Length > 0)
            {
                switch (dateFilter.ToUpper())
                {
                    case "WEEK":
                        sql += " AND DATEDIFF(NOW(), r.dataCriacao) BETWEEN 0 AND 7";
                        break;
                    case "MONTH":
                        sql += " AND DATEDIFF(NOW(), r.dataCriacao) BETWEEN 0 AND 30";
                        break;
                    case "SEMESTER":
                        sql += " AND DATEDIFF(NOW(), r.dataCriacao) BETWEEN 0 AND 180";
                        break;
                    default: throw new Exception("O tipo de filtro de data informado é inválido!");
                }
            }

            sql += " GROUP BY p.nome;";

            MySqlCommand cmd = db.CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("idVendedor", idVendedor);

            MySqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    ret.Add(new ReportData
                    {
                        Produto = (string)dr["Produto"],
                        QuantidadeReservas = Int32.Parse(dr["TotalReservas"].ToString()),
                        ValorTotal = float.Parse(dr["ValorTotal"].ToString()),
                        QuantidadeItens = Decimal.ToInt32(Decimal.Parse(dr["QuantidadeVendida"].ToString()))
                    });
                }
            }

            dr.Close();
            db.Close();

            return ret;
        }
    }
}
