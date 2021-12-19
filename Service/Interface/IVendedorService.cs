using System.Collections.Generic;
using HubUfpr.Model ;

namespace HubUfpr.Service.Interface
{
    public interface IVendedorService
    {
        Vendedor getVendedorById(int id);

        List<Vendedor> getVendedoresByName(string name, int ignoreSellerId);

        List<Vendedor> getVendedoresByLocation(float lat, float lon, int ignoreSellerId);

        List<Vendedor> getAllSellers(int ignoreSellerId);

        int AddFavoriteSeller(int idVendedor, int idCliente);

        int RemoveFavoriteSeller(int idVendedor, int idCliente);

        List<Vendedor> GetFavorteSellersByCustomer(int idCliente);

        bool IsVendedorInCustomerFavorites(int idCliente, int idVendedor);

        int UpdateSellerStatus(int idVendedor, bool isAtivo, bool isOpen);
    }
}
