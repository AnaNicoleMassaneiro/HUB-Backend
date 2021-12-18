using System.Collections.Generic;
using HubUfpr.Model;

namespace HubUfpr.Service.Interface
{
    public interface IReservaService
    {
        void CreateReserve(int idCliente, int idProduto, int quantidade, float lat, float lon);

        int UpdateReserveStatus(int idReserve, int statusCode);

        int GetCurrentStatus(int idReserve);

        List<Reserva> GetReservasByVendedor(int idVendedor);

        List<Reserva> GetReservasByCliente(int idCliente);

        int GetSellerIdFromReservation(int id);

        int GetCustomerIdFromReservation(int id);
    }
}