using Dapper;
using HubUfpr.Data.DapperORM.Interface;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System;

namespace HubUfpr.Data.DapperORM.Class
{
    public class ReservaRepository : BaseRepository, IReservaRepository
    {
        private readonly IProdutoRepository _produtoRepository;

        public ReservaRepository(IProdutoRepository repo)
        {
            _produtoRepository = repo;
        }

        public void CreateReserve(int idCliente, int idProduto, int quantidade, float lat, float lon)
        {
            using var db = GetMySqlConnection();
            const string sql = @"INSERT INTO Reserva (idCliente, idProduto, statusReserva, quantidadeDesejada, localizacaoLat, localizacaoLong) " +
                "VALUES (@idCliente, @idProduto, 0, @quantidade, @lat, @lon);";

            db.Execute(sql, new { idCliente, idProduto, quantidade, lat, lon }, commandType: CommandType.Text);

            int newQt = _produtoRepository.GetCurrentAvailableAmount(idProduto) - quantidade;

            int ret = _produtoRepository.UpdateCurrentAvailableAmount(idProduto, newQt);

            if (ret == 0)
            {
                throw new System.Exception("Houve um erro ao atualziar a quantidade disponível do produto após a criação da reserva.");
            }
        }

        public int UpdateReserveStatus(int idReserve, int statusCode)
        {
            using var db = GetMySqlConnection();
            const string sql = @"UPDATE Reserva set statusReserva = @status WHERE idReserva = @idReserva";

            MySqlCommand cmd = db.CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("idReserva", idReserve);
            cmd.Parameters.AddWithValue("status", statusCode);

            int ret = cmd.ExecuteNonQuery();

            db.Close();

            if (statusCode == 2 || statusCode == 3)
            {
                int[] data = GetProductAndAmountFromReserve(idReserve);

                int newQt = _produtoRepository.GetCurrentAvailableAmount(data[0]) + data[1];

                int update = _produtoRepository.UpdateCurrentAvailableAmount(data[0], newQt);

                if (update == 0)
                {
                    throw new System.Exception("Houve um erro ao atualizar a quantidade disponível do produto após atualização da reserva.");
                }
            }

            return ret;
        }

        public int GetCurrentStatus(int idReserve)
        {
            using var db = GetMySqlConnection();
            const string sql = @"SELECT statusReserva FROM Reserva WHERE idReserva = @idReserve";
            int ret = db.Query<int>(sql, new { idReserve }, commandType: CommandType.Text).FirstOrDefault();

            db.Close();

            return ret;
        }

        private int[] GetProductAndAmountFromReserve(int idReserve)
        {
            using var db = GetMySqlConnection();
            const string sql = @"SELECT idProduto, quantidadeDesejada from Reserva WHERE idReserva = @idReserve;";
            int[] arr = new int[2];
            var ret = db.Query(sql, new { idReserve }).First();

            db.Close();

            arr[0] = ret.idProduto;
            arr[1] = ret.quantidadeDesejada;

            return arr;
        }
    }
}
