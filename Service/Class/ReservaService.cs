using System;
using HubUfpr.Model;
using HubUfpr.Service.Interface;
using HubUfpr.Data.DapperORM.Interface;
using System.Collections.Generic;

namespace HubUfpr.Service.Class
{
    public class ReservaService : IReservaService
    {
        private readonly IReservaRepository _reservaRepository;

        public ReservaService(IReservaRepository repo)
        {
            _reservaRepository = repo;
        }
        
        public void CreateReserve(int idCliente, int idProduto, int quantidade, float lat, float lon)
        {
            _reservaRepository.CreateReserve(idCliente, idProduto, quantidade, lat, lon);
        }

        public int UpdateReserveStatus(int idReserve, int statusCode)
        {
            return _reservaRepository.UpdateReserveStatus(idReserve, statusCode);
        }

        public int GetCurrentStatus(int idReserve)
        {
            return _reservaRepository.GetCurrentStatus(idReserve);
        }
    }
}
