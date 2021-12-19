using System;
using HubUfpr.Model;
using HubUfpr.Service.Interface;
using HubUfpr.Data.DapperORM.Interface;
using System.Collections.Generic;

namespace HubUfpr.Service.Class
{
    public class VendedorService : IVendedorService
    {
        private readonly IVendedorRepository _vendedorRepository;

        public VendedorService(IVendedorRepository vendedorRepository)
        {
            _vendedorRepository = vendedorRepository;
        }

        public Vendedor getVendedorById(int id)
        {
            return _vendedorRepository.getVendedorById(id);
        }

        public List<Vendedor> getVendedoresByLocation(float lat, float lon, int ignoreSellerId)
        {
            return _vendedorRepository.getVendedoresByLocation(lat, lon, ignoreSellerId);
        }

        public List<Vendedor> getVendedoresByName(string name, int ignoreSellerId)
        {
            return _vendedorRepository.getVendedoresByName(name, ignoreSellerId);
        }

        public List<Vendedor> getAllSellers(int ignoreSellerId)
        {
            return _vendedorRepository.getAllSellers(ignoreSellerId);
        }

        public int AddFavoriteSeller(int idVendedor, int idCliente)
        {
            return _vendedorRepository.AddFavoriteSeller(idVendedor, idCliente);
        }

        public int RemoveFavoriteSeller(int idVendedor, int idCliente)
        {
            return _vendedorRepository.RemoveFavoriteSeller(idVendedor, idCliente);
        }

        public List<Vendedor> GetFavorteSellersByCustomer(int idCliente)
        {
            return _vendedorRepository.GetFavorteSellersByCustomer(idCliente);
        }

        public bool IsVendedorInCustomerFavorites(int idCliente, int idVendedor)
        {
            return _vendedorRepository.IsVendedorInCustomerFavorites(idCliente, idVendedor);
        }

        public int UpdateSellerStatus(int idVendedor, bool isAtivo, bool isOpen)
        {
            return _vendedorRepository.UpdateSellerStatus(idVendedor, isAtivo, isOpen);
        }
    }
}
