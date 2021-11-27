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

        public List<Vendedor> getVendedoresByLocation(float lat, float lon)
        {
            return _vendedorRepository.getVendedoresByLocation(lat, lon);
        }

        public List<Vendedor> getVendedoresByName(string name)
        {
            return _vendedorRepository.getVendedoresByName(name);
        }

        public List<Vendedor> getAllSellers()
        {
            return _vendedorRepository.getAllSellers();
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

        public int AddFormaPagamento(int idFormaPagamento, int idVendedor)
        {
            return _vendedorRepository.AddFormaPagamento(idFormaPagamento, idVendedor);
        }

        public int RemoveFormaPagamento(int idFormaPagamento, int idVendedor)
        {
            return _vendedorRepository.RemoveFormaPagamento(idFormaPagamento, idVendedor);
        }

        public List<FormaDePagamento> GetFormaDePagamentoByVendedor(int idVendedor)
        {
            return _vendedorRepository.GetFormaDePagamentoByVendedor(idVendedor);
        }
    }
}
