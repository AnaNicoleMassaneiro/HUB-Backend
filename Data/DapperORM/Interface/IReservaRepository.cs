using System;
using System.Collections.Generic;
using System.Text;

namespace HubUfpr.Data.DapperORM.Interface
{
    public interface IReservaRepository
    {
        void CreateReserve(int idCliente, int idProduto, int quantidade, float lat, float lon);

        int UpdateReserveStatus(int idReserve, int statusCode);

        int GetCurrentStatus(int idReserve);
    }
}
